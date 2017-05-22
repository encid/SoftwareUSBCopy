using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;

namespace SoftwareUSBCopy
{
    public partial class Main : Form
    {
        public const string VAULT_PATH = @"\\pandora\vault\Released_Part_Information\240-XXXXX-XX_SOFTWARE\240-9XXXX-XX\240-91XXX-XX";
        int currDriveCount;
        BackgroundWorker bw;

        private struct CopyParams
        {
            public readonly string _sourceDir;
            public readonly List<string> _destDirs;
            public CopyParams(string source, List<string> destinations)
            {
                _sourceDir = source;
                _destDirs = destinations;
            }
        }

        private struct ProgressParams
        {
            public int _currentDrive;
            public readonly int _totalDrives;
            public ProgressParams(int currentDrive, int totalDrives)
            {
                _currentDrive = currentDrive;
                _totalDrives = totalDrives;
            }
        }

        public Main()
        {
            InitializeComponent();

            // Add (destination) removable drives to ListView
            PopulateListView(lvDrives);

            currDriveCount = lvDrives.Items.Count;

            // Tick the checkbox if any part of the item line in ListView is clicked
            //lvDrives.MouseClick += (o, e) =>
            //{
            //    var lvi = lvDrives.GetItemAt(e.X, e.Y);
            //    if (e.X > 16) lvi.Checked = !lvi.Checked;
            //};

            // Add columns to the ListView
            lvDrives.Columns.Add("Drive", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Volume Name", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("File System", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Capacity", -2, HorizontalAlignment.Left);
            lvDrives.Columns.Add("Free Space", -2, HorizontalAlignment.Left);

            // Set up BackgroundWorker
            bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            //bw.DoWork += bw_DoWork;
            //bw.ProgressChanged += bw_ProgressChanged;
            //bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            // Make sure textbox stays at the most recent line(bottom most)
            rt.TextChanged += (sender, e) => {
                if (rt.Visible)
                    rt.ScrollToCaret();
            };
        }

        private List<string> GetDestinationDirs(ListView lview)
        {
            var d = new List<string>();

            foreach (ListViewItem item in lview.CheckedItems)
                d.Add(item.Text);

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
            var drives = DriveInfo.GetDrives();

            return drives.Where(p => p.DriveType == DriveType.Removable && p.IsReady);
        }

        /// <summary>
        /// Detects removable drives and adds them to ListView.
        /// </summary>
        /// <param name="lview">ListView to populate.</param>
        private void PopulateListView(ListView lview)
        {
            Logger.Log("Scanning for removable drives...", rt);

            lview.Items.Clear();

            var drives = GetRemovableDrives();

            // If no removable drives detected, exit method
            if (!drives.Any())
            {
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

                lview.Items.Add(oItem);
            }

            // Get count and names of drives found and log it to status
            driveNames = driveNames.Substring(0, driveNames.Length - 2);
            var logStr = string.Format("Detected {0} removable {1}: {2}", drives.Count(), "drive".Pluralize(drives.Count()), driveNames);
            Logger.Log(logStr, rt);
        }

        /// <summary>
        /// Validates the UI input parameters for copying, and throws exceptions based on parameters.
        /// </summary>
        /// <param name="srcDir">Source directory to copy from.</param>
        /// <param name="destDirs">Destination directories in list collection.</param>
        private void ValidateCopyParams(string srcDir, List<string> destDirs)
        {
            // Check if source drive is ready and exists
            var dInfo = new DriveInfo(srcDir.Substring(0, 2));
            if (!dInfo.IsReady)
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
            string softwareDir = "";
            // Set software directory based on what touchscreen version is selected
            if (rbPitco.Checked)
            {
                softwareDir = VAULT_PATH + @"\240-91452-XX\240-91452-03";
            }
            

            PictureBox1.Visible = false;

            var cp = new CopyParams(softwareDir, GetDestinationDirs(lvDrives));

            try
            {
                // Validate user input on UI
                ValidateCopyParams(cp._sourceDir, cp._destDirs);

                // No exceptions, so continue....
                // Disable UI controls and set status
                DisableUI();
                lblStatus.Text = "Copying...";
                var logStr = string.Format("Starting to copy '{0}' to {1} {2}...", cp._sourceDir, cp._destDirs.Count, "drive".Pluralize(cp._destDirs.Count));
                Logger.Log(logStr, rt);

                // Begin the copy in BackgroundWorker, pass CopyParams object into it
                bw.RunWorkerAsync(cp);
            }
            catch (Exception ex)
            {
                var logStr = string.Format("Error: {0}", ex.Message);
                Logger.Log(logStr, rt, Color.Red);
                //MessageBox.Show("Error:  " + ex.Message, "USB Batch Copy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Message.Contains("Target destination drive does not exist"))
                    PopulateListView(lvDrives);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var cp = (CopyParams)e.Argument;

            try
            {
                PerformCopy(cp._sourceDir, cp._destDirs);
            }
            catch (ArgumentException)
            {  // Catch user removal of drive during copy for RunWorkerCompleted to process
            }
            catch (OperationCanceledException)
            {  // Catch user cancelling copy for RunWorkerCompleted to process
                e.Cancel = true;
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var pp = (ProgressParams)e.UserState;

            var logStr = string.Format("Copying drive {0} of {1}..", pp._currentDrive, pp._totalDrives);
            Logger.Log(logStr, rt);

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user cancelled the operation.
                lblStatus.Text = "Ready";
                Logger.Log("Copy operation has been cancelled", rt, Color.Red);
                this.BringToFront();
                this.Focus();
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                lblStatus.Text = "Ready";
                PopulateListView(lvDrives);
                var logStr = string.Format("An error has occured: {0}", e.Error.Message);
                Logger.Log(logStr, rt, Color.Red);
                MessageBox.Show("An error has occured: " + e.Error.Message, "USB Batch Copy", MessageBoxButtons.OK);
                this.BringToFront();
                this.Focus();
            }
            else
            {
                // The operation completed normally.
                PictureBox1.Visible = true;
                lblStatus.Text = "Ready";
                var logStr = string.Format("Copy operation successful -- copied to {0} {1}", GetDestinationDirs(lvDrives).Count(),
                                                                            "drive".Pluralize(GetDestinationDirs(lvDrives).Count));
                Logger.Log(logStr, rt, Color.Green);
            }

            // Enable UI controls            
            EnableUI();

            SetListViewCheckState(lvDrives, false);
        }

        private void PerformCopy(string srcDir, List<string> destDirs)
        {
            var pp = new ProgressParams(0, destDirs.Count);

            // Start copy execution
            for (int i = 0; i < destDirs.Count; i++)
            {
                pp._currentDrive++;
                bw.ReportProgress(i, pp);
                string destDir = destDirs[i];
                FileSystem.CopyDirectory(srcDir, destDir, UIOption.AllDialogs, UICancelOption.ThrowException);
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            var drives = GetRemovableDrives();

            if (drives.Count() != currDriveCount)
                PopulateListView(lvDrives);

            currDriveCount = drives.Count();            
        }

        private void EnableUI()
        {
            // Enable UI controls                       
            btnStartCopy.Enabled = true;
            lvDrives.Enabled = true;
            tmrRefresh.Enabled = true;
        }

        private void DisableUI()
        {
            // Enable UI controls                        
            btnStartCopy.Enabled = false;
            lvDrives.Enabled = false;
            tmrRefresh.Enabled = false;
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
    }
}
