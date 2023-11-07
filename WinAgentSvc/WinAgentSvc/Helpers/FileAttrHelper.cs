using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WinAgentSvc.Helpers
{
    public static class FileAttrHelper
    {
        public static string calculateMD5(string _strFilename)
        {
            string w_strRet = string.Empty;
            if (!File.Exists(_strFilename))
                return w_strRet;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(_strFilename))
                {
                    var hash = md5.ComputeHash(stream);
                    w_strRet = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            return w_strRet;
        }

        public static long getFileSize(string _strFilename)
        {
            if (!File.Exists(_strFilename))
                return 0;
            long w_lSvcSize = new FileInfo(_strFilename).Length;
            return w_lSvcSize;
        }
    }
}
