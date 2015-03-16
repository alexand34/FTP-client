using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Dialogs;
using Win7FTP.Library;
using System.IO;
using System.Collections;
using System.Net;
using Raccoom.Windows.Forms;
using Win7FTP.HelperClasses;
using System.Diagnostics;
using System.Reflection;

namespace Win7FTP
{
    public partial class frmMain : Form
    {
        TreeNode DirNode;
        public FTPclient FtpClient;
        CommonOpenFileDialog objOpenDialog;
        frmRename RenameDialog;
        ListViewItem Message;
        string AppID = "FTP client";



        public frmMain()
        {

            InitializeComponent();


            this.tvFileSystem.DataSource =new Raccoom.Windows.Forms.TreeViewFolderBrowserDataProvider();
            this.tvFileSystem.Populate();
            this.tvFileSystem.Nodes[0].Expand();
            txtRemoteDirectory.Text = "/";
            lstRemoteSiteFiles.FullRowSelect = true;

            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance.ApplicationId = AppID;
            
        }


        #region Functions

        public void SetFtpClient(Win7FTP.Library.FTPclient client)
        {

            FtpClient = client;
            Message = new ListViewItem();
            Message.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            Message.SubItems.Add("Welcome Message");
            Message.SubItems.Add(FtpClient.WelcomeMessage);
            Message.SubItems.Add("No Code");
            Message.SubItems.Add("/");
            lstMessages.Items.Add(Message);

            FtpClient.OnNewMessageReceived += new FTPclient.NewMessageHandler(FtpClient_OnNewMessageReceived);

            foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail("/"))
            {
                ListViewItem item = new ListViewItem();
                item.Text = folder.Filename;
                if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                    item.SubItems.Add("Folder");
                else
                    item.SubItems.Add("File");

                item.SubItems.Add(folder.FullName);
                item.SubItems.Add(folder.Permission);
                item.SubItems.Add(folder.FileDateTime.ToShortTimeString() + folder.FileDateTime.ToShortDateString());
                item.SubItems.Add(GetFileSize(folder.Size));
                lstRemoteSiteFiles.Items.Add(item);
            }
        }
        private void RefreshDirectory()
        {

            lstRemoteSiteFiles.Items.Clear();


            foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
            {
                ListViewItem item = new ListViewItem();
                item.Text = folder.Filename;
                if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                    item.SubItems.Add("Folder");
                else
                    item.SubItems.Add("File");

                item.SubItems.Add(folder.FullName);
                item.SubItems.Add(folder.Permission);
                item.SubItems.Add(folder.FileDateTime.ToShortTimeString() + folder.FileDateTime.ToShortDateString());
                item.SubItems.Add(folder.Size.ToString());
                lstRemoteSiteFiles.Items.Add(item);
            }
        }

        #region Download File

