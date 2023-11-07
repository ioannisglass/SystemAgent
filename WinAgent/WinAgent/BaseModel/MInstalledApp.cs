using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAgent.BaseModel
{
    [Serializable]
    public class MInstalledApp
    {
        public string name { get; set; }
        public string loc { get; set; }
        public string ver { get; set; }
        public string pub { get; set; }
        public string uns { get; set; }
    }
}
