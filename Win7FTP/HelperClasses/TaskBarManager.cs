using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Reflection;

namespace Win7FTP.HelperClasses
{
    public class TaskBarManager
    {

        public static void SetProgressValue(int Progress, int Maximum)
        {
            TaskbarProgressBarState state = TaskbarProgressBarState.Normal;
            TaskbarManager.Instance.SetProgressState(state);
            TaskbarManager.Instance.SetProgressValue(Progress, Maximum);
        }

        public static void SetTaskBarProgressState(TaskbarProgressBarState state)
        {
            TaskbarManager.Instance.SetProgressState(state);
        }
        public static void ClearProgressValue()
        {
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
        }
    }
}
