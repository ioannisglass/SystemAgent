using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static UserSetting g_setting = new UserSetting();
        public static List<string> g_lstBaseProcList = new List<string>();
        public static string g_base_path = System.Environment
                .GetEnvironmentVariable("SCAN_HOME", EnvironmentVariableTarget.Machine);
        public static string g_log_path = Path.Combine(g_base_path, "main.log");
        static void Main()
        {
            g_setting = UserSetting.Load();
            if (g_setting == null)
            {
                g_setting = new UserSetting();
                g_setting.Save();
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
