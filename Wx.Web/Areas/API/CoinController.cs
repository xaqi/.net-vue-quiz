using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using Wx.BLL;
using Wx.Common;
using System.Web.Http;

namespace Wx.Web.Areas.API
{
    public class CoinController : BaseApiController
    {
        public object GetRedeemInfo()
        {
            var data = base.GetTodaySummaryAndUserBalance();
            var a = 1;
            data["redeemConfig"] = new object[] { new { rmb = 20, golds = 2400 }, new { rmb = 50, golds = 5700 }, new { rmb = 100, golds = 11000 } };
            return new ReturnMessage { success = true, data = data };
        }



        [HttpGet]
        public object InitRedeemFormData()
        {
            var data = base.GetTodaySummaryAndUserBalance();
            data["submit"] = new { alipay = "", phone = "" };
            return new ReturnMessage { success = true, data = data };
        }
        public object RedeemSubmit(RedeemSubmitParam param)
        {
            var bll = new CoinBLL();
            var rm = bll.RedeemSubmit(CurrentUser.userId, param.golds, param.alipay, param.phone, param.detail);
            UpdateCurrentUser();
            return rm;
        }


        public object GetPayInfo()
        {
            var data = base.GetTodaySummaryAndUserBalance();
            data["payConfig"] = new
            {
                row1 = new object[]{
                new { rmb = 10, golds = 1050 },
                new { rmb = 20, golds = 2200 }
                , new { rmb = 50, golds = 5500 }
                },

                row2 = new object[]{
                 new { rmb = 100, golds = 11000 }
                , new { rmb = 200, golds = 22000 }
                }
            };
            return new ReturnMessage { success = true, data = data };
        }
        private static TenPayV3Info _tenPayV3Info;
        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }


        public object GetWeixinPayOption()
        {
            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";

            string sp_billno = DateTime.Now.ToString("yyyyMMddHHmmss") + TenPayV3Util.BuildRandomStr(28);


            RequestHandler packageReqHandler = new RequestHandler(null);
            packageReqHandler.Init();

            timeStamp = TenPayV3Util.GetTimestamp();
            nonceStr = TenPayV3Util.GetNoncestr();


            var dict_package = new Dictionary<string, string> {
                {"appid", TenPayV3Info.AppId},
                {"mch_id", TenPayV3Info.MchId},
                {"nonce_str", nonceStr},
                {"body", "充值"},
                {"out_trade_no", sp_billno},
                {"total_fee", (0.01 * 100).ToString()},
                {"spbill_create_ip", System.Web.HttpContext.Current.Request.UserHostAddress},
                {"notify_url", TenPayV3Info.TenPayV3Notify},
                {"trade_type", "JSAPI"},
                {"openid", CurrentUser.openId}
            };
            dict_package.Keys.ToList().ForEach(key => packageReqHandler.SetParameter(key, dict_package[key]));

            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);

            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.Unifiedorder(data);
            var res = XDocument.Parse(result);
            string prepayId = string.Empty;
            try
            {
                prepayId = res.Element("xml").Element("prepay_id").Value;
            }
            catch (Exception ex)
            {
                return new ReturnMessage { success = false, message = ex.Message, data = new { xml = res.ToString() } };
            }

            var cbll = new CoinBLL();
            cbll.SaveJsapiPackageRequest(prepayId, CurrentUser.userId, dict_package);

            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("appId", TenPayV3Info.AppId);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
            paySignReqHandler.SetParameter("signType", "MD5");
            paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

            var ViewData = new Dictionary<string, object> { };
            ViewData["appId"] = TenPayV3Info.AppId;
            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["package"] = string.Format("prepay_id={0}", prepayId);
            ViewData["paySign"] = paySign;
            ViewData["signType"] = "MD5";
            return ViewData;
        }
        public object PaySubmit(PaySubmitParam param)
        {
            var bll = new CoinBLL();
            var rm = bll.PaySubmit(CurrentUser.userId, param.rmb);
            UpdateCurrentUser();
            return rm;
        }
    }

    public class PaySubmitParam
    {
        public double rmb { get; set; }
    }
    public class RedeemSubmitParam
    {
        public int golds { get; set; }
        public string alipay { get; set; }
        public string detail { get; set; }
        public string phone { get; set; }
    }
}
