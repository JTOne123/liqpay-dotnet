using LiqPaySDK.Dto.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace LiqPaySDK.Dto
{
    public class LiqPayRequest
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiqPayRequestAction? Action { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("sandbox")]
        public string Sandbox { get; set; }
        [JsonIgnore]
        public bool IsSandbox
        {
            get
            {
                return Sandbox == "1";
            }
            set
            {
                Sandbox = value ? "1" : null;
            }
        }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("action_payment")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiqPayRequestActionPayment? ActionPayment { get; set; }
        [JsonProperty("expired_date")]
        public DateTime? ExpiredDate { get; set; }
        [JsonProperty("goods")]
        public List<LiqPayRequestGoods> Goods { get; set; }
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiqPayRequestLanguage? Language { get; set; }
        [JsonProperty("result_url")]
        public string ResultUrl { get; set; }
        [JsonProperty("server_url")]
        public string ServerUrl { get; set; }
    }
}