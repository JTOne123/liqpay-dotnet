using LiqPaySDK.Dto;
using System.Threading.Tasks;

namespace LiqPaySDK
{
    public interface ILiqPayClient
    {
        Task<LiqPayResponse> RequestAsync(string path, LiqPayRequest requestParams);

        /**
         * Liq and Buy
         * Payment acceptance on the site client to server
         * To accept payments on your site you will need:
         * Register on www.liqpay.ua
         * Create a store in your account using install master
         * Get a ready HTML-button or create a simple HTML form
         * HTML form should be sent by POST to URL https://www.liqpay.ua/api/3/checkout Two parameters data and signature, where:
         * data - function result base64_encode( $json_string )
         * signature - function result base64_encode( sha1( $private_key . $data . $private_key ) )
         */
        string CNBForm(LiqPayRequest requestParams);
    }
}