﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public static object g_objLock = new object();
        public static string g_log_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");
        static void Main(string[] args)
        {
            // if (args.Length != 2)
            // {
            //     SvcLogger.log("No Customer ID and Activation Key.");
            //     return;
            // }
            // string[] args = Environment.GetCommandLineArgs();
            // g_strCustomerID = args[0];
            // g_strActivationKey = args[1];
            
            // g_setting = UserSetting.Load();
            // if (g_setting == null)
            // {
            //     g_setting = new UserSetting();
            //     g_setting.Save();
            // }

            if (!File.Exists("key.ini"))
            {
                SvcLogger.log("No key file.");
                return;
            }
            try
            {
                string[] w_strrLines = File.ReadAllLines("key.ini");
                g_strCustomerID = w_strrLines[0];
                g_strActivationKey = w_strrLines[1];
            }
            catch (Exception ex)
            {
                SvcLogger.log(ex.Message);
            }

            int w_nRet = AgentHelper.checkActivated(g_strCustomerID, g_strActivationKey);
            if (w_nRet == ConstEnv.AGENT_REGISTERED)
                SvcLogger.log("Customer is Activated.");
            else if (w_nRet == ConstEnv.API_SERVER_ERROR)
            {
                SvcLogger.log("Server No Response.");
                Environment.Exit(0);
            }
            else
            {
                SvcLogger.log("Activation Failed");
                Environment.Exit(0);
            }
            // else if (w_nRet == ConstEnv.AGENT_NO_ACTIVATED)
            // {
            //     Console.WriteLine("Customer not Activated.");
            //     Environment.Exit(0);
            // }
            // else if (w_nRet == ConstEnv.AGENT_NO_REGISTERED)
            // {
            //     Console.WriteLine("No Registered Customer.");
            //     Environment.Exit(0);
            // }
            // else
            // {
            //     Console.WriteLine("Server No Response.");
            //     Environment.Exit(0);
            // }

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
            ProcessExts.unblockFileByShell(w_strSvcFullPath);
            ServiceExts.InstallService("WinAgentSvc.exe");

            int w_nRetryNum = 0;
            while (true)
            {
                if (!ServiceExts.IsServiceInstalled())
                {
                    if (w_nRetryNum >= 5)
                    {
                        SvcLogger.log("Service is not installed.");
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
                SvcLogger.log("Service is running already.");
                return;
            }
            ServiceExts.StartService(args);
        }
    }
}
