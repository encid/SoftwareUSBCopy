/* 
 * 
 * USB Software Loader
 * Designed by R. Cavallaro
 * 
 */

using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Specialized;

namespace USBSoftwareLoader
{
    public partial class Main : Form
    {
        public string VAULT_PATH = ConfigurationManager.AppSettings["VAULT_PATH"];
        public string FORCE_UPDATE_FILE = Application.StartupPath + ConfigurationManager.AppSettings["FORCE_UPDATE_FILE"];
        public string INI_FILE = ConfigurationManager.AppSettings["INI_FILE"];
        public int CurrDriveCount { get; set; }
        public Dictionary<string, string> AssemblyDict = new Dictionary<string, string>();
        public Dictionary<string, string> DescriptionDict = new Dictionary<string, string>();
        public BackgroundWorker bw;
        public Timer tmrRefresh = new Timer();

        public class CopyParams
        {
            public string Source1 { get; set; }
            public string Source2 { get; set; }
            public List<string> Destinations { get; set; }
            public string SoftwarePartNumber { get; set; }
            public string Ecl1 { get; set; }
            public string Ecl2 { get; set; }
            public bool EraseUsb { get; set; }

            public CopyParams(string _Source1, string _Source2, List<string> _Destinations, string _SoftwarePartNumber, string _Ecl1, string _Ecl2, bool _EraseUsb)
            {
                Ecl1 = _Ecl1;
                Ecl2 = _Ecl2;
                SoftwarePartNumber = _SoftwarePartNumber;
                Source1 = _Source1;
                Source2 = _Source2;
                Destinations = _Destinations;
                EraseUsb = _EraseUsb;
            }
        }

        public Main()
        {
            InitializeComponent();
            InitializeEventHandlers();

            // import list of assembly/software from config file to dict
            //if (ReadSoftwareSettingsToDict() != null)
            //{
            AssemblyDict = ReadSoftwareSettingsToDict();
            DescriptionDict = ReadSoftwareDescriptionsToDict();
            //}            

            // Populate combobox of assemblies
            if (AssemblyDict.Count > 0)
            {
                foreach (var x in AssemblyDict)
                {
                    cboAssembly.Items.Add(x.Key);
                }

                cboAssembly.SelectedIndex = 0;
            }

            Logger.Log($"Kitchen Brains USB Software Loader version: {ProductVersion}", rt);
            Logger.Log($"Imported {cboAssembly.Items.Count} software part numbers.", rt);

            // Add (destination) removable drives to ListView
            PopulateListView(lvDrives, true);

            CurrDriveCount = lvDrives.Items.Count;
        }

