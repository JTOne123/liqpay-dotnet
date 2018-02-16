using System.Runtime.Serialization;

namespace LiqPaySDK.Dto.Enums
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