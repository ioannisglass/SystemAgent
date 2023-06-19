using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentInstaller
{
    public static class AgentHelper
    {
        public static int checkActivated(string _strCustomerID, string _strActivationKey)
        {
            string w_strURL = Program.g_setting.api_base + Program.g_setting.api_activate;
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
    }
}
