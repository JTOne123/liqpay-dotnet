using System.Runtime.Serialization;

namespace LiqPay.SDK.Dto.Enums
{
    public enum LiqPayResponseStatus
    {
        [EnumMember(Value = "error")]
        Error,
        [EnumMember(Value = "failure")]
        Failure,
        [EnumMember(Value = "sandbox")]
        Sandbox,
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "subscribed")]
        Subscribed,
        [EnumMember(Value = "unsubscribed")]
        Unsubscribed,
        [EnumMember(Value = "invoice_wait")]
        InvoiceWait,
        [EnumMember(Value = "try_again")]
        TryAgain,
        [EnumMember(Value = "reversed")]
        Reversed,
        [EnumMember(Value = "cash_wait")]
        CashWait,
        [EnumMember(Value = "3ds_verify")]
        _3DSVerify,
        [EnumMember(Value = "captcha_verify")]
        CaptchaVerify,
        [EnumMember(Value = "cvv_verify")]
        CVVVerify,
        [EnumMember(Value= "ivr_verify")]
        IVRVerify,
        [EnumMember(Value = "otp_verify")]
        OTPVerify,
        [EnumMember(Value = "password_verify")]
        PasswordVerify,
        [EnumMember(Value = "phone_verify")]
        PhoneVerify,
        [EnumMember(Value = "pin_verify")]
        PinVerify,
        [EnumMember(Value = "receiver_verify")]
        ReceiverVerify,
        [EnumMember(Value = "sender_verify")]
        SenderVerify,
        [EnumMember(Value = "wait_qr")]
        WaitQR,
        [EnumMember(Value = "wait_sender")]
        WaitSender,
        [EnumMember(Value = "p24_verify")]
        P24Verify,
        [EnumMember(Value = "mp_verify")]
        MPVerify,
        [EnumMember(Value = "hold_wait")]
        HoldWait,
        [EnumMember(Value = "prepared")]
        Prepared,
        [EnumMember(Value = "processing")]
        Processing,
        [EnumMember(Value = "wait_accept")]
        WaitAccept,
        [EnumMember(Value = "wait_card")]
        WaitCard,
        [EnumMember(Value = "wait_compensation")]
        WaitCompensation,
        [EnumMember(Value = "wait_lc")]
        WaitLC,
        [EnumMember(Value = "wait_reserve")]
        WaitReserve,
        [EnumMember(Value = "wait_secure")]
        WaitSecure


    }
}