        public void InitializeEventHandlers()
        {
            // Initialize Timer
            tmrRefresh.Enabled = true;
            tmrRefresh.Interval = 750;
            tmrRefresh.Tick += (s, e) =>
            {
                var drives = GetRemovableDrives();

                if (drives.Count() != CurrDriveCount)
                    PopulateListView(lvDrives, true);

                CurrDriveCount = drives.Count();
            };

            // Tick the checkbox if any part of the item line in ListView is clicked
            lvDrives.MouseClick += (s, e) =>
            {
                var lvi = lvDrives.GetItemAt(e.X, e.Y);
                if (e.X > 16) lvi.Checked = !lvi.Checked;
            };

            // Add columns to the ListView
            lvDrives.Columns.Add("Drive", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Volume Name", 110, HorizontalAlignment.Left);
            lvDrives.Columns.Add("File System", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Capacity", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Free Space", -2, HorizontalAlignment.Left);

            // Disallow changing width on ListView columns
            lvDrives.ColumnWidthChanging += (s, e) =>
            {
                e.NewWidth = this.lvDrives.Columns[e.ColumnIndex].Width;
                e.Cancel = true;
            };

            // Set up BackgroundWorker
            bw = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            // Make sure textbox stays at the most recent line(bottom most)
            rt.TextChanged += (s, e) =>
            {
                if (rt.Visible)
                    rt.ScrollToCaret();
            };
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //set default config values if paths are null or empty (config file didn't load)
            if (string.IsNullOrEmpty(VAULT_PATH))
            {
                VAULT_PATH = @"V:\";
            }
            if (string.IsNullOrEmpty(FORCE_UPDATE_FILE))
            {
                FORCE_UPDATE_FILE = Application.StartupPath + @"\files";
            }
            if (string.IsNullOrEmpty(INI_FILE))
            {
                INI_FILE = @"\\kb-fp02\shared\Operations\Test Engineering\Test Softwares\USB Software Loader\software.ini";
            }
        }

        /// <summary>
        /// Gets a list of checked items (drive letters as string) from the specified ListView control.
        /// </summary>
        /// <param name="lView"></param>
        /// <returns></returns>
        private List<string> GetDestinationDirs(ListView lView)
        {
            var d = new List<string>();

            foreach (ListViewItem item in lView.CheckedItems)
            {
                d.Add(item.Text);
            }

            return d;
        }

        /// <summary>
        /// Format a long number into a readable string; i.e. input: 15520 output: "15.5 KB"
        /// </summary>
        /// <param name="byteCount">Number of bytes.</param>
        /// <returns></returns>
        private string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        /// <summary>
        /// Performs an action safely the appropriate thread.
        /// </summary>
        /// <param name="a">Action to perform.</param>
        private void ExecuteSecure(Action a)
        // Usage example: ExecuteSecure(() => this.someLabel.Text = "foo");
        {
            BeginInvoke(a);
        }

        /// <summary>
        /// Set CheckState for all items in a ListView.
        /// </summary>
        /// <param name="lview">ListView to set CheckState on.</param>
        /// <param name="choice">Set CheckState; Checked = True and Unchecked = False.</param>
        private void SetListViewCheckState(ListView lview, bool choice)
        // Check or uncheck all items in CheckedListBox, choice = false for uncheck, choice = true for check
        {
            foreach (ListViewItem item in lview.Items)
                item.Checked = choice;
        }

        /// <summary>
        /// Retrieves a collection of removable drives that are ready.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DriveInfo> GetRemovableDrives()
        {
            return DriveInfo.GetDrives().Where(p => p.DriveType == DriveType.Removable && p.IsReady);
        }

        /// <summary>
        /// Gets the latest ECL revision of a software part number.
        /// </summary>
        /// <param name="softwarePartNumber"></param>
        /// <returns></returns>
        private string GetECL(string softwarePartNumber)
        {
            int dashIndex;
            string tempECLStr;
            string swDir;
            string softwarePartOne = softwarePartNumber.Substring(4, 5);

            try
            {
                // Determine which folder we're looking in, 91XXX or 94XXX
                if (softwarePartOne.Substring(0, 2) == "91")
                {
                    swDir = @"\240-91XXX-XX\";
                }
                else
                {
                    swDir = @"\240-94XXX-XX\";
                }

                var softwareDir = (from dir in Directory.GetDirectories($@"{VAULT_PATH}\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\{swDir}\240-{softwarePartOne}-XX\")
                                   where dir.Contains(softwarePartNumber)
                                   select dir)
                                  .FirstOrDefault();

                var eclDir = (from dir in Directory.GetDirectories(softwareDir)
                              where dir.Contains("ECL")
                              select dir)
                             .FirstOrDefault();

                tempECLStr = eclDir.Substring(eclDir.Length - 6);
                dashIndex = tempECLStr.IndexOf("-", StringComparison.CurrentCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return string.Empty;
            }

            return tempECLStr.Substring(dashIndex + 1);
        }

        /// <summary>
        /// Detects removable drives and adds them to ListView.
        /// </summary>
        /// <param name="lView">ListView to populate.</param>
        /// <param name="updateLog">Boolean choice to send update to status log.</param>
        private void PopulateListView(ListView lView, bool updateLog)
        {
            if (updateLog)
                Logger.Log("\nScanning for removable drives...", rt);

            lView.Items.Clear();

            var drives = GetRemovableDrives();

            // If no removable drives detected, exit method
            if (!drives.Any())
            {
                if (updateLog)
                    Logger.Log("No removable drives detected", rt);
                return;
            }

            List<string> driveNames = new List<string>();
            // Iterate through collection and add each removable drive as to ListView as items and subitems
            foreach (var drive in drives)
            {
                driveNames.Add(drive.Name);
                var freeSpace = BytesToString(drive.TotalFreeSpace);
                var totalSpace = BytesToString(drive.TotalSize);
                var oItem = new ListViewItem
                {
                    Text = drive.Name
                };
                oItem.SubItems.Add(drive.VolumeLabel);
                oItem.SubItems.Add(drive.DriveFormat);
                oItem.SubItems.Add(totalSpace);
                oItem.SubItems.Add(freeSpace);

                lView.Items.Add(oItem);
            }

            // Get count and names of drives found and log it to status
            if (updateLog)
            {
                var logStr = $"Detected {drives.Count()} removable {"drive".Pluralize(drives.Count())}: {string.Join(", ", driveNames)}";
                Logger.Log(logStr, rt);
            }
        }

        /// <summary>
        /// Validates the UI input parameters for copying, and throws exceptions based on parameters.
        /// </summary>
        /// <param name="srcDir">Source directory to copy from.</param>
        /// <param name="destDirs">Destination directories in list collection.</param>
        private void ValidateCopyParams(string srcDir, List<string> destDirs)
        {
            // Check if source path exists
            if (!Directory.Exists(srcDir))
                throw new Exception("Source drive is not ready. Please try again.");

            // If no drives are checked in CheckedListBox, exit method 
            if (destDirs.Count == 0)
                throw new Exception("Please select at least one destination drive and try again.");

            // Check to make sure source drive and destination drive are not the same, exit if true
            // Check to make sure user did not remove drive after refresh, but before copy
            foreach (var destDir in destDirs)
            {
                if (srcDir.Substring(0, 1) == destDir.Substring(0, 1))
                    throw new Exception("Source drive and destination drive cannot be the same. Please try again.");
                if (!Directory.Exists(destDir))
                    throw new Exception("Target destination drive does not exist. Please try again.");
            }
        }

        private string GetFullSoftwarePath(string partNumber)
        {
            string middle = partNumber.Substring(4, 5);
            string softwareDir = "";

            switch (middle.Substring(0, 2))
            {
                case "91":
                softwareDir += $@"{VAULT_PATH}\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\240-91XXX-XX\240-{middle}-XX\{partNumber}";
                break;
                case "92":
                softwareDir += $@"{VAULT_PATH}\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\240-92XXX-XX\240-{middle}-XX\{partNumber}";
                break;
                case "93":
                softwareDir += $@"{VAULT_PATH}\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\240-93XXX-XX\240-{middle}-XX\{partNumber}";
                break;
                case "94":
                softwareDir += $@"{VAULT_PATH}\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\240-94XXX-XX\240-{middle}-XX\{partNumber}";
                break;
            }

            return softwareDir;
        }

        private void btnStartCopy_click(object sender, EventArgs e)
        {
            var ECL2 = "";
            var softwarePartNumber1 = "";
            var softwareDir2 = "";
            bool eraseUSB = true;
            PictureBox1.Image = Properties.Resources.questionmark;

            if (!Directory.Exists(VAULT_PATH))
            {
                Logger.Log(@"Error: Cannot find vault (V:\ drive).  Check network connection and try again.", rt, Color.Red);
                return;
            }

            if (cboAssembly.Items.Count == 0)
            {
                Logger.Log("Error: No software part numbers found.  Check config file.", rt, Color.Red);
                return;
            }

            // if CTRL is held while clicking the Copy button, the USB drive will not be erased before copying.
            eraseUSB &= Control.ModifierKeys != Keys.Control;

            try
            {
                if (AssemblyDict.TryGetValue(cboAssembly.Text, out string value))
                {
                    softwarePartNumber1 = value;
                }

                // if there are two softwares for that assembly, parse them
                if (softwarePartNumber1.Contains(";"))
                {
                    //string softwarePartNumber2 = softwarePartNumber1.Substring(softwarePartNumber1.IndexOf(";") + 1);
                    //softwarePartNumber1 = softwarePartNumber1.Substring(0, softwarePartNumber1.IndexOf(";"));
                    //ECL2 = GetECL(softwarePartNumber2);
                    //string softwareTopDir2 = GetFullSoftwarePath(softwarePartNumber2);
                    //softwareDir2 = $@"{softwareTopDir2}\ECL-{ECL2}";

                    string[] SoftwarePartNums = softwarePartNumber1.Split(';');
                    softwarePartNumber1 = SoftwarePartNums[0];
                    string softwarePartNumber2 = SoftwarePartNums[1];
                    ECL2 = GetECL(softwarePartNumber2);
                    string softwareTopDir2 = GetFullSoftwarePath(softwarePartNumber2);
                    softwareDir2 = $@"{softwareTopDir2}\ECL-{ECL2}";
                }

                string ECL1 = GetECL(softwarePartNumber1);
                string softwareTopDir1 = GetFullSoftwarePath(softwarePartNumber1);
                string softwareDir1 = $@"{softwareTopDir1}\ECL-{ECL1}";

                // Validate user input on UI
                var cp = new CopyParams(softwareDir1, softwareDir2, GetDestinationDirs(lvDrives), softwarePartNumber1, ECL1, ECL2, eraseUSB);
                ValidateCopyParams(cp.Source1, cp.Destinations);

                // No exceptions, so continue....
                Logger.Log($"Latest software found: {softwarePartNumber1} ECL-{ECL1}", rt);

                // Show warning that drive will be erased
                if (eraseUSB)
                {
                    if (MessageBox.Show(new Form { TopMost = true }, "The selected USB drive will be erased!\nDo you wish to continue?",
                                                                     "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                                                     == DialogResult.No)
                    {
                        Logger.Log("Software loading operation cancelled.", rt, Color.Red);
                        return;
                    }
                }

                // Disable UI controls
                DisableUI();

                // Begin the copy in BackgroundWorker, pass CopyParams object into it
                bw.RunWorkerAsync(cp);
            }
            catch (Exception ex)
            {
                var logStr = $"Error: {ex.Message}";
                Logger.Log(logStr, rt, Color.Red);
                if (ex.Message.Contains("Target destination drive does not exist"))
                    PopulateListView(lvDrives, false);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var cp = (CopyParams)e.Argument;

            try
            {
                PerformCopy(cp.Source1, cp.Source2, cp.Destinations, cp.SoftwarePartNumber, cp.Ecl1, cp.Ecl2, cp.EraseUsb);
            }
            catch (ArgumentException)
            {  // Catch user removal of drive during copy for RunWorkerCompleted to process
            }
            catch (OperationCanceledException)
            {  // Catch user cancelling copy for RunWorkerCompleted to process
                e.Cancel = true;
            }
            finally
            {
                e.Result = cp;
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user cancelled the operation.
                Logger.Log("Software loading has been cancelled.", rt, Color.Red);
                this.BringToFront();
                this.Focus();
            }
            else if (e.Error != null)
            {
                // There was an error during the operation
                PopulateListView(lvDrives, false);
                var logStr = $"An error has occured: {e.Error.Message}";
                Logger.Log(logStr, rt, Color.Red);
                MessageBox.Show($"An error has occured: {e.Error.Message}", "USB Software Loader", MessageBoxButtons.OK);
                this.BringToFront();
                this.Focus();
            }
            else
            {
                // The operation completed normally.
                PictureBox1.Image = Properties.Resources.check;
                var cp = (CopyParams)e.Result;
                var logStr = $"Software {cp.SoftwarePartNumber} ECL-{cp.Ecl1} was successfully loaded!";
                Logger.Log(logStr, rt, Color.Green);
            }

            // Enable UI controls            
            EnableUI();
            PopulateListView(lvDrives, false);
        }

        /// <summary>
        /// Begins a copy operation.
        /// </summary>
        /// <param name="sourceDir1"></param>
        /// <param name="sourceDir2"></param>
        /// <param name="destDirs"></param>
        /// <param name="softwarePartNumber"></param>
        /// <param name="ECL1"></param>
        /// <param name="ECL2"></param>
        /// <param name="eraseUSB"></param>
        private void PerformCopy(string sourceDir1, string sourceDir2, List<string> destDirs, string softwarePartNumber, string ECL1, string ECL2, bool eraseUSB)
        {
            // Start copy execution
            for (int i = 0; i < destDirs.Count; i++)
            {
                var dInfo = new DriveInfo(destDirs[i].Substring(0, 1))
                {
                    // Set volume label
                    VolumeLabel = $"{softwarePartNumber.Substring(4, 8)} {ECL1}"
                };

                // if eraseUSB = true, delete all files and directories of root dir on removable drive
                if (eraseUSB)
                {
                    ExecuteSecure(() => Logger.Log($"\nErasing removable drive {destDirs[i]}...", rt));
                    DeleteDirContents(destDirs[i]);
                }

                // Begin copying
                ExecuteSecure(() => Logger.Log($"Starting to copy {softwarePartNumber} ECL-{ECL1} to {destDirs[i]}...", rt));
                FileSystem.CopyDirectory(sourceDir1, destDirs[i], UIOption.AllDialogs, UICancelOption.ThrowException);
                if (!string.IsNullOrEmpty(sourceDir2))
                {
                    FileSystem.CopyDirectory(sourceDir2, destDirs[i], UIOption.AllDialogs, UICancelOption.ThrowException);
                }

                // copy force update file for Vesta 750 or Pitco 750
                if (softwarePartNumber.Contains("91452"))
                {
                    FileSystem.CopyDirectory(FORCE_UPDATE_FILE, destDirs[i], UIOption.AllDialogs, UICancelOption.ThrowException);
                }
            }
        }

        private void EnableUI()
        {
            // Enable UI controls
            groupBox1.Enabled = true;
            lvDrives.Enabled = true;
            btnStartCopy.Enabled = true;
            btnCheckVersion.Enabled = true;
        }

        private void DisableUI()
        {
            // Enable UI controls
            groupBox1.Enabled = false;
            lvDrives.Enabled = false;
            btnStartCopy.Enabled = false;
            btnCheckVersion.Enabled = false;
        }

        /// <summary>
        /// Deletes all files and directories in a path, with no warning messages.
        /// </summary>
        /// <param name="path"></param>
        private void DeleteDirContents(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo fi in dir.GetFiles())
                fi.Delete();

            foreach (DirectoryInfo di in dir.GetDirectories())
                di.Delete(true);
        }

        private void btnCheckVersion_click(object sender, EventArgs e)
        {
            string softwarePartNumber = "";
            if (!Directory.Exists(VAULT_PATH))
            {
                Logger.Log(@"Error: Cannot find vault (V:\ drive).  Check network connection and try again.", rt, Color.Red);
                return;
            }

            if (cboAssembly.Text?.Length == 0)
            {
                Logger.Log("No assembly selected; cannot check software version.", rt, Color.Red);
                return;
            }

            try
            {
                if (AssemblyDict.TryGetValue(cboAssembly.Text, out string value))
                {
                    softwarePartNumber = value;
                }

                if (softwarePartNumber.Contains(";"))
                {
                    softwarePartNumber = softwarePartNumber.Substring(0, softwarePartNumber.IndexOf(";"));
                }

                string softwareVersion = GetECL(softwarePartNumber);
                Logger.Log($"Current released version of software {softwarePartNumber}: ECL-{softwareVersion}", rt);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error checking software version: {ex.Message}", rt);
            }
        }

        /// <summary>
        /// Reads assembly/software part numbers from the config file into a dict.
        /// Ignores lines that start with "#"
        /// </summary>
        /// <returns>dict</returns>
        private Dictionary<string,string> ReadSoftwareSettingsToDict()
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>();

            //try
            //{
            //    NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("SoftwarePartNumbers");

            //    // if software settings section doesn't exist or has no software in it, return empty dict
            //    if ((section == null) || (section.Count == 0)) return dict;

            //    foreach (string key in section.AllKeys)
            //    {
            //        var value = section[key];
            //        dict.Add(key, value);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.Log($"Error loading software part numbers: {e.Message}", rt, Color.Red);
            //}

            //return dict;

            Dictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                var fs = new FileStream(INI_FILE, FileMode.Open, FileAccess.Read);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith(@"#"))   // ignore comment lines
                        {
                            string[] parts = line.Split(',');
                            dict.Add(parts[0], parts[2]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error loading software settings: {e.Message}", rt, Color.Red);
            }

            return dict;
        }

        private Dictionary<string, string> ReadSoftwareDescriptionsToDict()
        {
            //Dictionary<string, string> dict = new Dictionary<string, string>();

            //try
            //{
            //    NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("SoftwareDescriptions");

            //    // if software settings section doesn't exist or has no software in it, return empty dict
            //    if ((section == null) || (section.Count == 0)) return dict;

            //    foreach (string key in section.AllKeys)
            //    {
            //        var value = section[key];
            //        dict.Add(key, value);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.Log($"Error loading software descriptions: {e.Message}", rt, Color.Red);
            //}

            //return dict;

            Dictionary<string, string> dict = new Dictionary<string, string>();

            try
            {
                var fs = new FileStream(INI_FILE, FileMode.Open, FileAccess.Read);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.StartsWith(@"#"))   // ignore comment lines
                        {
                            string[] parts = line.Split(',');
                            dict.Add(parts[0], parts[1]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Error loading software descriptions: {e.Message}", rt, Color.Red);
            }

            return dict;
        }

        private void cboAssembly_SelectedValueChanged(object sender, EventArgs e)
        {
            if (AssemblyDict.TryGetValue(cboAssembly.Text, out string software))
            {
                if (software.Contains(";"))
                {
                    software = software.Substring(0, software.IndexOf(";"));
                }
                lblSoftware.Text = software;
            }

            if (DescriptionDict.TryGetValue(cboAssembly.Text, out string description))
            {
                lblDescription.Text = description;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(AssemblyDict["231-60295-60"]);
        }
    }
}