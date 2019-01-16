namespace USBSoftwareLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lvDrives = new System.Windows.Forms.ListView();
            this.btnStartCopy = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblVestaSWPN = new System.Windows.Forms.Label();
            this.lblPitcoSWPN = new System.Windows.Forms.Label();
            this.rbVesta = new System.Windows.Forms.RadioButton();
            this.rbPitco = new System.Windows.Forms.RadioButton();
            this.rt = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCheckVersion = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvDrives
            // 
            this.lvDrives.BackColor = System.Drawing.Color.White;
            this.lvDrives.CheckBoxes = true;
            this.lvDrives.FullRowSelect = true;
            this.lvDrives.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDrives.Location = new System.Drawing.Point(3, 5);
            this.lvDrives.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lvDrives.MultiSelect = false;
            this.lvDrives.Name = "lvDrives";
            this.lvDrives.ShowGroups = false;
            this.lvDrives.Size = new System.Drawing.Size(425, 159);
            this.lvDrives.TabIndex = 0;
            this.lvDrives.UseCompatibleStateImageBehavior = false;
            this.lvDrives.View = System.Windows.Forms.View.Details;
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCopy.Location = new System.Drawing.Point(507, 168);
            this.btnStartCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(125, 40);
            this.btnStartCopy.TabIndex = 1;
            this.btnStartCopy.Text = "Load Software";
            this.btnStartCopy.UseVisualStyleBackColor = true;
            this.btnStartCopy.Click += new System.EventHandler(this.btnStartCopy_click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblVestaSWPN);
            this.groupBox1.Controls.Add(this.lblPitcoSWPN);
            this.groupBox1.Controls.Add(this.rbVesta);
            this.groupBox1.Controls.Add(this.rbPitco);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(434, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(198, 120);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Touchscreen Part Number";
            // 
            // lblVestaSWPN
            // 
            this.lblVestaSWPN.AutoSize = true;
            this.lblVestaSWPN.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVestaSWPN.Location = new System.Drawing.Point(32, 90);
            this.lblVestaSWPN.Name = "lblVestaSWPN";
            this.lblVestaSWPN.Size = new System.Drawing.Size(115, 17);
            this.lblVestaSWPN.TabIndex = 3;
            this.lblVestaSWPN.Text = "s/w: 240-91452-02";
            // 
            // lblPitcoSWPN
            // 
            this.lblPitcoSWPN.AutoSize = true;
            this.lblPitcoSWPN.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPitcoSWPN.Location = new System.Drawing.Point(32, 44);
            this.lblPitcoSWPN.Name = "lblPitcoSWPN";
            this.lblPitcoSWPN.Size = new System.Drawing.Size(115, 17);
            this.lblPitcoSWPN.TabIndex = 2;
            this.lblPitcoSWPN.Text = "s/w: 240-91452-03";
            // 
            // rbVesta
            // 
            this.rbVesta.AutoSize = true;
            this.rbVesta.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbVesta.Location = new System.Drawing.Point(12, 72);
            this.rbVesta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbVesta.Name = "rbVesta";
            this.rbVesta.Size = new System.Drawing.Size(150, 21);
            this.rbVesta.TabIndex = 1;
            this.rbVesta.TabStop = true;
            this.rbVesta.Text = "231-60295-70 (Vesta)";
            this.rbVesta.UseVisualStyleBackColor = true;
            // 
            // rbPitco
            // 
            this.rbPitco.AutoSize = true;
            this.rbPitco.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPitco.Location = new System.Drawing.Point(12, 26);
            this.rbPitco.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rbPitco.Name = "rbPitco";
            this.rbPitco.Size = new System.Drawing.Size(146, 21);
            this.rbPitco.TabIndex = 0;
            this.rbPitco.TabStop = true;
            this.rbPitco.Text = "231-60295-60 (Pitco)";
            this.rbPitco.UseVisualStyleBackColor = true;
            // 
            // rt
            // 
            this.rt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rt.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rt.Location = new System.Drawing.Point(3, 182);
            this.rt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rt.Name = "rt";
            this.rt.ReadOnly = true;
            this.rt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rt.Size = new System.Drawing.Size(498, 162);
            this.rt.TabIndex = 2;
            this.rt.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(507, 306);
            this.label1.MaximumSize = new System.Drawing.Size(126, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "Version: 1.6.1                 1/16/19  13:35";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureBox1.Image = global::USBSoftwareLoader.Properties.Resources.questionmark;
            this.PictureBox1.Location = new System.Drawing.Point(526, 212);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(90, 90);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 4;
            this.PictureBox1.TabStop = false;
            // 
            // btnCheckVersion
            // 
            this.btnCheckVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckVersion.Location = new System.Drawing.Point(507, 129);
            this.btnCheckVersion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckVersion.Name = "btnCheckVersion";
            this.btnCheckVersion.Size = new System.Drawing.Size(125, 35);
            this.btnCheckVersion.TabIndex = 6;
            this.btnCheckVersion.Text = "Check Vault for Latest S/W Version";
            this.btnCheckVersion.UseVisualStyleBackColor = true;
            this.btnCheckVersion.Click += new System.EventHandler(this.btnCheckVersion_click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 347);
            this.Controls.Add(this.btnCheckVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rt);
            this.Controls.Add(this.lvDrives);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStartCopy);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kitchen Brains USB Software Loader";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvDrives;
        private System.Windows.Forms.Button btnStartCopy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVesta;
        private System.Windows.Forms.RadioButton rbPitco;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label lblVestaSWPN;
        private System.Windows.Forms.Label lblPitcoSWPN;
        private System.Windows.Forms.RichTextBox rt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckVersion;
    }
}

