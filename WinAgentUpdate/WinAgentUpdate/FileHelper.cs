using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentUpdate
{
    public static class FileHelper
    {
        public static void deleteSelf()
        {
            string currentPath = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " + currentPath;
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
        }

        public static string getSelfName()
        {
            string currentPath = Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetFileName(currentPath);
        }
    }
}
