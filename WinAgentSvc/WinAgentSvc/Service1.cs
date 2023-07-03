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
using WinAgentSvc.Helpers;
using WinAgentSvc.BaseModel;
using Newtonsoft.Json;

namespace WinAgentSvc
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SvcLogger.log("OnStart");
            Program.g_strCusID = args[0];
            Program.g_strActkey = args[1];
            SvcLogger.log($"{Program.g_strCusID}:{Program.g_strActkey}");

            submitData();                       // Submit data on first start
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 3600000; //number in milisecinds
            // timer.Interval = 20000; //number in milisecinds
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DateTime newDT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            SvcLogger.log(newDT.ToString());

            bool w_bAgentUpdated = AgentHelper.isAgentNew();
            if (w_bAgentUpdated)
            {

            }

            if (newDT.Hour == 17)
            {
                int w_nRet = AgentHelper.checkActivated(Program.g_strCusID, Program.g_strActkey);
                if (w_nRet == ConstEnv.AGENT_REGISTERED)
                    submitData();
                else if (w_nRet == ConstEnv.API_SERVER_ERROR)
                    Console.WriteLine("Server No Response.");
                else
                    Console.WriteLine("Activation Failed");
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

            List<MInstalledApp> w_lstmApps = OSInfoHelper.getFullThirdPartyApps();
            w_mAgentData.installedApps.AddRange(w_lstmApps);

            string w_strRet = AgentHelper.postAgentData(w_mAgentData);
        }
    }
}
