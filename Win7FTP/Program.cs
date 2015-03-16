using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Win7FTP
{
    static class Program
    {
        [STAThread]
        static void Main()
        {        
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }


    }
}
