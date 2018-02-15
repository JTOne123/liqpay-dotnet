using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiqPaySDK.Tests
{
    [TestClass]
    public class LiqPayTest
    {
        static readonly string CNB_FORM_WITHOUT_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"eyJhbW91bnQiOiIxLjUiLCJjdXJyZW5jeSI6IlVTRCIsImRlc2NyaXB0aW9uIjoiRGVzY3JpcHRpb24iLCJsYW5ndWFnZSI6ImVuIiwicHVibGljX2tleSI6InB1YmxpY0tleSIsInZlcnNpb24iOiIzIn0=\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"krCwuK4CBtNFAb6zqmJCeR/85VU=\" />\n" +
            "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1en.radius.png\" name=\"btn_text\" />\n" +
            "</form>\n";

        static readonly string CNB_FORM_WITH_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"eyJhbW91bnQiOiIxLjUiLCJjdXJyZW5jeSI6IlVTRCIsImRlc2NyaXB0aW9uIjoiRGVzY3JpcHRpb24iLCJsYW5ndWFnZSI6ImVuIiwicHVibGljX2tleSI6InB1YmxpY0tleSIsInNhbmRib3giOiIxIiwidmVyc2lvbiI6IjMifQ==\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"jDmdwKnagO2JhE1ONHdk3F7FG0c=\" />\n" +
            "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1en.radius.png\" name=\"btn_text\" />\n" +
            "</form>\n";

        LiqPay lp;

        [TestInitialize]
        public void LiqPayTestInit()
        {
            lp = new LiqPay("publicKey", "privateKey");
        }

        private Dictionary<string, string> DefaultTestParams(string removedKey)
        {
            var queryParams = new Dictionary<string, string>();
            queryParams.Add("language", "en");
            queryParams.Add("amount", "1.5");
            queryParams.Add("currency", "USD");
            queryParams.Add("description", "Description");
            queryParams.Add("sandbox", "1");

            if (removedKey != null)
                queryParams.Remove(removedKey);

            return queryParams;
        }

        [TestMethod]
        public void LiqPayTest_CnbFormWithoutSandboxParam()
        {
            var queryParams = DefaultTestParams("sandbox");
            Assert.AreEqual(CNB_FORM_WITHOUT_SANDBOX, lp.CNBForm(queryParams));
        }

        [TestMethod]
        public void LiqPayTest_CnbFormWithSandboxParam()
        {
            var queryParams = DefaultTestParams(null);
            Assert.AreEqual(CNB_FORM_WITH_SANDBOX, lp.CNBForm(queryParams));
        }

        [TestMethod]
        public void LiqPayTest_CnbFormWillSetSandboxParamIfItEnabledGlobally()
        {
            var queryParams = DefaultTestParams("sandbox");
            lp.IsCnbSandbox = true;
            Assert.AreEqual(CNB_FORM_WITH_SANDBOX, lp.CNBForm(queryParams));
        }

        [TestMethod]
        public void LiqPayTest_CnbParams()
        {
            var cnbParams = DefaultTestParams(null);
            lp.CheckCnbParams(cnbParams);
            Assert.AreEqual("en", cnbParams["language"]);
            Assert.AreEqual("USD", cnbParams["currency"]);
            Assert.AreEqual("1.5", cnbParams["amount"]);
            Assert.AreEqual("Description", cnbParams["description"]);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LiqPayTest_CnbParamsThrowsNpeIfNotAmount()
        {
            var queryParams = DefaultTestParams("amount");
            lp.CheckCnbParams(queryParams);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LiqPayTest_CnbParamsThrowsNpeIfNotCurrency()
        {
            var queryParams = DefaultTestParams("currency");
            lp.CheckCnbParams(queryParams);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LiqPayTest_CnbParamsThrowsNpeIfNotDescription()
        {
            var queryParams = DefaultTestParams("description");
            lp.CheckCnbParams(queryParams);
        }

        [TestMethod]
        public void LiqPayTest_WithBasicApiParams()
        {
            var cnbParams = DefaultTestParams(null);
            var fullParams = lp.WithBasicApiParams(cnbParams);
            Assert.AreEqual("publicKey", fullParams["public_key"]);
            Assert.AreEqual("3", fullParams["version"]);
            Assert.AreEqual("1.5", fullParams["amount"]);
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
            var invoiceParams = new Dictionary<string, string>();
            invoiceParams.Add("email", "client-email@gmail.com");
            invoiceParams.Add("amount", "200");
            invoiceParams.Add("currency", "USD");
            invoiceParams.Add("order_id", "order_id_1");
            invoiceParams.Add("goods", "[{amount:100, count: 2, unit: 'un.', name: 'phone'}]");
            var generated = lp.GenerateData(invoiceParams);
            Assert.AreEqual("DqcGjvo2aXgt0+zBZECdH4cbPWY=", generated["signature"]);
            Assert.AreEqual("eyJhbW91bnQiOiIyMDAiLCJjdXJyZW5jeSI6IlVTRCIsImVtYWlsIjoiY2xpZW50LWVtYWlsQGdtYWlsLmNvbSIsImdvb2RzIjoiW3thbW91bnQ6IDEwMCwgY291bnQ6IDIsIHVuaXQ6ICd1bi4nLCBuYW1lOiAncGhvbmUnfV0iLCJvcmRlcl9pZCI6Im9yZGVyX2lkXzEiLCJwdWJsaWNfa2V5IjoicHVibGljS2V5IiwidmVyc2lvbiI6IjMifQ==", generated["data"]);
        }
    }
}