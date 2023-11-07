using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModule
{
    public static class ProcessExts
    {
        public static Process[] GetRunningProcessList()
        {
            Process[] processlist = Process.GetProcesses();

            return processlist;
        }

        public static string GetProcessPath(Process _pro)
        {
            try
            {
                return _pro.MainModule.FileName;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static bool CheckRunning(string _proc_name)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process runningProc in processlist)
            {
                try
                {
                    if (runningProc.MainModule.FileName == _proc_name)
                        return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            return false;
        }

        public static void RunCmdFlag(string proc_path)
        {
            string arg = $"-k 100 -n 20 {proc_path}";
            ProcessExtensions.StartProcessAsCurrentUser("c:\\blah\\blah.exe", arg);
        }

        public static bool ExecuteCommand(string command, out string err_msg)
        {
            bool w_ret = false;
            err_msg = string.Empty;
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = $"cmd.exe";
                startInfo.Arguments = "/C " + command;
                process.StartInfo = startInfo;
                // process.Start();
                using (var processHandler = Process.Start(startInfo))
                    processHandler.WaitForExit();
                w_ret = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Launch Application : " + ex.Message);
            }
            return w_ret;
        }
    }
}
