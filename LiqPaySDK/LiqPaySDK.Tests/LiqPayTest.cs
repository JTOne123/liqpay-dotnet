using LiqPaySDK.Dto;
using LiqPaySDK.Dto.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiqPaySDK.Tests
{
    [TestClass]
    public class LiqPayTest
    {
        static readonly string CNB_FORM_WITHOUT_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"eyJ2ZXJzaW9uIjozLCJwdWJsaWNfa2V5IjoicHVibGljS2V5IiwiYW1vdW50IjoxLjUsImN1cnJlbmN5IjoiVVNEIiwiZGVzY3JpcHRpb24iOiJEZXNjcmlwdGlvbiIsImxhbmd1YWdlIjoiZW4ifQ==\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"gCMDMPVUIu6f2aH7T1OIqzwb7BM=\" />\n" +
            "<input type=\"image\" src=\"//static.liqpay.ua/buttons/p1en.radius.png\" name=\"btn_text\" />\n" +
            "</form>\n";

        static readonly string CNB_FORM_WITH_SANDBOX = "<form method=\"post\" action=\"https://www.liqpay.ua/api/3/checkout\" accept-charset=\"utf-8\">\n" +
            "<input type=\"hidden\" name=\"data\" value=\"eyJ2ZXJzaW9uIjozLCJwdWJsaWNfa2V5IjoicHVibGljS2V5IiwiYW1vdW50IjoxLjUsImN1cnJlbmN5IjoiVVNEIiwiZGVzY3JpcHRpb24iOiJEZXNjcmlwdGlvbiIsInNhbmRib3giOiIxIiwibGFuZ3VhZ2UiOiJlbiJ9\" />\n" +
            "<input type=\"hidden\" name=\"signature\" value=\"aA2nLvqUhbXQ7a0W3WQj0bdmcSc=\" />\n" +
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
        public void LiqPayTest_CnbParams()
        {
            var cnbParams = CreateDefaultTestRequest();
            lp.CheckCnbParams(cnbParams);
            Assert.AreEqual("en", cnbParams.Language.Value.GetAttributeOfType<EnumMemberAttribute>().Value);
            Assert.AreEqual("USD", cnbParams.Currency);
            Assert.AreEqual("1.5", cnbParams.Amount.ToString());
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
            Assert.AreEqual("1.5", fullParams.Amount.ToString());
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
            Assert.AreEqual("ep8wax2+ELYPDoW8U9Vg3hG8IYY=", generated["signature"]);
            Assert.AreEqual("eyJ2ZXJzaW9uIjozLCJwdWJsaWNfa2V5IjoicHVibGljS2V5IiwiYW1vdW50IjoyMDAuMCwiY3VycmVuY3kiOiJVU0QiLCJvcmRlcl9pZCI6Im9yZGVyX2lkXzEiLCJlbWFpbCI6ImNsaWVudC1lbWFpbEBnbWFpbC5jb20iLCJnb29kcyI6W3siYW1vdW50IjoxMDAuMCwiY291bnQiOjIsInVuaXQiOiJ1bi4iLCJuYW1lIjoicGhvbmUifV19", generated["data"]);
        }
    }
}