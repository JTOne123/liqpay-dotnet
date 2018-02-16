liqpay.ua API SDK for .Net/C# [.Net Standard 2.0]
===========================

API Documentation [in Russian](https://www.liqpay.ua/documentation/ru) and [in English](https://www.liqpay.ua/documentation/en)


Usage
----------------------

```csharp
// send invoce by email
var invoiceRequest = new LiqPayRequest
{
    Email = "email@example.com",
    Amount = 200,
    Currency = "USD",
    OrderId = "order_id",
    Action = LiqPayRequestAction.InvoiceSend,
    Language = LiqPayRequestLanguage.EN,
    Goods = new List<LiqPayRequestGoods> {
        new LiqPayRequestGoods {
            Amount = 100,
            Count = 2,
            Unit = "pcs.",
            Name = "phone"
        }
    }
};

var liqPayClient = new LiqPayClient("publicApiKey", "priveteApiKEy");
//liqPayClient.IsCnbSandbox = true;
var response = await liqPayClient.RequestAsync("request", invoiceRequest);
```