using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Win7FTP
{
    public partial class frmNewFolder : Form
    {

        public string NewDirName = null;


        #region Constructor
        public frmNewFolder()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtNewDir.Text != null)
            {
                NewDirName = txtNewDir.Text;
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion
    }
}
