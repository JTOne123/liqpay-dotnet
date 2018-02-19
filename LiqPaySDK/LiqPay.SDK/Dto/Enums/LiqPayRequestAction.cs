using System.Runtime.Serialization;

namespace LiqPay.SDK.Dto.Enums
{
    public enum LiqPayRequestAction
    {
        [EnumMember(Value = "invoice_send")]
        InvoiceSend,
        [EnumMember(Value = "invoice_cancel")]
        InvoiceCancel,
        [EnumMember(Value = "pay")]
        Pay,
        [EnumMember(Value = "payqr")]
        PayQR,
        [EnumMember(Value = "paytoken")]
        PayToken,
        [EnumMember(Value = "paycash")]
        PayCash,
        [EnumMember(Value = "paytrack")]
        PayTrack,
        [EnumMember(Value = "refund")]
        Refund
    }
}