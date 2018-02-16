using System.Runtime.Serialization;

namespace LiqPay.SDK.Dto.Enums
{
    public enum LiqPayRequestActionPayment
    {
        [EnumMember(Value = "pay")]
        Pay,
        [EnumMember(Value = "hold")]
        Hold,
        [EnumMember(Value = "subscribe")]
        Subscribe,
        [EnumMember(Value = "paydonate")]
        Paydonate
    }
}