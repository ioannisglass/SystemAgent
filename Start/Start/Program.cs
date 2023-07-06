using Start;
using Start.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Remoting.Channels;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.IO;

namespace WinAgentUninstaller
{
    internal class Program
    {
        public static string g_strScanHome = string.Empty;
        private static string varAgentEnv = "AGENT_HOME";
        public static string strAgentPath = string.Empty;
        static void Main(string[] args)
        {
            string assemblyPath = Assembly.GetEntryAssembly().Location;

            // Unblock the file if it is blocked
            UnblockFile(assemblyPath);

            bool isAdmin = ProcessExts.isUserAnAdmin();

            if (isAdmin)
                Console.WriteLine("Command Prompt is running as administrator.");
            else
            {
                Console.WriteLine("Run Command Prompt as administrator.");
                return;
            }

            if (ServiceExts.IsServiceInstalled())
            {
                Console.WriteLine("Service is already installed.");
                return;
            }

            if (args.Length != 2)
            {
                Console.WriteLine("No Customer ID and Activation Key.");
                return;
            }
            string w_strCustomerID = args[0];
            string w_strActivationKey = args[1];

            File.WriteAllLines("key.ini", new string[] { w_strCustomerID, w_strActivationKey });
            extractAllFiles();
            Thread.Sleep(1500);
            // ProcessExtensions.StartProcessAsCurrentUser("WinAgentInstaller.exe", $"{w_strCustomerID} {w_strActivationKey}");
            // Process.Start("WinAgentInstaller.exe", $"{w_strCustomerID} {w_strActivationKey}");
            Process.Start("WinAgentInstaller.exe");
        }
        private static void UnblockFile(string filePath)
        {
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Unblock the file
                // FileInfo fileInfo = new FileInfo(filePath);
                // fileInfo.Unblock();
                unblockFileByShell(filePath);
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
        public static void extractAllFiles()
        {
            
            string w_strInstaller = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentInstaller.exe");
            File.WriteAllBytes(w_strInstaller, Resources.WinAgentInstaller);

            string w_strInstallUtil = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallUtil.exe");
            File.WriteAllBytes(w_strInstallUtil, Resources.InstallUtil);

            string w_strUninstaller = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentUninstaller.exe");
            File.WriteAllBytes(w_strUninstaller, Resources.WinAgentUninstaller);

            string w_strSvc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentSvc.exe");
            File.WriteAllBytes(w_strSvc, Resources.WinAgentSvc);

            string w_strUpdate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentUpdate.exe");
            File.WriteAllBytes(w_strUpdate, Resources.WinAgentUpdate);

            string w_strJsonDLL = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Newtonsoft.Json.dll");
            File.WriteAllBytes(w_strJsonDLL, Resources.Newtonsoft_Json);

            string w_strWinmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows.winmd");
            File.WriteAllBytes(w_strWinmd, Resources.Windows);
        }
    }
}
