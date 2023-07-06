using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgent.BaseModel
{
    [Serializable]
    public class MAuth
    {
        public string customerid { get; set; }
        public string actkey { get; set; }
        public MAuth()
        {
            customerid = string.Empty;
            actkey = string.Empty;
        }
        public MAuth(string customerid, string actkey)
        {
            this.customerid = customerid;
            this.actkey = actkey;
        }
    }
}
