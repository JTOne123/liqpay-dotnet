using LiqPay.SDK.Dto.Enums;

namespace LiqPay.SDK
{
    public static class LiqPayConsts
    {
        public const int ApiVersion = 3;
        public const string LiqpayApiUrl = "https://www.liqpay.ua/api/";
        public const string LiqpayApiCheckoutUrl = "https://www.liqpay.ua/api/3/checkout";
        public const LiqPayRequestLanguage DefaultLanguage = LiqPayRequestLanguage.RU;
    }
}