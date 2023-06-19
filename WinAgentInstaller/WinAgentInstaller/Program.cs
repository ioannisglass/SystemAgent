using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinAgentInstaller
{
    internal class Program
    {
        public static string g_strScanHome = string.Empty;
        private static string varAgentEnv = "AGENT_HOME";
        public static string strAgentPath = string.Empty;
        public static string g_strCustomerID = string.Empty;
        public static string g_strActivationKey = string.Empty;
        public static UserSetting g_setting = new UserSetting();
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("No Customer ID and Activation Key.");
                return;
            }
            // string[] args = Environment.GetCommandLineArgs();
            g_strCustomerID = args[0];
            g_strActivationKey = args[1];
            
            g_setting = UserSetting.Load();
            if (g_setting == null)
            {
                g_setting = new UserSetting();
                g_setting.Save();
            }

            int w_nRet = AgentHelper.checkActivated(g_strCustomerID, g_strActivationKey);
            if (w_nRet == ConstEnv.AGENT_REGISTERED)
                Console.WriteLine("Customer is Activated.");
            else if (w_nRet == ConstEnv.AGENT_NO_ACTIVATED)
            {
                Console.WriteLine("Customer not Activated.");
                Environment.Exit(0);
            }
            else if (w_nRet == ConstEnv.AGENT_NO_REGISTERED)
            {
                Console.WriteLine("No Registered Customer.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Server No Response.");
                Environment.Exit(0);
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

            ServiceExts.InstallService("WinAgentService.exe");

            int w_nRetryNum = 0;
            while (true)
            {
                if (!ServiceExts.IsServiceInstalled())
                {
                    if (w_nRetryNum >= 5)
                    {
                        Console.WriteLine("Service is not installed.");
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

            if (ServiceExts.GetWindowsServiceStatus() == "Running")
            {
                Console.WriteLine("Service is running already.");
                return;
            }

            ServiceExts.StartService(args);
        }
    }
}
