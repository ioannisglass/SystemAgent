using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace WinAgentService
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
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            return false;
        }

        public void RunAlert(string msg)
        {
            string path = System.Environment
                .GetEnvironmentVariable("SCAN_HOME", EnvironmentVariableTarget.Machine);

            System.Threading.Thread.Sleep(1000);
            string msgbx_path = Path.Combine(path, "AlertStatus.exe");

            msg = $"\"{msg}\"";

            ProcessExtensions.StartProcessAsCurrentUser(msgbx_path, msg);
        }

        public void RunNotePad(string text)
        {
            System.Threading.Thread.Sleep(1000);

            string path = System.Environment
                .GetEnvironmentVariable("SCAN_HOME", EnvironmentVariableTarget.Machine);
            path = Path.Combine(path, "temp.txt");

            File.WriteAllText(path, text);
            path = $"\"{path}\"";
            ProcessExtensions.StartProcessAsCurrentUser("notepad.exe", $"{path} {path}");
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
