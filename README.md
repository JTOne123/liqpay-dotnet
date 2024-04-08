liqpay.ua API SDK for .Net/C# [.Net Standard 2.0]
===========================

API Documentation [in Ukrainian](https://www.liqpay.ua/doc) and [in English](https://www.liqpay.ua/en/doc)


Nuget
----------------------

[![NuGet][lp-img]][lp-link] 

[![GitHub Feed][ghf-img]][ghf-link] 


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


<a href="https://www.buymeacoffee.com/pauldatsiuk" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/purple_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>



[lp-img]: https://img.shields.io/badge/nuget-v1.0.1-blue.svg
[ghf-img]: https://img.shields.io/badge/github_feed-v1.0.1-green.svg

[lp-link]: https://www.nuget.org/packages/LiqPay/
[ghf-link]: https://github.com/JTOne123/liqpay-dotnet/packages
