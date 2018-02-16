using System.Runtime.Serialization;

namespace LiqPaySDK.Dto.Enums
{
    public enum LiqPayRequestAction
    {
        [EnumMember(Value = "invoice_send")]
        InvoiceSend
    }
}