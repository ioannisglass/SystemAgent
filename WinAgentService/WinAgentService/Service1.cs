using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using WinAgentService;
using WinAgentService.BaseModule;
using Newtonsoft.Json;
using System.IO;
using System.Runtime;
using Helpers;
using WinAgentService.BaseModel;
using WinAgent.Helpers;
using WinAgentService.Helpers;

namespace WinAgentService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        // string strScanHome = string.Empty;
        // private string varScanApp = "AGENT_HOME";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // strScanHome = System.Environment
            //     .GetEnvironmentVariable(varScanApp, EnvironmentVariableTarget.Machine);
            SvcLogger.log("OnStart");
            string w_strArgs = string.Empty;
            foreach (string arg in args)
                w_strArgs += $":{arg}";
            SvcLogger.log($"Arg: {w_strArgs}");
            Program.g_strCusID = args[0];
            Program.g_strActkey = args[1];

            GetConfigFile();

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 3600000; //number in milisecinds
            // timer.Interval = 10000; //number in milisecinds
            timer.Enabled = true;

            // scheduled function running
            // DateTime w_dteCstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            // TimeSpan targetTime = new TimeSpan(10, 30, 0);
            // DateTime w_dteTargetUtc = new DateTime(w_dteUtcNow.Year, w_dteUtcNow.Month, w_dteUtcNow.Day, 17, 0, 0);
            // DateTime w_dteTargetCst = new DateTime(w_dteCstNow.Year, w_dteCstNow.Month, w_dteCstNow.Day, 7, 0, 0);

            // if (w_dteCstNow > w_dteTargetCst)
            // {
            //     // If the target time has already passed for today, schedule it for the next day
            //     w_dteTargetCst = w_dteTargetCst.AddDays(1);
            // }
            // TimeSpan delay = w_dteTargetCst - w_dteCstNow;
            // // Create a timer that will execute the function every day at the specified time
            // System.Threading.Timer w_tmrSchedule = new System.Threading.Timer(DoSomething, null, delay, TimeSpan.FromDays(1));
        }
        public void DoSomething(object state)
        {
            SvcLogger.log("Submit executed at: " + TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone));
            submitData();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DateTime newDT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            SvcLogger.log(newDT.ToString());
            if (newDT.Hour == 17)
            {
                submitData();
            }
        }

        protected override void OnStop()
        {
        }
        public void submitData()
        {
            MAgentData w_mAgentData = new MAgentData();
            w_mAgentData.osInfo = OSInfoHelper.getOSFullName() + $" {OSInfoHelper.getOSbit()} ({OSInfoHelper.getOSDescription()})";
            w_mAgentData.version = OSInfoHelper.getOSVersion();
            w_mAgentData.machineName = OSInfoHelper.getMachineName();
            // w_mAgentData.auth.cusid = Program.g_setting.customer_id;
            // w_mAgentData.auth.actkey = Program.g_setting.activation_key;
            w_mAgentData.auth.cusid = Program.g_strCusID;
            w_mAgentData.auth.actkey = Program.g_strActkey;

            w_mAgentData.installedApps.AddRange(OSInfoHelper.getFullThirdPartyApps());

            string w_strRet = AgentHelper.postAgentData(w_mAgentData);
            SvcLogger.log(w_strRet);
        }
        public void GetConfigFile()
        {
            Program.g_setting = UserSetting.Load(Program.g_setting_path);
            if (Program.g_setting == null)
            {
                Program.g_setting = new UserSetting();
                Program.g_setting.Save(Program.g_setting_path);
            }
            SvcLogger.log(File.ReadAllText(Program.g_setting_path));
        }
    }
}
