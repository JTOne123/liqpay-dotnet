using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiqPaySDK
{
    public class LiqPayRequest
    {
        public static async Task<string> PostAsync(string url, Dictionary<string, string> list, WebProxy proxy = null)
        {
            var urlParameters = "";

            foreach (var entry in list)
            {
                var queryValue = WebUtility.HtmlEncode(entry.Value);
                byte[] bytes = Encoding.Default.GetBytes(queryValue);
                var utf8QueryValue = Encoding.UTF8.GetString(bytes);

                urlParameters += entry.Key + "=" + utf8QueryValue + "&";
            }

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy
            };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                var encoding = Encoding.GetEncoding(Encoding.UTF8.CodePage);
                using (var responseStream = await httpClient.GetStreamAsync(url + urlParameters))
                using (var reader = new StreamReader(responseStream, encoding))
                    return reader.ReadToEnd();
            }
        }
    }
}