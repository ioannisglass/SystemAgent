using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinAgentSvc.BaseModel;
using WinAgentSvc.Helpers;

namespace WinAgentSvc
{
    public class AppSettings<T> where T : new()
    {
        public void Save(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception exception)
            {
                SvcLogger.log($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
            }
        }

        public static void Save(T pSettings, string fileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(pSettings, Newtonsoft.Json.Formatting.Indented));
        }

        public static T Load(string fileName)
        {
            try
            {
                T t = new T();
                if (File.Exists(fileName))
                    t = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
                else
                    return default(T);
                return t;
            }
            catch (Exception exception)
            {
                SvcLogger.log($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
                return default(T);
            }
        }
    }

    public class UserSetting : AppSettings<UserSetting>
    {
        public Dictionary<string, MAppData> app_data = new Dictionary<string, MAppData>();
    }
}
