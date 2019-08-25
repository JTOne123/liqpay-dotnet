﻿using LiqPay.SDK.Dto.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LiqPay.SDK.Dto
{
    public class LiqPayResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiqPayResponseAction Action { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("payment_id")]
        public long PaymentId { get; set; }
        [JsonProperty("transaction_id")]
        public long TransactionId { get; set; }
        [JsonProperty("receiver_type")]
        public string ReceiverType { get; set; }
        [JsonProperty("receiver_value")]
        public string ReceiverValue { get; set; }
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LiqPayResponseStatus Status { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("err_code")]
        public string ErrorCode { get; set; }
        [JsonProperty("err_description")]
        public string ErrorDescription { get; set; }
        [JsonProperty("invoice_id")]
        public int? InvoiceId { get; set; }
    }
}