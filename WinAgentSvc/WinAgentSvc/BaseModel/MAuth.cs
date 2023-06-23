using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModel
{
    [Serializable]
    public class MAuth
    {
        public string cusid { get; set; }
        public string actkey { get; set; }
        public MAuth()
        {
            cusid = string.Empty;
            actkey = string.Empty;
        }
        public MAuth(string cusid, string actkey)
        {
            this.cusid = cusid;
            this.actkey = actkey;
        }
    }
}
