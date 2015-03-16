using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using System.Net;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Win7FTP.Library;
using Win7FTP.HelperClasses;

namespace Win7FTP
{
    public partial class frmDownload : GlassForm
    {

        string FileName, SaveFilePath, CurrentDirectory;
        TaskDialog taskDialogMain = null;
        FTPclient FtpClient;


        #region Constructor

        public frmDownload(string Filename, string Current_Directory, string SavePath, FTPclient Ftpclient)
        {
            InitializeComponent();

            FileName = Filename;
            SaveFilePath = SavePath;
            CurrentDirectory = Current_Directory;
            lblDownloadFrom.Text = Ftpclient.Hostname + Current_Directory + FileName;   //ex: ftp://ftp.somesite.com/current_dir/File.exe
            lblSavePath.Text = SaveFilePath;
            FtpClient = Ftpclient;
            TaskBarManager.ClearProgressValue();


           
            
            this.Show();

            FtpClient.CurrentDirectory = Current_Directory;
            FtpClient.OnDownloadProgressChanged += new FTPclient.DownloadProgressChangedHandler(FtpClient_OnDownloadProgressChanged);
            FtpClient.OnDownloadCompleted += new FTPclient.DownloadCompletedHandler(FtpClient_OnDownloadCompleted);
            FtpClient.Download(FileName, SavePath, true);
        }
        #endregion

        #region TaskDialog
        void ShowCompleteDownloadDialog()
        {
            taskDialogMain = new TaskDialog();
            taskDialogMain.Caption = "File Download Completed Task";
            taskDialogMain.InstructionText = "Select an Action to Perform with the Downloaded File:";
            taskDialogMain.FooterText = FileName + " has successfully downloaded. Please select an action to perform or click Cancel and Return.";
            taskDialogMain.Cancelable = true;

            taskDialogMain.StandardButtons = TaskDialogStandardButtons.Close;
            taskDialogMain.Closing += new EventHandler<TaskDialogClosingEventArgs>(taskDialogMain_Closing);

            #region Creating and adding command link buttons

            TaskDialogCommandLink btnOpenFile = new TaskDialogCommandLink("cmdOpenFile", "Open Downloaded File");
            btnOpenFile.Click += new EventHandler(btnOpenFile_Click);

            TaskDialogCommandLink btnOpenFolder = new TaskDialogCommandLink("cmdOpenFolder", "Open Folder containing Downloaded File");
            btnOpenFolder.Click += new EventHandler(btnOpenFolder_Click);

            taskDialogMain.Controls.Add(btnOpenFile);
            taskDialogMain.Controls.Add(btnOpenFolder);

            #endregion

            TaskDialogResult result = taskDialogMain.Show();
        }
        #endregion

        #region Events
        #region Download Client Events
        bool Happened = false;      //Supposedly, The OnDownloadCompleted repeats.  Well each time, it repeats one more time than
                                    //it previously repeated. Nothing bad about this, except that tyhe ShowCompleteDownloadDialog 
                                    //gets called as well. We don't want that.  So I have this variable to keep track of everything.

        //Event fires when the Download has completed.
        void FtpClient_OnDownloadCompleted(object sender, DownloadCompletedArgs e)
        {
            if (e.DownloadCompleted)
            {
                if (!Happened)
                { 
                    //Display the appropriate information to the User regarding the Download.
                    this.Text = "Download Completed!";
                    lblDownloadStatus.Text = "Downloaded File Successfully!";
                    progressBar1.Value = progressBar1.Maximum;
                    btnCancel.Text = "Exit";
                    //Display the TaskDialog, which will ask the user about what he/she needs to do with the file.
                    ShowCompleteDownloadDialog();
                }
                Happened = true;
            }
            else
            {
                lblDownloadStatus.Text = "Download Status: " + e.DownloadStatus;
                this.Text = "Download Error";
                btnCancel.Text = "Exit";

                HelperClasses.TaskBarManager.SetTaskBarProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Error);
                TaskDialog.Show("Error: " + e.DownloadStatus);
            }
            Happened = true;
        }

        //Event Fires whenever the Download Progress in changed.
        void FtpClient_OnDownloadProgressChanged(object sender, DownloadProgressChangedArgs e)
        {
            //Set Value for Progressbar
            progressBar1.Maximum = Convert.ToInt32(e.TotleBytes);
            progressBar1.Value = Convert.ToInt32(e.BytesDownloaded);

            //Taskbar Progress
            HelperClasses.TaskBarManager.SetProgressValue(Convert.ToInt32(e.BytesDownloaded), Convert.ToInt32(e.TotleBytes));

            // Calculate the download progress in percentages
            Int64 PercentProgress = Convert.ToInt64((progressBar1.Value * 100) / e.TotleBytes);

            //Display Information to the User on Form and on Labels
            this.Text = PercentProgress.ToString() + "% Downloading " + FileName;
            lblDownloadStatus.Text = "Download Status: Downloaded " + GetFileSize(e.BytesDownloaded) + " out of " + GetFileSize(e.TotleBytes) + " (" + PercentProgress.ToString() + "%)";
        }
        #endregion
        
        #region TaskDialog CommandLink Events
        void btnOpenFolder_Click(object sender, EventArgs e)
        {
            taskDialogMain.Close(TaskDialogResult.Close);
            Process.Start(System.IO.Path.GetPathRoot(SaveFilePath));
        }

        void btnOpenFile_Click(object sender, EventArgs e)
        {
            taskDialogMain.Close(TaskDialogResult.Close);
            Process.Start(SaveFilePath);
        }
        #endregion

        void taskDialogMain_Closing(object sender, TaskDialogClosingEventArgs e)
        {
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text != "Exit")
                FtpClient.CancelDownload(); 
                                            
            this.Close();
        }

        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {

            TaskBarManager.ClearProgressValue();

            Happened = false;
            this.taskDialogMain = null;
        }
        
        private void Form1_AeroGlassCompositionChanged(object sender, AeroGlassCompositionChangedEvenArgs e)
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
