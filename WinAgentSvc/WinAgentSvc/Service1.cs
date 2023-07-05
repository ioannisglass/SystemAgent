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
using System.IO;

namespace WinAgentSvc
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        bool m_bPosted = false;

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
            // timer.Interval = 3600000; //number in milisecinds
            timer.Interval = 60000; //number in milisecinds
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            DateTime newDT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
            SvcLogger.log(newDT.ToString());
            
            string w_strUninstallerPath = Path.Combine(Program.g_base_path, "WinAgentUninstaller.exe");
            string w_strUpdatePath = Path.Combine(Program.g_base_path, "WinAgentUpdate.exe");

            int w_nRet = AgentHelper.checkActivated(Program.g_strCusID, Program.g_strActkey);
            if (w_nRet != ConstEnv.AGENT_REGISTERED && w_nRet != ConstEnv.API_SERVER_ERROR)
            {
                SvcLogger.log("Activation Failed");
                Process.Start(w_strUninstallerPath);
            }
            else if (w_nRet == ConstEnv.AGENT_REGISTERED)
            {
                if (newDT.Hour == 17)
                {
                    if (!m_bPosted)
                        submitData();
                    m_bPosted = true;
                }
                else
                    m_bPosted = false;

                bool w_bAgentUpdated = AgentHelper.isAgentChanged();
                if (w_bAgentUpdated)
                {
                    Process.Start(w_strUpdatePath);
                }
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
            w_mAgentData.auth.customerid = Program.g_strCusID;
            w_mAgentData.auth.actkey = Program.g_strActkey;

            // List<MInstalledApp> w_lstmApps = OSInfoHelper.getFullThirdPartyApps();
            List<MInstalledApp> w_lstmApps = OSInfoHelper.getExactNameAppList();
            w_mAgentData.installedApps.AddRange(w_lstmApps);

            string w_strRet = AgentHelper.postAgentData(w_mAgentData);
        }
    }
}
