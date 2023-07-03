using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Security;
using System.Security.Principal;

namespace WinAgentUninstaller
{
    public static class ProcessExts
    {
        public static Process[] GetRunningProcessList()
        {
            Process[] processlist = Process.GetProcesses();

            return processlist;
            /*foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);

                string str_full_path = theprocess.MainModule.FileName;
                Icon ico = Icon.ExtractAssociatedIcon(str_full_path);
                using (FileStream fs = new FileStream("icons\\test.ico", FileMode.Create))
                    ico.Save(fs);
            }*/
        }

        public static string GetProcessPath(Process _pro)
        {
            try
            {
                return _pro.MainModule.FileName;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public static string GetExeName(Process _pro)
        {
            try
            {
                return Path.GetFileName(GetProcessPath(_pro));
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public static string GetProcessName(Process _pro)
        {
            try
            {
                return _pro.ProcessName;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public static bool executeCommand(string command, out string err_msg)
        {
            err_msg = string.Empty;
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
                err_msg = ex.Message;
                Console.WriteLine("Failed to Launch Application : " + ex.Message);
            }

            return w_ret;
        }

        private static void processValueNames(RegistryKey Key)
        { //function to process the valueNames for a given key
            string[] valuenames = Key.GetValueNames();
            if (valuenames == null || valuenames.Length <= 0) //has no values
                return;
            foreach (string valuename in valuenames)
            {
                object obj = Key.GetValue(valuename);
                if (obj != null && valuename.Contains("SteamService.exe"))
                {
                    Debug.WriteLine(Key.Name + " " + valuename + " " + obj.ToString()); //assuming the output to be in comboBox1 in string type
                    FileInfo info = new FileInfo(valuename);
                    // Program.g_strSteamPath = info.DirectoryName;
                }
            }
        }

        public static void OutputRegKey(RegistryKey Key)
        {
            try
            {
                string[] subkeynames = Key.GetSubKeyNames(); //means deeper folder
                if (subkeynames == null || subkeynames.Length <= 0)
                { //has no more subkey, process
                    processValueNames(Key);
                    return;
                }
                foreach (string keyname in subkeynames)
                { //has subkeys, go deeper
                    using (RegistryKey key2 = Key.OpenSubKey(keyname))
                        OutputRegKey(key2);
                }
                processValueNames(Key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //error, do something
            }
        }

        public static void unblockFile(string filePath)
        {
            // Create a FileIOPermission to modify the file's security attributes
            FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.Write, filePath);

            try
            {
                // Check if the current security context has the required permission
                filePermission.Demand();

                // Open the file and set the Unblock flag using the alternate data stream
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Write))
                {
                    fileStream.WriteByte(0);
                }
            }
            catch (SecurityException)
            {
                // Handle the exception if the required permission is not available
                // or if the file is in a restricted location
                // (e.g., system directories, read-only locations)
                Console.WriteLine("Access denied. Unable to unblock the file.");
            }
        }

        public static void unblockFileByShell(string filePath)
        {
            string powershellCommand = $"Unblock-File -Path '{filePath}'";
            string escapedCommand = $"-ExecutionPolicy Bypass -Command \"{powershellCommand}\"";

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "powershell.exe";
            processStartInfo.Arguments = escapedCommand;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();

                // Read the output of the PowerShell command
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                // Check the exit code to determine if the command executed successfully
                int exitCode = process.ExitCode;
                if (exitCode == 0)
                {
                    Console.WriteLine("File unblocked successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to unblock file.");
                }

                Console.WriteLine("PowerShell Output:");
                Console.WriteLine(output);
            }
        }

        public static bool isUserAnAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
