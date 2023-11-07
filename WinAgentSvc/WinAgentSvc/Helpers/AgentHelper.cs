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
            string w_strLocalSvcMD5 = FileAttrHelper.calculateMD5(w_strSvcFullPath);

            var w = new WebClient();
            string w_strServerSvcMD5 = w.DownloadString(ConstEnv.API_SVC_SIZE);
            if (w_strLocalSvcMD5 == w_strServerSvcMD5)
                return false;
            else
                return true;
        }

        public static List<string> GetAppsToRemove(string _strHost)
        {
            try
            {
                if (string.IsNullOrEmpty(_strHost))
                    return new List<string>();
                string w_strURL = ConstEnv.API_BASE_URL + ConstEnv.API_APPTOREMOVE;
                // MAuth w_mAuth = new MAuth(_strCusID, _strActKey);
                // string w_strPostData = $"cusid={_strCustomerID}&actkey={_strActivationKey}";
                // string w_strPostData = JsonConvert.SerializeObject(w_mAuth, Formatting.Indented);
                // string w_strResponse = WebReqHelper.postData(w_strPostData, w_strURL, "application/json");

                w_strURL = $"{w_strURL}?host={_strHost}";
                WebClient wc = new WebClient();
                string w_strResponse = wc.DownloadString(w_strURL);
                List<string> w_lstrAppsToRemove = JsonConvert.DeserializeObject<List<string>>(w_strResponse);
                return w_lstrAppsToRemove;
            }
            catch (Exception ex)
            {
                SvcLogger.log("Failed to get apps to remove : " + ex.Message);
                return new List<string>();
            }
        }
    }
}
