using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LiqPaySDK
{
    public class LiqPay : ILiqPay
    {
        private readonly string _publicKey;
        private readonly string _privateKey;

        public WebProxy Proxy { get; set; }
        public bool IsCnbSandbox { get; set; }
        public bool WithRenderPayButton { get; set; } = true;

        public LiqPay(string publicKey, string privateKey)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;

            CheckRequired();
        }

        public LiqPay(String publicKey, String privateKey, WebProxy proxy)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;

            Proxy = proxy;

            CheckRequired();
        }

        private void CheckRequired()
        {
            if (string.IsNullOrEmpty(_publicKey))
                throw new ArgumentNullException("publicKey is empty");

            if (string.IsNullOrEmpty(_publicKey))
                throw new ArgumentNullException("privateKey is empty");
        }

        public async Task<Dictionary<string, object>> RequestAsync(string path, Dictionary<string, string> queryParams)
        {
            var data = GenerateData(queryParams);
            string response = await LiqPayRequest.PostAsync(LiqPayConsts.LiqpayApiUrl + path, data, Proxy);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
        }

        public Dictionary<String, String> GenerateData(Dictionary<String, String> queryParams)
        {
            var apiData = new Dictionary<String, String>();
            var jsonString = JsonConvert.SerializeObject(WithBasicApiParams(queryParams));
            string data = jsonString.ToBase64String();
            apiData.Add("data", data);
            apiData.Add("signature", CreateSignature(data));
            return apiData;
        }

        public Dictionary<string, string> WithBasicApiParams(Dictionary<string, string> queryParams)
        {
            queryParams.Add("public_key", _publicKey);
            queryParams.Add("version", LiqPayConsts.ApiVersion);
            return queryParams;
        }

        protected Dictionary<string, string> WithSandboxParam(Dictionary<string, string> queryParams)
        {
            if (queryParams.ContainsKey("sandbox") && IsCnbSandbox)
            {
                queryParams.Add("sandbox", "1");
            }

            return queryParams;
        }

        public string CNBForm(Dictionary<string, string> queryParams)
        {
            CheckCnbParams(queryParams);

            var jsonString = JsonConvert.SerializeObject(WithSandboxParam(WithBasicApiParams(queryParams)));
            var data = jsonString.ToBase64String();
            var signature = CreateSignature(data);
            var language = queryParams.ContainsKey("language") ? queryParams["language"] : LiqPayConsts.DefaultLanguage;
            return RenderHtmlForm(data, language, signature);
        }

        private string RenderHtmlForm(string data, string language, string signature)
        {
            String form = "";
            form += "<form method=\"post\" action=\"" + LiqPayConsts.LiqpayApiCheckoutUrl + "\" accept-charset=\"utf-8\">\n";
            form += "<input type=\"hidden\" name=\"data\" value=\"" + data + "\" />\n";
            form += "<input type=\"hidden\" name=\"signature\" value=\"" + signature + "\" />\n";
            if (this.WithRenderPayButton)
            {
                form += "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1" + language + ".radius.png\" name=\"btn_text\" />\n";
            }
            form += "</form>\n";
            return form;
        }

        public void CheckCnbParams(Dictionary<string, string> queryParams)
        {
            if (!queryParams.ContainsKey("amount"))
                throw new NullReferenceException("amount can't be null");

            if (!queryParams.ContainsKey("currency"))
                throw new NullReferenceException("currency can't be null");

            if (!queryParams.ContainsKey("description"))
                throw new NullReferenceException("description can't be null");
        }

        public string StrToSign(string str) => str.SHA1Hash().ToBase64String();

        public string CreateSignature(string base64EncodedData) => StrToSign(_privateKey + base64EncodedData + _privateKey);
    }
}