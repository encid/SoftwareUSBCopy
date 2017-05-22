namespace SoftwareUSBCopy
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lvDrives = new System.Windows.Forms.ListView();
            this.btnStartCopy = new System.Windows.Forms.Button();
            this.rt = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbVesta = new System.Windows.Forms.RadioButton();
            this.rbPitco = new System.Windows.Forms.RadioButton();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvDrives
            // 
            this.lvDrives.FullRowSelect = true;
            this.lvDrives.Location = new System.Drawing.Point(43, 23);
            this.lvDrives.MultiSelect = false;
            this.lvDrives.Name = "lvDrives";
            this.lvDrives.ShowGroups = false;
            this.lvDrives.Size = new System.Drawing.Size(365, 131);
            this.lvDrives.TabIndex = 0;
            this.lvDrives.UseCompatibleStateImageBehavior = false;
            this.lvDrives.View = System.Windows.Forms.View.Details;
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.Location = new System.Drawing.Point(463, 277);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(92, 23);
            this.btnStartCopy.TabIndex = 1;
            this.btnStartCopy.Text = "Copy Software";
            this.btnStartCopy.UseVisualStyleBackColor = true;
            this.btnStartCopy.Click += new System.EventHandler(this.btnStartCopy_click);
            // 
            // rt
            // 
            this.rt.Location = new System.Drawing.Point(43, 277);
            this.rt.Name = "rt";
            this.rt.Size = new System.Drawing.Size(365, 96);
            this.rt.TabIndex = 2;
            this.rt.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVesta);
            this.groupBox1.Controls.Add(this.rbPitco);
            this.groupBox1.Location = new System.Drawing.Point(449, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 131);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Touchscreen Part Number";
            // 
            // rbVesta
            // 
            this.rbVesta.AutoSize = true;
            this.rbVesta.Location = new System.Drawing.Point(14, 51);
            this.rbVesta.Name = "rbVesta";
            this.rbVesta.Size = new System.Drawing.Size(127, 17);
            this.rbVesta.TabIndex = 1;
            this.rbVesta.TabStop = true;
            this.rbVesta.Text = "231-60295-70 (Vesta)";
            this.rbVesta.UseVisualStyleBackColor = true;
            // 
            // rbPitco
            // 
            this.rbPitco.AutoSize = true;
            this.rbPitco.Location = new System.Drawing.Point(14, 28);
            this.rbPitco.Name = "rbPitco";
            this.rbPitco.Size = new System.Drawing.Size(124, 17);
            this.rbPitco.TabIndex = 0;
            this.rbPitco.TabStop = true;
            this.rbPitco.Text = "231-60295-60 (Pitco)";
            this.rbPitco.UseVisualStyleBackColor = true;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 750;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(463, 313);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(79, 74);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 4;
            this.PictureBox1.TabStop = false;
            this.PictureBox1.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(135, 215);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "label1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 399);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rt);
            this.Controls.Add(this.btnStartCopy);
            this.Controls.Add(this.lvDrives);
            this.Name = "Main";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDrives;
        private System.Windows.Forms.Button btnStartCopy;
        private System.Windows.Forms.RichTextBox rt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVesta;
        private System.Windows.Forms.RadioButton rbPitco;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label lblStatus;
    }
}

