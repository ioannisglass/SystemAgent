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
        public static object g_objLock = new object();
        public static string g_log_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uninstall.log");
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

                // string w_strSvcFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentSvc.exe");

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

                string w_strSelfName = FileHelper.getSelfName();
                string[] w_strrFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
                foreach(string w_strFile in  w_strrFiles)
                {
                    w_nRetryNum = 0;
                    string w_strFileName = Path.GetFileName(w_strFile);
                    while (true)
                    {
                        try
                        {
                            if (w_strFileName != w_strSelfName && w_strFileName != "Start.exe")
                                File.Delete(w_strFile);
                            break;
                        }
                        catch (Exception ex)
                        {
                            w_nRetryNum++;
                            if (w_nRetryNum >= 5)
                            {
                                SvcLogger.log(ex.Message);
                                SvcLogger.log($"{w_strFileName} can not be removed.");
                                break;
                            }
                            Thread.Sleep(3000);
                        }
                    }
                }
                FileHelper.deleteSelf();
            }
            catch(Exception e)
            {
                SvcLogger.log(e.Message);
            }
        }
    }
}
