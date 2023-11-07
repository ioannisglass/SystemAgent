using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.BaseModel
{
    [Serializable]
    public class MAppData
    {
        public string name { get; set; }
        public string dis { get; set; }
        public string loc { get; set; }
        public string ver { get; set; }
        public string uns { get; set; }
        public string quns { get; set; }
        public string pub { get; set; }
        public string reg { get; set; }

        public MAppData()
        {
            name = string.Empty;
            dis = string.Empty;
            loc = string.Empty;
            ver = string.Empty;
            uns = string.Empty;
            quns = string.Empty;
            pub = string.Empty;
            reg = string.Empty;
        }

        public MAppData(MAppData _mAppData)
        {
            name = _mAppData.name;
            dis = _mAppData.dis;
            loc = _mAppData.loc;
            ver = _mAppData.ver;
            uns = _mAppData.uns;
            quns= _mAppData.quns;
            pub = _mAppData.pub;
            reg = _mAppData.reg;
        }
    }
}
