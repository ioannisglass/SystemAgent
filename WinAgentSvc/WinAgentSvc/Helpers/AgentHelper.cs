using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WinAgentSvc.BaseModel;

namespace WinAgentSvc.Helpers
{
    public static class AgentHelper
    {
        public static int checkActivated(string _strCustomerID, string _strActivationKey)
        {
            // string w_strURL = Program.g_setting.api_base + Program.g_setting.api_activate;
            string w_strURL = ConstEnv.API_BASE_URL + ConstEnv.API_AUTH_URL;
            int w_nRet = ConstEnv.API_SERVER_ERROR;
            MAuth w_mAuth = new MAuth(_strCustomerID, _strActivationKey);
            // string w_strPostData = $"cusid={_strCustomerID}&actkey={_strActivationKey}";
            string w_strPostData = JsonConvert.SerializeObject(w_mAuth, Formatting.Indented);
            string w_strResponse = WebReqHelper.postData(w_strPostData, w_strURL, "application/json");

            bool w_bRet = int.TryParse(w_strResponse, out w_nRet);
            if (!w_bRet)
                return ConstEnv.API_SERVER_ERROR;
            else
                return w_nRet;
        }

        public static string postAgentData(MAgentData _mAgentData)
        {
            // string w_strURL = Program.g_setting.api_base + Program.g_setting.api_submit;
            string w_strURL = ConstEnv.API_BASE_URL + ConstEnv.API_SUBMIT_URL;
            // int w_nRet = 0;
            string w_strPostData = JsonConvert.SerializeObject(_mAgentData, Formatting.Indented);
            string w_strResponse = WebReqHelper.postData(w_strPostData, w_strURL, "application/json");
            return w_strResponse;
        }
        public static bool isAgentChanged()
        {
            string w_strSvcFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinAgentSvc.exe");
            long w_lSvcSize = new FileInfo(w_strSvcFullPath).Length;

            var w = new WebClient();
            string w_strSvcSize = w.DownloadString(ConstEnv.API_SVC_SIZE);
            if (w_strSvcSize == w_lSvcSize.ToString())
                return false;
            else
                return true;
        }
    }
}
