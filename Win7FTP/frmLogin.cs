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

namespace Win7FTP
{
    public partial class frmLogin : GlassForm
    {
        public static FTPclient objFtp;


        #region Contructor
        public frmLogin()
        {

            InitializeComponent();


            AeroGlassCompositionChanged += new AeroGlassCompositionChangedEvent(Form1_AeroGlassCompositionChanged);

            if (AeroGlassCompositionEnabled)
            {

                ExcludeControlFromAeroGlass(pnlNonTransparent);
            }
            else
            {
                this.BackColor = Color.Teal;
            }
        }
        #endregion

        #region Events
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

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {

                FTPclient objFtp = new FTPclient(txtHostName.Text, txtUserName.Text, txtPassword.Text);
                objFtp.CurrentDirectory = "/";              
                frmMain  frm= new frmMain();
           

                frm.SetFtpClient(objFtp);


                frm.ShowDialog();
                this.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        #endregion
    }
}
