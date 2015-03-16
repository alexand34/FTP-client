namespace Win7FTP
{
    partial class frmUpload
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDownloadStatus = new System.Windows.Forms.Label();
            this.lblUploadDirectory = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlNonTransparent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNonTransparent
            // 
            this.pnlNonTransparent.Controls.Add(this.progressBar1);
            this.pnlNonTransparent.Controls.Add(this.btnCancel);
            this.pnlNonTransparent.Controls.Add(this.lblDownloadStatus);
            this.pnlNonTransparent.Controls.Add(this.lblUploadDirectory);
            this.pnlNonTransparent.Controls.Add(this.lblFileName);
            this.pnlNonTransparent.Controls.Add(this.label2);
            this.pnlNonTransparent.Controls.Add(this.label1);
            this.pnlNonTransparent.Location = new System.Drawing.Point(12, 12);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(349, 137);
            this.pnlNonTransparent.TabIndex = 2;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(19, 78);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(310, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(254, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDownloadStatus
            // 
            this.lblDownloadStatus.AutoSize = true;
            this.lblDownloadStatus.Location = new System.Drawing.Point(16, 62);
            this.lblDownloadStatus.Name = "lblDownloadStatus";
            this.lblDownloadStatus.Size = new System.Drawing.Size(77, 13);
            this.lblDownloadStatus.TabIndex = 4;
            this.lblDownloadStatus.Text = "Upload Status:";
            // 
            // lblUploadDirectory
            // 
            this.lblUploadDirectory.AutoSize = true;
            this.lblUploadDirectory.Location = new System.Drawing.Point(16, 49);
            this.lblUploadDirectory.Name = "lblUploadDirectory";
            this.lblUploadDirectory.Size = new System.Drawing.Size(35, 13);
            this.lblUploadDirectory.TabIndex = 3;
            this.lblUploadDirectory.Text = "label4";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(16, 23);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(35, 13);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Upload Directory:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Name:";
            // 
            // frmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 161);
            this.Controls.Add(this.pnlNonTransparent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmUpload";
            this.Text = "Upload File";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpload_FormClosing);
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDownloadStatus;
        private System.Windows.Forms.Label lblUploadDirectory;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}