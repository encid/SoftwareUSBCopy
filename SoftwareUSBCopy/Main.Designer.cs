namespace SoftwareCopy
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
            this.lvDrives = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbPitco = new System.Windows.Forms.RadioButton();
            this.rbVesta = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDrives
            // 
            this.lvDrives.Location = new System.Drawing.Point(43, 23);
            this.lvDrives.Name = "lvDrives";
            this.lvDrives.Size = new System.Drawing.Size(365, 131);
            this.lvDrives.TabIndex = 0;
            this.lvDrives.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(463, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Copy Software";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(43, 277);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(365, 96);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 399);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvDrives);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDrives;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVesta;
        private System.Windows.Forms.RadioButton rbPitco;
    }
}

