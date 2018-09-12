/* 
 * 
 * USB Software Loader
 * Designed by R. Cavallaro
 * Last update: 9/12/18
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

namespace USBSoftwareLoader
{
    public partial class Main : Form
    {
        public string VAULT_PATH = ConfigurationManager.AppSettings["VAULT_PATH"];
        public string CALIBRATION_FILE = Application.StartupPath + ConfigurationManager.AppSettings["CALIBRATION_FILE"];
        public string FORCE_UPDATE_FILE = Application.StartupPath + ConfigurationManager.AppSettings["FORCE_UPDATE_FILE"];
        int currDriveCount;
        BackgroundWorker bw;
        Timer tmrRefresh = new Timer();

        private struct CopyParams
        {
            public readonly string source1;
            public readonly string source2;
            public readonly List<string> dest;
            public readonly string swpn;
            public readonly string ecl;
            public CopyParams(string _source1, string _source2, List<string> _destinations, string _swpn, string _ecl)
            {
                ecl = _ecl;
                swpn = _swpn;
                source1 = _source1;
                source2 = _source2;
                dest = _destinations;
            }
        }

        private struct ProgressParams
        {
            public int currentDrive;
            public readonly int totalDrives;
            public ProgressParams(int _currentDrive, int _totalDrives)
            {
                currentDrive = _currentDrive;
                totalDrives = _totalDrives;
            }
        }

        public Main()
        {
            InitializeComponent();
            InitializeEventHandlers();

            Logger.Log("Application loading.. ready.", rt);

            // Add (destination) removable drives to ListView
            PopulateListView(lvDrives, true);

            currDriveCount = lvDrives.Items.Count;
                        
            rbPitco.Checked = true;
        }

        public void InitializeEventHandlers()
        {
            // Initialize Timer
            tmrRefresh.Enabled = true;
            tmrRefresh.Interval = 750;
            tmrRefresh.Tick += (s, e) =>
            {
                var drives = GetRemovableDrives();

                if (drives.Count() != currDriveCount)
                    PopulateListView(lvDrives, true);

                currDriveCount = drives.Count();
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

                var softwareDir = (from sd in Directory.GetDirectories($@"{VAULT_PATH}\{swDir}\240-{softwarePartOne}-XX\")
                                   where sd.Contains(softwarePartNumber)
                                   select sd)
                                  .FirstOrDefault();

                var eclDir = (from ed in Directory.GetDirectories(softwareDir)
                              where ed.Contains("ECL")
                              select ed)
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
        /// <param name="updateLog">Send update to status log.</param>
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

            string driveNames = "";
            // Iterate through collection and add each removable drive as to ListView as items and subitems
            foreach (var drive in drives)
            {
                driveNames += drive.Name + ", ";
                var freeSpace = BytesToString(drive.TotalFreeSpace);
                var totalSpace = BytesToString(drive.TotalSize);
                var oItem = new ListViewItem();

                oItem.Text = drive.Name;
                oItem.SubItems.Add(drive.VolumeLabel);
                oItem.SubItems.Add(drive.DriveFormat);
                oItem.SubItems.Add(totalSpace);
                oItem.SubItems.Add(freeSpace);

                lView.Items.Add(oItem);
            }

            // Get count and names of drives found and log it to status
            driveNames = driveNames.Substring(0, driveNames.Length - 2);
            if (updateLog)
            {
                var logStr = $"Detected {drives.Count()} removable {"drive".Pluralize(drives.Count())}: {driveNames}";                
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
        
        private void btnStartCopy_click(object sender, EventArgs e)
        {
            string softwareTopDir1 = "";
            string softwareTopDir2 = "";
            var ECL1 = "";
            var ECL2 = "";
            var softwarePartNumber = "";
            PictureBox1.Image = USBSoftwareLoader.Properties.Resources.questionmark;

            if (!Directory.Exists(VAULT_PATH))
            {
                Logger.Log(@"Error: Cannot find vault (V:\ drive).  Check network connection and try again.", rt, Color.Red);
                return;
            }

            try
            {
                // Get software dir based on what touchscreen version is selected, get ECL, and get full software dir
                if (rbPitco.Checked)
                {
                    softwareTopDir1 = VAULT_PATH + @"\240-91XXX-XX\240-91452-XX\240-91452-03";
                    softwareTopDir2 = VAULT_PATH + @"\240-94XXX-XX\240-94452-XX\240-94452-99";
                    ECL1 = GetECL("240-91452-03");
                    ECL2 = GetECL("240-94452-99");
                    softwarePartNumber = "Pitco 240-91452-03";
                }
                else if (rbVesta.Checked)
                {
                    softwareTopDir1 = VAULT_PATH + @"\240-91452-XX\240-91452-02";
                    ECL1 = GetECL("240-91452-02");
                    softwarePartNumber = "Vesta 240-91452-02";
                }

                var softwareDir1 = $@"{softwareTopDir1}\ECL-{ECL1}";
                var softwareDir2 = $@"{softwareTopDir2}\ECL-{ECL2}";
                var cp = new CopyParams(softwareDir1, softwareDir2, GetDestinationDirs(lvDrives), softwarePartNumber, ECL1);

                // Validate user input on UI
                ValidateCopyParams(cp.source1, cp.dest);

                // No exceptions, so continue....
                Logger.Log("\nFinding latest software version...", rt);
                Logger.Log("Latest software found: " + softwarePartNumber.Substring(6) + " ECL-" + ECL1, rt);

                // Show warning that drive will be erased
                if (MessageBox.Show(new Form { TopMost = true }, "The selected USB drive will be erased!\nDo you wish to continue?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    Logger.Log("Software loading operation cancelled.", rt, Color.Red);
                    return;
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
                PerformCopy(cp.source1, cp.source2, cp.dest, cp.swpn, cp.ecl);                
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
                MessageBox.Show("An error has occured: " + e.Error.Message, "USB Software Loader", MessageBoxButtons.OK);
                this.BringToFront();
                this.Focus();
            }
            else
            {
                // The operation completed normally.
                PictureBox1.Image = Properties.Resources.check;
                var cp = (CopyParams)e.Result;
                var logStr = $"Software {cp.swpn.Substring(6)} ECL-{cp.ecl} was successfully loaded!";
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
        private void PerformCopy(string sourceDir1, string sourceDir2, List<string> destDirs, string softwarePartNumber, string ECL)
        {
            // Start copy execution
            for (int i = 0; i < destDirs.Count; i++)
            {
                var dInfo = new DriveInfo(destDirs[i].Substring(0, 1));
                dInfo.VolumeLabel = $"{softwarePartNumber.Substring(10, 8)} {ECL}";
                
                // delete all files and directories of root dir on removable drive
                ExecuteSecure(() => Logger.Log("\nErasing removable drive " + destDirs[i] + "...", rt));
                DeleteDirContents(destDirs[i]);

                // Begin copying
                ExecuteSecure(() => Logger.Log($"Starting to copy {softwarePartNumber} ECL-{ECL} to {destDirs[i]}...", rt));
                FileSystem.CopyDirectory(sourceDir1, destDirs[i], UIOption.AllDialogs, UICancelOption.ThrowException);
                FileSystem.CopyDirectory(sourceDir2, destDirs[i], UIOption.AllDialogs, UICancelOption.ThrowException);
                FileSystem.CopyFile(FORCE_UPDATE_FILE, destDirs[i] + @"\force_update.txt", UIOption.AllDialogs, UICancelOption.ThrowException);
            }
        }

        private void EnableUI()
        {
            // Enable UI controls
            groupBox1.Enabled = true;
            lvDrives.Enabled = true;
            btnStartCopy.Enabled = true;
        }

        private void DisableUI()
        {
            // Enable UI controls
            groupBox1.Enabled = false;
            lvDrives.Enabled = false;
            btnStartCopy.Enabled = false;
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
            {
                DeleteDirContents(di.FullName);
                di.Delete(true);
            }
        }

        /// <summary>
        /// Check if specified directory is empty.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns></returns>
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string softwareVersion;
            if (!Directory.Exists(VAULT_PATH))
            {
                Logger.Log(@"Error: Cannot find vault (V:\ drive).  Check network connection and try again.", rt, Color.Red);
                return;
            }

            try
            {
                // Get software ECL based on what radio button is checked
                if (rbPitco.Checked)
                {
                    softwareVersion = GetECL("240-91452-03");
                    var softwareVersion2 = GetECL("240-94452-99");
                    Logger.Log($"Current version of Pitco software 240-91452-03: ECL-{softwareVersion}", rt);
                    Logger.Log($"Current version of Pitco software 240-94452-99: ECL-{softwareVersion2}", rt);
                }
                else if (rbVesta.Checked)
                {
                    softwareVersion = GetECL("240-91452-02");
                }
            }
            catch
            {

            }
        }
    }
}