        private void DownloadFile(string FileName, string CurrentDirectory)
        {

            CommonSaveFileDialog SaveDialog = new CommonSaveFileDialog("Save File To...");
            SaveDialog.AddToMostRecentlyUsedList = true;
            SaveDialog.DefaultFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            SaveDialog.DefaultFileName = FileName;
            if (SaveDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SaveDialog.FileName, FtpClient);
            }
            else
                TaskDialog.Show("Download has been cancelled.");        //Notify user that Download has been cancelled.
        }


        private void DownloadFile(string FileName, string CurrentDirectory, string SavePath)
        {

            if (SavePath.EndsWith("\\"))
            {
                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SavePath + FileName, FtpClient);
            }
            else
            {

                frmDownload DownloadForm = new frmDownload(FileName, CurrentDirectory, SavePath + "\\" + FileName, FtpClient);
            }
        }
        #endregion


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

        #region Events
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {

                DirNode = new TreeNode();
                tvFileSystem.ShowFolder(txtLocalFolderName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnOpenDialog_Click(object sender, EventArgs e)
        {

            objOpenDialog = new CommonOpenFileDialog("Select Folder");
            objOpenDialog.IsFolderPicker = true;
            objOpenDialog.Multiselect = false;
            objOpenDialog.AddToMostRecentlyUsedList = true;
            objOpenDialog.AllowNonFileSystemItems = false;
            objOpenDialog.EnsurePathExists = true;


            if (objOpenDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                txtLocalFolderName.Text = objOpenDialog.FileName;
                try
                {

                    DirNode = new TreeNode();
                    tvFileSystem.ShowFolder(txtLocalFolderName.Text);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message);
                }
            }


            objOpenDialog.Dispose();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {

            gbFileSystem.Size = new Size(this.Size.Width / 2, gbFileSystem.Height);
            gbRemoteSite.Size = gbFileSystem.Size;

            txtLocalFolderName.Size = new Size(toolStrip1.Size.Width - 115, txtLocalFolderName.Size.Height);
            txtRemoteDirectory.Size = txtLocalFolderName.Size;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            gbFileSystem.Size = new Size(this.Size.Width / 2, gbFileSystem.Height);
            gbRemoteSite.Size = gbFileSystem.Size;
            tvFileSystem.AllowDrop = true;
        }

        private void FtpClient_OnNewMessageReceived(object myObject, NewMessageEventArgs e)
        {

            Message = new ListViewItem();
            Message.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            Message.SubItems.Add(e.StatusType);
            Message.SubItems.Add(e.StatusMessage);
            Message.SubItems.Add(e.StatusCode);
            Message.SubItems.Add(txtRemoteDirectory.Text);
            lstMessages.Items.Add(Message);

            this.lstMessages.EnsureVisible(this.lstMessages.Items.Count - 1);
        }


        private void lstRemoteSiteFiles_SelectedValueChanged(object sender, EventArgs e)
        {

            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0] == null)
                {
                    lstRemoteSiteFiles.Items[0].Selected = true;
                }
            }
            catch
            {
                return;
            }


            if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "File")
            {

                btnRename.Enabled = true;
                btnDownload.Enabled = true;
            }
            else if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "Folder") 
            {
                btnRename.Enabled = false;
                btnDownload.Enabled = false;
            }
        }

        private void btnRemoteBack_Click(object sender, EventArgs e)
        {

            if (txtRemoteDirectory.Text != "/")
            {

                int endTagStartPosition = txtRemoteDirectory.Text.LastIndexOf("/");
                txtRemoteDirectory.Text = txtRemoteDirectory.Text.Substring(0, endTagStartPosition);
                int endTagStartPosition1 = txtRemoteDirectory.Text.LastIndexOf("/");
                txtRemoteDirectory.Text = txtRemoteDirectory.Text.Substring(0, endTagStartPosition1);


                if (txtRemoteDirectory.Text != "/")
                    txtRemoteDirectory.Text += "/";


                lstRemoteSiteFiles.Items.Clear();

                FtpClient.CurrentDirectory = txtRemoteDirectory.Text;


                foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = folder.Filename;
                    if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                        item.SubItems.Add("Folder");
                    else
                        item.SubItems.Add("File");

                    item.SubItems.Add(folder.FullName);
                    item.SubItems.Add(folder.Permission);
                    item.SubItems.Add(folder.FileDateTime.ToShortTimeString() + folder.FileDateTime.ToShortDateString());
                    item.SubItems.Add(folder.Size.ToString());
                    lstRemoteSiteFiles.Items.Add(item);
                }
            }
        }

        private void txtLocalFolderName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    DirNode = new TreeNode();
                    tvFileSystem.ShowFolder(txtLocalFolderName.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "File")
                {
                    string extension = lstRemoteSiteFiles.SelectedItems[0].Text.Substring(lstRemoteSiteFiles.SelectedItems[0].Text.LastIndexOf("."));

                    RenameDialog = new frmRename(lstRemoteSiteFiles.SelectedItems[0].Text, txtRemoteDirectory.Text, this, extension);
                    RenameDialog.ShowDialog(this);

                }
                else
                    RenameDialog = new frmRename(lstRemoteSiteFiles.SelectedItems[0].Text, txtRemoteDirectory.Text, this, "");

                RefreshDirectory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstRemoteSiteFiles.SelectedItems[0] != null)
            {

                if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "File")
                {
                    try
                    {

                        FtpClient.FtpDelete(lstRemoteSiteFiles.SelectedItems[0].Text);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error Occured while Deleting File.  Error Message: " + ex.Message);
                    }
                }
                else
                {
                    try
                    {

                        FtpClient.FtpDeleteDirectory(lstRemoteSiteFiles.SelectedItems[0].SubItems[2].Text);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error Occured while Deleting File.  Error Message: " + ex.Message);
                    }
                }

                RefreshDirectory();
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TaskBarManager.ClearProgressValue();

            try
            {
                if (lstRemoteSiteFiles.SelectedItems[0] != null)
                {
                    if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "File")
                    {

                        DownloadFile(lstRemoteSiteFiles.SelectedItems[0].Text, FtpClient.CurrentDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewDir_Click(object sender, EventArgs e)
        {
            try
            {

                frmNewFolder NewFolderForm = new frmNewFolder();
                if (NewFolderForm.ShowDialog() == DialogResult.OK)
                {

                    FtpClient.FtpCreateDirectory(FtpClient.CurrentDirectory + NewFolderForm.NewDirName);

                    RefreshDirectory();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error Occured while Creating Folder. Error Message: " + ex.Message);
            }
        }

        #region Drag & Drop to FileSystem from Remote Server
        private void lstRemoteSiteFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lstRemoteSiteFiles.DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void tvFileSystem_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                if (li.SubItems[1].Text == "File")
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void tvFileSystem_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                try
                {

                    if (li.SubItems[1].Text == "File")
                    {
                        Point pos = tvFileSystem.PointToClient(new Point(e.X, e.Y));
                        TreeNode targetNode = tvFileSystem.GetNodeAt(pos);
                        if (targetNode != null)
                        {

                            TreeNodePath SelectedNode = targetNode as TreeNodePath;

                            DialogResult DownloadConfirm = MessageBox.Show("Do you want to save " + li.Text + " to " + SelectedNode.Path + "?", "Download File?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (DownloadConfirm == DialogResult.Yes)
                            {

                                DownloadFile(li.Text, FtpClient.CurrentDirectory, SelectedNode.Path);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void tvFileSystem_DragOver(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem li = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (li.SubItems[1].Text == "File")
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                    e.Effect = DragDropEffects.None;

            }
        }
        #endregion

        private void lstRemoteSiteFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstRemoteSiteFiles.Items.Count != 0)
            {
                try
                {
                    if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "File")
                    {

                        btnRename.Enabled = true;
                        btnDownload.Enabled = true;

                        FtpClient.CurrentDirectory = txtRemoteDirectory.Text;
                        
                        if (MessageBox.Show("Do you want to save this file: " + txtRemoteDirectory.Text + lstRemoteSiteFiles.SelectedItems[0].Text + "/" + "?", "Download File?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            
                            downloadToolStripMenuItem_Click(this, e);
                        }
                    }
                    else if (lstRemoteSiteFiles.SelectedItems[0].SubItems[1].Text == "Folder")
                    {
                        
                        
                        txtRemoteDirectory.Text += lstRemoteSiteFiles.SelectedItems[0].Text + "/";
                        lstRemoteSiteFiles.Items.Clear();

                        
                        FtpClient.CurrentDirectory = txtRemoteDirectory.Text;

                        
                        foreach (FTPfileInfo folder in FtpClient.ListDirectoryDetail(txtRemoteDirectory.Text))
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = folder.Filename;
                            if (folder.FileType == FTPfileInfo.DirectoryEntryTypes.Directory)
                                item.SubItems.Add("Folder");
                            else
                                item.SubItems.Add("File");

                            item.SubItems.Add(folder.FullName);
                            item.SubItems.Add(folder.Permission);
                            item.SubItems.Add(folder.FileDateTime.ToShortTimeString() + folder.FileDateTime.ToShortDateString());
                            item.SubItems.Add(GetFileSize(folder.Size));
                            lstRemoteSiteFiles.Items.Add(item);
                        }
                    }
                }
                catch { }
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            Application.OpenForms[0].Show();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            
            TaskBarManager.ClearProgressValue();

            objOpenDialog = new CommonOpenFileDialog("Select File to Upload");
            objOpenDialog.Multiselect = false;
            CommonFileDialogFilter Filter = new CommonFileDialogFilter("All Files", "*.*");
            objOpenDialog.Filters.Add(Filter);
            objOpenDialog.RestoreDirectory = true;
            if (objOpenDialog.ShowDialog() == CommonFileDialogResult.OK)
            {
                
                frmUpload UploadForm = new frmUpload(objOpenDialog.FileName, FtpClient.CurrentDirectory, FtpClient);
            }
            
            RefreshDirectory();
        }
        #endregion        
    }
}

