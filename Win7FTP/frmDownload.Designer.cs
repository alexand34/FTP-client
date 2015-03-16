namespace Win7FTP
{
    partial class frmDownload
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
            this.lblSavePath = new System.Windows.Forms.Label();
            this.lblDownloadFrom = new System.Windows.Forms.Label();
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
            this.pnlNonTransparent.Controls.Add(this.lblSavePath);
            this.pnlNonTransparent.Controls.Add(this.lblDownloadFrom);
            this.pnlNonTransparent.Controls.Add(this.label2);
            this.pnlNonTransparent.Controls.Add(this.label1);
            this.pnlNonTransparent.Location = new System.Drawing.Point(-1, 0);
            this.pnlNonTransparent.Name = "pnlNonTransparent";
            this.pnlNonTransparent.Size = new System.Drawing.Size(410, 181);
            this.pnlNonTransparent.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 93);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(387, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(322, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // lblDownloadStatus
            // 
            this.lblDownloadStatus.AutoSize = true;
            this.lblDownloadStatus.Location = new System.Drawing.Point(7, 77);
            this.lblDownloadStatus.Name = "lblDownloadStatus";
            this.lblDownloadStatus.Size = new System.Drawing.Size(91, 13);
            this.lblDownloadStatus.TabIndex = 4;
            this.lblDownloadStatus.Text = "Download Status:";
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Location = new System.Drawing.Point(7, 64);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(35, 13);
            this.lblSavePath.TabIndex = 3;
            this.lblSavePath.Text = "label4";
            // 
            // lblDownloadFrom
            // 
            this.lblDownloadFrom.AutoSize = true;
            this.lblDownloadFrom.Location = new System.Drawing.Point(7, 28);
            this.lblDownloadFrom.Name = "lblDownloadFrom";
            this.lblDownloadFrom.Size = new System.Drawing.Size(35, 13);
            this.lblDownloadFrom.TabIndex = 2;
            this.lblDownloadFrom.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Save Path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File URL:";
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 151);
            this.Controls.Add(this.pnlNonTransparent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDownload";
            this.Text = "Downloading";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.pnlNonTransparent.ResumeLayout(false);
            this.pnlNonTransparent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlNonTransparent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblDownloadStatus;
        private System.Windows.Forms.Label lblSavePath;
        private System.Windows.Forms.Label lblDownloadFrom;
        private System.Windows.Forms.Button btnCancel;
    }
}