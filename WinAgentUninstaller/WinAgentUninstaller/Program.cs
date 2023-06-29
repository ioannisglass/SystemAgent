using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinAgentUninstaller
{
    internal class Program
    {
        public static string g_strScanHome = string.Empty;
        private static string varAgentEnv = "AGENT_HOME";
        public static string strAgentPath = string.Empty;
        static void Main(string[] args)
        {
            bool isAdmin = ProcessExts.isUserAnAdmin();

            if (isAdmin)
            {
                Console.WriteLine("Command Prompt is running as administrator.");
            }
            else
            {
                Console.WriteLine("Run Command Prompt as administrator.");
                return;
            }

            strAgentPath = System.Environment
                .GetEnvironmentVariable(varAgentEnv, EnvironmentVariableTarget.Machine);

            if (strAgentPath == null || strAgentPath != AppDomain.CurrentDomain.BaseDirectory)
            {
                EnvironmentPermission permissions = new EnvironmentPermission(EnvironmentPermissionAccess.AllAccess, varAgentEnv);
                permissions.Demand();
                Environment.SetEnvironmentVariable(varAgentEnv, AppDomain.CurrentDomain.BaseDirectory,
                    EnvironmentVariableTarget.Machine);
            }

            string w_strSvcFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentSvc.exe");

            int w_nRetryNum = 0;
            if (ServiceExts.IsServiceInstalled())
            {
                if (ServiceExts.GetWindowsServiceStatus() == "Running")
                {
                    Console.WriteLine("Service is running. Service will be stopped.");
                    ServiceExts.StopService();
                }
                ServiceExts.UninstallService("WinAgentSvc.exe");

                while (true)
                {
                    if (ServiceExts.IsServiceInstalled())
                    {
                        if (w_nRetryNum >= 5)
                        {
                            Console.WriteLine("Service can not be uninstalled.");
                            return;
                        }
                        else
                        {
                            w_nRetryNum++;
                            Thread.Sleep(3000);
                        }
                    }
                    else
                        break;
                }
            }
            else
                Console.WriteLine("Service is not installed.");
            Console.WriteLine("Service is uninstalled successfully.");

            // Check if the WinAgentSvc.exe is existed. If yes, remove it.
            if (File.Exists(w_strSvcFullPath))
            {
                try
                {
                    File.Delete(w_strSvcFullPath);
                    Console.WriteLine("WinAgnetSvc.exe removed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WinAgnetSvc.exe can not be removed. Error({ex.Message})");
                }
            }
            else
                Console.WriteLine("No WinAgnetSvc.exe.");
        }
    }
}
