using LiqPay.SDK.Dto;
using LiqPay.SDK.Dto.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace LiqPay.SDK.Tests
{
    [TestClass]
    public class LiqPayTest
    {
        static readonly string CNB_FORM_WITHOUT_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"ew0KICAidmVyc2lvbiI6IDMsDQogICJwdWJsaWNfa2V5IjogInB1YmxpY0tleSIsDQogICJhbW91bnQiOiAxLjUsDQogICJjdXJyZW5jeSI6ICJVU0QiLA0KICAiZGVzY3JpcHRpb24iOiAiRGVzY3JpcHRpb24iLA0KICAibGFuZ3VhZ2UiOiAiZW4iDQp9\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"iKN9Hh4HWJGyCMefiU0wDPGavYg=\" />\n" +
            "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1en.radius.png\" name=\"btn_text\" />\n" +
            "</form>\n";

        static readonly string CNB_FORM_WITH_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"ew0KICAidmVyc2lvbiI6IDMsDQogICJwdWJsaWNfa2V5IjogInB1YmxpY0tleSIsDQogICJhbW91bnQiOiAxLjUsDQogICJjdXJyZW5jeSI6ICJVU0QiLA0KICAiZGVzY3JpcHRpb24iOiAiRGVzY3JpcHRpb24iLA0KICAic2FuZGJveCI6ICIxIiwNCiAgImxhbmd1YWdlIjogImVuIg0KfQ==\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"hZyFKilGTSAvRczVgGPejFkBKbg=\" />\n" +
            "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1en.radius.png\" name=\"btn_text\" />\n" +
            "</form>\n";

        LiqPayClient lp;

        [TestInitialize]
        public void LiqPayTestInit()
        {
            lp = new LiqPayClient("publicKey", "privateKey");
        }

        private LiqPayRequest CreateDefaultTestRequest()
        {
            return new LiqPayRequest
            {
                Language = LiqPayRequestLanguage.EN,
                Amount = 1.5,
                Currency = LiqPayCurrency.USD.GetAttributeOfType<EnumMemberAttribute>().Value,
                Description = "Description",
                IsSandbox = true
            };
        }

        [TestMethod]
        public void LiqPayTest_CnbFormWithoutSandboxParam()
        {
            var queryParams = CreateDefaultTestRequest();
            queryParams.IsSandbox = false;
            Assert.AreEqual(CNB_FORM_WITHOUT_SANDBOX, lp.CNBForm(queryParams));
        }

        [TestMethod]
        public void LiqPayTest_CnbFormWillSetSandboxParamIfItEnabledGlobally()
        {
            var queryParams = CreateDefaultTestRequest();
            lp.IsCnbSandbox = true;
            Assert.AreEqual(CNB_FORM_WITH_SANDBOX, lp.CNBForm(queryParams));
        }

        [TestMethod]
        public void LiqPayTest_OtherParamsSerializedToJsonObjectDirectly()
        {
            var queryParams = CreateDefaultTestRequest();
            queryParams.OtherParams["test"] = "value";
            var requestData = lp.PrepareRequestData(queryParams);
            var json = JObject.Parse(requestData["data"].DecodeBase64());
            Assert.IsNotNull(json.GetValue("test"));
            Assert.AreEqual(json.GetValue("test").Value<string>(), "value");
        }

        [TestMethod]
        public void LiqPayTest_CnbParams()
        {
            var cnbParams = CreateDefaultTestRequest();
            lp.CheckCnbParams(cnbParams);
            Assert.AreEqual("en", cnbParams.Language.Value.GetAttributeOfType<EnumMemberAttribute>().Value);
            Assert.AreEqual("USD", cnbParams.Currency);
            Assert.AreEqual("1.5", cnbParams.Amount.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("Description", cnbParams.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LiqPayTest_CnbParamsThrowsNpeIfNotAmount()
        {
            var queryParams = CreateDefaultTestRequest();
            queryParams.Amount = 0;
            lp.CheckCnbParams(queryParams);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LiqPayTest_CnbParamsThrowsNpeIfNotDescription()
        {
            var queryParams = CreateDefaultTestRequest();
            queryParams.Description = null;
            lp.CheckCnbParams(queryParams);
        }

        [TestMethod]
        public void LiqPayTest_WithBasicApiParams()
        {
            var cnbParams = CreateDefaultTestRequest();
            var fullParams = lp.WithBasicApiParams(cnbParams);
            Assert.AreEqual("publicKey", fullParams.PublicKey);
            Assert.AreEqual("3", fullParams.Version.ToString());
            Assert.AreEqual("1.5", fullParams.Amount.ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void LiqPayTest_StrToSign()
        {
            Assert.AreEqual("i0XkvRxqy4i+v2QH0WIF9WfmKj4=", lp.StrToSign("some string"));
        }

        [TestMethod]
        public void LiqPayTest_CreateSignature()
        {
            var jsonObj = JsonConvert.DeserializeObject("{\"field\": \"value\"}");
            var jsonString = JsonConvert.SerializeObject(jsonObj);

            var base64EncodedData = jsonString.ToBase64String();
            Assert.AreEqual("d3dP/5qWQFlZgFR53eAwqJ+xIOQ=", lp.CreateSignature(base64EncodedData));
        }

        [TestMethod]
        public void LiqPayTest_GenerateData()
        {
            var invoiceParams = new LiqPayRequest
            {
                Email = "client-email@gmail.com",
                Amount = 200,
                Currency = LiqPayCurrency.USD.GetAttributeOfType<EnumMemberAttribute>().Value,
                OrderId = "order_id_1",
                Goods = new List<LiqPayRequestGoods> {
                    new LiqPayRequestGoods {
                        Amount = 100,
                        Count = 2,
                        Unit = "un.",
                        Name = "phone"
                    }
                }
            };

            var generated = lp.PrepareRequestData(invoiceParams);
            Assert.AreEqual("hrXopoo2eyDAtknnZn3Ez4VEka0=", generated["signature"]);
            Assert.AreEqual("ew0KICAidmVyc2lvbiI6IDMsDQogICJwdWJsaWNfa2V5IjogInB1YmxpY0tleSIsDQogICJhbW91bnQiOiAyMDAuMCwNCiAgImN1cnJlbmN5IjogIlVTRCIsDQogICJvcmRlcl9pZCI6ICJvcmRlcl9pZF8xIiwNCiAgImVtYWlsIjogImNsaWVudC1lbWFpbEBnbWFpbC5jb20iLA0KICAiZ29vZHMiOiBbDQogICAgew0KICAgICAgImFtb3VudCI6IDEwMC4wLA0KICAgICAgImNvdW50IjogMiwNCiAgICAgICJ1bml0IjogInVuLiIsDQogICAgICAibmFtZSI6ICJwaG9uZSINCiAgICB9DQogIF0NCn0=", generated["data"]);
        }
    }
}