using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Win7FTP.Library;
using Microsoft.WindowsAPICodePack.Taskbar;
using Win7FTP.HelperClasses;
namespace Win7FTP
{
    public partial class frmUpload : GlassForm
    {


        string FileName;

        FTPclient FtpClient;


        #region Contructor

        public frmUpload(string UploadFilePath, string UploadDirectory, FTPclient Ftpclient)
        {
            InitializeComponent();


            FileName = System.IO.Path.GetFileName(UploadFilePath);                
            lblFileName.Text = Ftpclient.Hostname + UploadDirectory + FileName;   
            lblUploadDirectory.Text = UploadDirectory;                            
            FtpClient = Ftpclient;


            this.Show();

            FtpClient.CurrentDirectory = UploadDirectory;
            FtpClient.OnUploadCompleted += new FTPclient.UploadCompletedHandler(FtpClient_OnUploadCompleted);
            FtpClient.OnUploadProgressChanged += new FTPclient.UploadProgressChangedHandler(FtpClient_OnUploadProgressChanged);
            FtpClient.Upload(UploadFilePath, UploadDirectory + FileName);
        }
        #endregion

        #region Events
        #region Uploading File Events
        void FtpClient_OnUploadProgressChanged(object sender, UploadProgressChangedArgs e)
        {
            //Set Value and Maximum for Progressbar
            progressBar1.Maximum = Convert.ToInt32(e.TotleBytes);
            progressBar1.Value = Convert.ToInt32(e.BytesUploaded);
            //TaskManager Progress
            TaskBarManager.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
            // Calculate the Upload progress in percentages
            Int64 PercentProgress = Convert.ToInt64((e.BytesUploaded * 100) / e.TotleBytes);

            this.Text = PercentProgress.ToString() + " % Uploading " + FileName;
            lblDownloadStatus.Text = "Upload Status: Uploaded " + GetFileSize(e.BytesUploaded) + " out of " + GetFileSize(e.TotleBytes) + " (" + PercentProgress.ToString() + "%)";
            
        }

        void FtpClient_OnUploadCompleted(object sender, UploadCompletedArgs e)
        {
            if (e.UploadCompleted)
            {
                //No Error
                this.Text = "Upload Completed!";
                lblDownloadStatus.Text = "Uploaded File Successfully!";
                //change value
                progressBar1.Value = progressBar1.Maximum;
                btnCancel.Text = "Exit";
                MessageBox.Show("File Successfully Uploaded!", "Upload Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Change Texts of various controls.
                lblDownloadStatus.Text = "Upload Status: " + e.UploadStatus;
                this.Text = "Upload Error";
                btnCancel.Text = "Exit";
                //Display the Error State
                Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState state = TaskbarProgressBarState.Error;
                TaskBarManager.SetTaskBarProgressState(state);
                //Display a Messagebox with the error.
                MessageBox.Show("Error: " + e.UploadStatus, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion

        void frmUpload_AeroGlassCompositionChanged(object sender, AeroGlassCompositionChangedEvenArgs e)
        {
         
            if (e.GlassAvailable)
            {
                ExcludeControlFromAeroGlass(pnlNonTransparent);
                Invalidate();
            }
            else
            {
                this.BackColor = Color.Teal;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.btnCancel.Text != "Exit")
                FtpClient.CancelUpload(FileName);
            this.Close();
        }

        private void frmUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaskBarManager.ClearProgressValue();
        }
        #endregion

        #region Functions

        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        #endregion
    }
}
