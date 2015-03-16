namespace Win7FTP
{
    partial class frmRename
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
            this.pnlNonTransparent = new System.Windows.Forms.Panel();
            this.cmdRename = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink();
            this.button1 = new System.Windows.Forms.Button();
            this.txtRenameTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.pnlNonTransparent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNonTransparent
            // 
            this.pnlNonTransparent.Controls.Add(this.cmdRename);
            this.pnlNonTransparent.Controls.Add(this.button1);
            this.pnlNonTransparent.Controls.Add(this.txtRenameTo);
            this.pnlNonTransparent.Controls.Add(this.label2);
            this.pnlNonTransparent.Controls.Add(this.lblFileName);
            this.pnlNonTransparent.Controls.Add(this.lblLocation);
            this.pnlNonTransparent.Location = new System.Drawing.Point(9, 10);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(357, 138);
            this.pnlNonTransparent.TabIndex = 0;
            // 
            // cmdRename
            // 
            this.cmdRename.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdRename.Location = new System.Drawing.Point(217, 90);
            this.cmdRename.Name = "cmdRename";
            this.cmdRename.NoteText = "";
            this.cmdRename.Size = new System.Drawing.Size(114, 45);
            this.cmdRename.TabIndex = 8;
            this.cmdRename.Text = "Rename";
            this.cmdRename.UseVisualStyleBackColor = true;
            this.cmdRename.Click += new System.EventHandler(this.cmdRename_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtRenameTo
            // 
            this.txtRenameTo.Location = new System.Drawing.Point(14, 64);
            this.txtRenameTo.Name = "txtRenameTo";
            this.txtRenameTo.Size = new System.Drawing.Size(317, 20);
            this.txtRenameTo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rename To";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(11, 22);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(54, 13);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "FileName:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(11, 9);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location:";
            // 
            // frmRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 160);
            this.Controls.Add(this.pnlNonTransparent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRename";
            this.Text = "Rename";
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Button button1;
        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink cmdRename;
        public System.Windows.Forms.TextBox txtRenameTo;
    }
}