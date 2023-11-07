using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WinAgentSvc.BaseModel;
using WinAgentSvc.Helpers;

namespace WinAgentSvc
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static string g_base_path = System.Environment
                .GetEnvironmentVariable("AGENT_HOME", EnvironmentVariableTarget.Machine);
        public static string g_log_path = Path.Combine(g_base_path, "main.log");
        // public static string g_setting_path = Path.Combine(g_base_path, "settings.ini");
        public static object g_objLock = new object();
        public static string g_strCusID = string.Empty;
        public static string g_strActkey = string.Empty;
        // public static UserSetting g_setting = new UserSetting();
        public static Dictionary<string, MAppData> g_dictAppData = new Dictionary<string, MAppData>();
        static void Main()
        {
            SvcLogger.log("Main function.");
            SvcLogger.log($"Log path: {g_log_path}");
            // SvcLogger.log($"Setting path: {g_setting_path}");

            // g_setting = UserSetting.Load(Program.g_setting_path);
            // if (g_setting == null)
            // {
            //     g_setting = new UserSetting();
            //     g_setting.Save(Program.g_setting_path);
            // }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
