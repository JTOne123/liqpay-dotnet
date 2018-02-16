using Newtonsoft.Json;

namespace LiqPay.SDK.Dto
{
    public class LiqPayRequestGoods
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}