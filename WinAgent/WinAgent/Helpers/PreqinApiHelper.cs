using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Helpers
{
    public static class WebReqHelper
    {
        public static bool IgnoreCertificateValidationErrors(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // You can add custom logic here to validate or ignore specific certificate errors
            // For example, you can return true to ignore all certificate errors, but exercise caution when doing this
            return true;
        }
        public static string post_x_www_form_urlencoded(string _strToPost, string _strURL)
        {
            string w_strToken = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_strURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
            request.Accept = "*/*";
            request.AllowAutoRedirect = true;
            request.ServerCertificateValidationCallback += IgnoreCertificateValidationErrors;
            ServicePointManager.ServerCertificateValidationCallback += IgnoreCertificateValidationErrors;
            request.KeepAlive = true;
            // request.UseDefaultCredentials = true;
            // request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            // request.Headers.Add("Accept-Encoding", "gzip, deflate, br");

            string w_strResponse = string.Empty;
            byte[] w_bytePostData = Encoding.UTF8.GetBytes(_strToPost);
            request.ContentLength = w_bytePostData.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(w_bytePostData, 0, w_bytePostData.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream respStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(respStream);
                w_strResponse = reader.ReadToEnd();
            }

            return w_strToken;
        }

        public static string get_x_www_form_urlencoded(string _strURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_strURL);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36";
            request.Accept = "*/*";
            request.AllowAutoRedirect = true;
            request.ServerCertificateValidationCallback += IgnoreCertificateValidationErrors;
            ServicePointManager.ServerCertificateValidationCallback += IgnoreCertificateValidationErrors;
            request.KeepAlive = true;
            // request.UseDefaultCredentials = true;
            // request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            // request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            // request.Headers.Add("Authorization", $"Bearer {_strToken}");

            string w_strResponse = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream respStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(respStream);
                w_strResponse = reader.ReadToEnd();
            }
            return w_strResponse;
        }
    }
}
