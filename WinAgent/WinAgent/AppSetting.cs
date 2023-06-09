using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailParser
{
    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.ini";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception exception)
            {
                MyLogger.Error($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
            }
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(pSettings, Newtonsoft.Json.Formatting.Indented));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
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
                MyLogger.Error($"Exception Error ({System.Reflection.MethodBase.GetCurrentMethod().Name}): {exception.Message + "\n" + exception.StackTrace}");
                return default(T);
            }
        }
    }

    public class UserSetting : AppSettings<UserSetting>
    {
        public string customer_id = "testagent1";
        public string activation_key = "activationkey1";
        public string api_base = "http://localhost:5000";
        public string api_activate = "/api/activate";
        public string api_submit = "/api/submit";
    }
}
