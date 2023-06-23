using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModule
{
    public class ProcessExts
    {
        public Process[] GetRunningProcessList()
        {
            Process[] processlist = Process.GetProcesses();

            return processlist;
        }

        public bool CheckRunning(string _proc_name)
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

        public void RunCmdFlag(string proc_path)
        {
            string arg = $"-k 100 -n 20 {proc_path}";
            ProcessExtensions.StartProcessAsCurrentUser("c:\\blah\\blah.exe", arg);
        }

        private bool executeCommand(string command)
        {
            bool w_ret = false;
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = $"cmd.exe";
                startInfo.Arguments = "/C " + command;
                process.StartInfo = startInfo;
                process.Start();

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
