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
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.cboAssembly = new System.Windows.Forms.ComboBox();
            this.lblSw = new System.Windows.Forms.Label();
            this.lblSoftware = new System.Windows.Forms.Label();
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
            this.lvDrives.Size = new System.Drawing.Size(425, 178);
            this.lvDrives.TabIndex = 1;
            this.lvDrives.UseCompatibleStateImageBehavior = false;
            this.lvDrives.View = System.Windows.Forms.View.Details;
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCopy.ForeColor = System.Drawing.Color.Navy;
            this.btnStartCopy.Location = new System.Drawing.Point(507, 179);
            this.btnStartCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(125, 40);
            this.btnStartCopy.TabIndex = 4;
            this.btnStartCopy.Text = "Load Software";
            this.btnStartCopy.UseVisualStyleBackColor = true;
            this.btnStartCopy.Click += new System.EventHandler(this.btnStartCopy_click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.lblDesc);
            this.groupBox1.Controls.Add(this.cboAssembly);
            this.groupBox1.Controls.Add(this.lblSw);
            this.groupBox1.Controls.Add(this.lblSoftware);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(434, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(198, 118);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assembly Part Number";
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(52, 82);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(137, 17);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "                         ";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(12, 82);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(39, 17);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Desc:";
            // 
            // cboAssembly
            // 
            this.cboAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssembly.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAssembly.FormattingEnabled = true;
            this.cboAssembly.Location = new System.Drawing.Point(15, 28);
            this.cboAssembly.Name = "cboAssembly";
            this.cboAssembly.Size = new System.Drawing.Size(164, 25);
            this.cboAssembly.TabIndex = 2;
            this.cboAssembly.SelectedIndexChanged += new System.EventHandler(this.cboAssembly_SelectedValueChanged);
            // 
            // lblSw
            // 
            this.lblSw.AutoSize = true;
            this.lblSw.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSw.Location = new System.Drawing.Point(12, 65);
            this.lblSw.Name = "lblSw";
            this.lblSw.Size = new System.Drawing.Size(62, 17);
            this.lblSw.TabIndex = 2;
            this.lblSw.Text = "Software:";
            // 
            // lblSoftware
            // 
            this.lblSoftware.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoftware.Location = new System.Drawing.Point(75, 65);
            this.lblSoftware.Name = "lblSoftware";
            this.lblSoftware.Size = new System.Drawing.Size(114, 17);
            this.lblSoftware.TabIndex = 3;
            this.lblSoftware.Text = "                         ";
            // 
            // rt
            // 
            this.rt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rt.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rt.Location = new System.Drawing.Point(3, 191);
            this.rt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rt.Name = "rt";
            this.rt.ReadOnly = true;
            this.rt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rt.Size = new System.Drawing.Size(498, 172);
            this.rt.TabIndex = 41;
            this.rt.TabStop = false;
            this.rt.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(507, 325);
            this.label1.MaximumSize = new System.Drawing.Size(126, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "Version: 1.8.1                 5/7/19  09:25";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureBox1.Image = global::USBSoftwareLoader.Properties.Resources.questionmark;
            this.PictureBox1.Location = new System.Drawing.Point(526, 231);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(90, 90);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 4;
            this.PictureBox1.TabStop = false;
            // 
            // btnCheckVersion
            // 
            this.btnCheckVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckVersion.Location = new System.Drawing.Point(507, 131);
            this.btnCheckVersion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckVersion.Name = "btnCheckVersion";
            this.btnCheckVersion.Size = new System.Drawing.Size(125, 40);
            this.btnCheckVersion.TabIndex = 3;
            this.btnCheckVersion.Text = "Check ECL";
            this.btnCheckVersion.UseVisualStyleBackColor = true;
            this.btnCheckVersion.Click += new System.EventHandler(this.btnCheckVersion_click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 366);
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
            this.Load += new System.EventHandler(this.Main_Load);
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
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label lblSoftware;
        private System.Windows.Forms.Label lblSw;
        private System.Windows.Forms.RichTextBox rt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckVersion;
        private System.Windows.Forms.ComboBox cboAssembly;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblDesc;
    }
}

