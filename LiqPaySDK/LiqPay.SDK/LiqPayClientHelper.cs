using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiqPay.SDK
{
    public class LiqPayClientHelper
    {
        public static async Task<string> PostAsync(string url, Dictionary<string, string> data, WebProxy proxy = null)
        {
            var parameters = new List<string>();
            foreach (var item in data)
            {
                var queryValue = WebUtility.HtmlEncode(item.Value);
                byte[] bytes = Encoding.Default.GetBytes(queryValue);
                var utf8QueryValue = Encoding.UTF8.GetString(bytes);
                
                parameters.Add($"{item.Key}={utf8QueryValue}");
            }

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy
            };

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                var encoding = Encoding.GetEncoding(Encoding.UTF8.CodePage);

				var urlParameters = string.Join("&", parameters);
                var stringContent = new StringContent(urlParameters);

				using (var responseMessage = await httpClient.PostAsync(url, stringContent).ConfigureAwait(false))
                {
                    responseMessage.EnsureSuccessStatusCode();

                    using (var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var reader = new StreamReader(responseStream, encoding))
                    {
						return reader.ReadToEnd();
					}
                }
            }
        }
    }
}
