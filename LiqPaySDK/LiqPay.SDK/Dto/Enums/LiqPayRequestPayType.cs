using System.Runtime.Serialization;

namespace LiqPay.SDK.Dto.Enums
{
    public enum LiqPayRequestPayType
    {
        [EnumMember(Value = "card")]
        Card,
        [EnumMember(Value = "liqpay")]
        LiqPay,
        [EnumMember(Value = "privat24")]
        Privat24,
        [EnumMember(Value = "masterpass")]
        Masterpass,
        [EnumMember(Value = "moment_part")]
        MomentPart,
        [EnumMember(Value = "cash")]
        Cash,
        [EnumMember(Value = "invoice")]
        Invoice,
        [EnumMember(Value = "qr")]
        QR
    }
}