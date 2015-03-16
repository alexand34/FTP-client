using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;

namespace Win7FTP
{
    public partial class frmRename : GlassForm
    {

        string Path, Extension, FileName;
        frmMain ForMain;


        #region Contructor

        public frmRename(string fileName, string CurrentDir, frmMain MainForm, string extension)
        {

            Path = CurrentDir;
            Extension = extension;
            FileName = fileName;
            InitializeComponent();
            ForMain = MainForm;

            AeroGlassCompositionChanged += new AeroGlassCompositionChangedEvent(Form1_AeroGlassCompositionChanged);
            if (AeroGlassCompositionEnabled)
            {

                ExcludeControlFromAeroGlass(pnlNonTransparent);
            }
            else
            {
                this.BackColor = Color.Teal;
            }


            this.Text = "Rename " + FileName;
            lblFileName.Text = "Filename: " + FileName;
            lblLocation.Text = "Location: " + Path;
        }
        #endregion

        #region Evemts
        void Form1_AeroGlassCompositionChanged(object sender, AeroGlassCompositionChangedEvenArgs e)
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

        private void Form1_Resize(object sender, EventArgs e)
        {

            Rectangle panelRect = ClientRectangle;
            panelRect.Inflate(-30, -30);
            pnlNonTransparent.Bounds = panelRect;
            ExcludeControlFromAeroGlass(pnlNonTransparent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdRename_Click(object sender, EventArgs e)
        {
            try
            {
                ForMain.FtpClient.CurrentDirectory = Path;
                ForMain.FtpClient.FtpRename(FileName, txtRenameTo.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }
        #endregion
    }
}
