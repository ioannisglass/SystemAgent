using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentUpdate
{
    public class AgentHelper
    {
        public static bool isAgentNew()
        {

            return true;
        }
        public static void downloadSvc()
        {
            string w_strSvcFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentSvc.exe");
            WebClient wc = new WebClient();
            wc.DownloadFile(ConstEnv.API_SVC_FILE_URL, w_strSvcFullPath);
        }
    }
}
