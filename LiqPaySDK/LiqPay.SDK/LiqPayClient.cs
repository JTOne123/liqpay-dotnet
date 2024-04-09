using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LiqPay.SDK
{
    public class LiqPayClient : ILiqPayClient
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly JsonSerializerSettings _jsonSettings;

        public WebProxy Proxy { get; set; }
        public bool IsCnbSandbox { get; set; }
        public bool WithRenderPayButton { get; set; } = true;

        public LiqPayClient(string publicKey, string privateKey)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            CheckRequired();
        }

        public LiqPayClient(string publicKey, string privateKey, WebProxy proxy)
            : this(publicKey, privateKey)
        {
            Proxy = proxy;
        }

        private void CheckRequired()
        {
            if (string.IsNullOrEmpty(_publicKey))
            {
                throw new ArgumentNullException("publicKey is empty");
            }

            if (string.IsNullOrEmpty(_privateKey))
            {
                throw new ArgumentNullException("privateKey is empty");
            }
        }

        public async Task<LiqPayResponse> RequestAsync(string path, LiqPayRequest requestParams)
        {
            var data = PrepareRequestData(requestParams);

			string response = await LiqPayClientHelper
                .PostAsync($"{LiqPayConsts.LiqpayApiUrl}{path}", data, Proxy)
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<LiqPayResponse>(response);
        }

        public Dictionary<string, string> PrepareRequestData(LiqPayRequest requestParams)
        {
            LiqPayRequest withApiParams = WithBasicApiParams(requestParams);
            string jsonString = SerializeToJson(withApiParams);
            string data = jsonString.ToBase64String();

            var apiData = new Dictionary<string, string>();
            apiData.Add("data", data);
            apiData.Add("signature", CreateSignature(data));
            return apiData;
        }

        private string SerializeToJson(LiqPayRequest requestParams)
        {
            var json = JObject.FromObject(requestParams, new JsonSerializer { NullValueHandling = _jsonSettings.NullValueHandling });
            foreach (var key in requestParams.OtherParams.Keys)
            {
                json.Add(key, requestParams.OtherParams[key]);
            }

            var jsonString = json.ToString();
            return jsonString;
        }

        public LiqPayRequest WithBasicApiParams(LiqPayRequest requestParams)
        {
            requestParams.PublicKey = _publicKey;
            requestParams.Version = LiqPayConsts.ApiVersion;
            return requestParams;
        }

        protected LiqPayRequest WithSandboxParam(LiqPayRequest requestParams)
        {
            requestParams.IsSandbox = IsCnbSandbox;

            return requestParams;
        }

        public string CNBForm(LiqPayRequest requestParams)
        {
            CheckCnbParams(requestParams);

            var jsonString = SerializeToJson(WithSandboxParam(WithBasicApiParams(requestParams)));
            var data = jsonString.ToBase64String();
            var signature = CreateSignature(data);
            return RenderHtmlForm(data, requestParams.Language ?? LiqPayConsts.DefaultLanguage, signature);
        }

        private string RenderHtmlForm(string data, LiqPayRequestLanguage language, string signature)
        {
            var form = new StringBuilder();
            form.Append($"<form method=\"post\" action=\"{ LiqPayConsts.LiqpayApiCheckoutUrl }\" accept-charset=\"utf-8\">\n");
            form.Append($"<input type=\"hidden\" name=\"data\" value=\"{ data }\" />\n");
            form.Append($"<input type=\"hidden\" name=\"signature\" value=\"{ signature }\" />\n");

            if (WithRenderPayButton)
            {
                form.Append($"<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1{language.GetAttributeOfType<EnumMemberAttribute>().Value}.radius.png\" name=\"btn_text\" />\n");
            }
            form.Append("</form>\n");

            return form.ToString();
        }

        public void CheckCnbParams(LiqPayRequest requestParams)
        {
            if (requestParams.Amount <= 0)
                throw new NullReferenceException("incorrect amount");

            if (string.IsNullOrEmpty(requestParams.Description))
                throw new NullReferenceException("description can't be null");
        }

        public string StrToSign(string str) => str.SHA1Hash().ToBase64String();

        public string CreateSignature(string base64EncodedData) => StrToSign(_privateKey + base64EncodedData + _privateKey);

        public KeyValuePair<string, string> GenerateDataAndSignature(LiqPayRequest requestParams)
        {
            CheckCnbParams(requestParams);

            var jsonString = SerializeToJson(WithSandboxParam(WithBasicApiParams(requestParams)));
            var data = jsonString.ToBase64String();
            var signature = CreateSignature(data);

            return new KeyValuePair<string, string>(data, signature);
        }
    }
}
