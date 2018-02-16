using System.Runtime.Serialization;

namespace LiqPay.SDK.Dto.Enums
{
    public enum LiqPayRequestAction
    {
        [EnumMember(Value = "invoice_send")]
        InvoiceSend
    }
}