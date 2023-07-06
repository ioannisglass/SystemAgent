using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinAgentUpdate
{
    internal class Program
    {
        public static string g_strScanHome = string.Empty;
        private static string varAgentEnv = "AGENT_HOME";
        public static string strAgentPath = string.Empty;
        public static object g_objLock = new object();
        public static string g_log_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.log");
        static void Main(string[] args)
        {
            try
            {
                bool isAdmin = ProcessExts.isUserAnAdmin();

                if (isAdmin)
                    SvcLogger.log("Command Prompt is running as administrator.");
                else
                {
                    SvcLogger.log("Run Command Prompt as administrator.");
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
                        SvcLogger.log("Service is running. Service will be stopped.");
                        ServiceExts.StopService();
                    }
                    ServiceExts.UninstallService("WinAgentSvc.exe");

                    while (true)
                    {
                        if (ServiceExts.IsServiceInstalled())
                        {
                            if (w_nRetryNum >= 5)
                            {
                                SvcLogger.log("Service can not be uninstalled.");
                                return;
                            }
                            else
                            {
                                w_nRetryNum++;
                                Thread.Sleep(3000);
                            }
                        }
                        else
                        {
                            SvcLogger.log("Service is uninstalled successfully.");
                            break;
                        }
                    }
                }
                else
                    SvcLogger.log("Service is not installed.");

                // remove previous WinAg
                w_nRetryNum = 0;
                while (true)
                {
                    try
                    {
                        File.Delete(w_strSvcFullPath);
                        SvcLogger.log($"{w_strSvcFullPath} removed.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        SvcLogger.log(ex.Message);
                        SvcLogger.log("Will try to remove again.");
                        w_nRetryNum++;
                        if (w_nRetryNum >= 5)
                        {
                            SvcLogger.log($"{w_strSvcFullPath} can not be removed.");
                            return;
                        }
                        Thread.Sleep(3000);
                    }
                }

                AgentHelper.downloadSvc();
                Thread.Sleep(1500);
                // ProcessExtensions.StartProcessAsCurrentUser("WinAgentInstaller.exe", $"{w_strCustomerID} {w_strActivationKey}");
                // Process.Start("WinAgentInstaller.exe", $"{w_strCustomerID} {w_strActivationKey}");
                Process.Start("WinAgentInstaller.exe");
            }
            catch(Exception e)
            {
                SvcLogger.log(e.Message);
            }
        }
    }
}
