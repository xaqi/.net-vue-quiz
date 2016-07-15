using System.Web.Mvc;
using System.Text;
using Senparc.Weixin.MP.TenPayLibV3;
using System.Xml;
using System.Xml.Serialization;
using Wx.DAL;
using Wx.Common;
using Wx.BLL;


namespace Wx.Web.Controllers
{
    public class CoinController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult PayNotifyUrl()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;

            resHandler.SetKey(TenPayV3Info.Key);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                res = "success";

                //正确的订单处理
            }
            else
            {
                res = "wrong";

                //错误的订单处理
            }

            var resXml = resHandler.ParseXML();
            var pay_jsapi_notify = Senparc.Weixin.XmlUtility.XmlUtility.Deserialize<pay_jsapi_notify>(resXml);
            var cbll = new CoinBLL();
            cbll.Resolve(pay_jsapi_notify.AsDictionary());


            var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/1.txt"));
            fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));
            fileStream.Close();

            //return Content("SUCCESS");
            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", "SUCCESS", "OK");

            return Content(xml, "text/xml");
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

        [AllowAnonymous]
        public ActionResult XX()
        {
            var xml = "<xml><result_code><![CDATA[SUCCESS]]></result_code><fee_type><![CDATA[CNY]]></fee_type><return_code><![CDATA[SUCCESS]]></return_code><total_fee>1</total_fee><mch_id><![CDATA[133290344343]]></mch_id><cash_fee>1</cash_fee><openid><![CDATA[oLA2zt9bnFr1ZmyebgLaBOY4Cpok]]></openid><transaction_id><![CDATA[4008012001201605125783424816]]></transaction_id><sign><![CDATA[2BFCCB8AC1B7F2B81297A143627C5D84]]></sign><bank_type><![CDATA[CMB_DEBIT]]></bank_type><appid><![CDATA[fsfsafsfdsafsafdsafsafsafsa]]></appid><time_end><![CDATA[20160512212749]]></time_end><trade_type><![CDATA[JSAPI]]></trade_type><nonce_str><![CDATA[9DCB88E0137649590B755372B040AFAD]]></nonce_str><is_subscribe><![CDATA[Y]]></is_subscribe><out_trade_no><![CDATA[212730404503329]]></out_trade_no></xml>";
            var obj = Senparc.Weixin.XmlUtility.XmlUtility.Deserialize<pay_jsapi_notify>(xml);


            var resXml = xml;
            var pay_jsapi_notify = Senparc.Weixin.XmlUtility.XmlUtility.Deserialize<pay_jsapi_notify>(resXml);
            var cbll = new CoinBLL();
            cbll.Resolve(pay_jsapi_notify.AsDictionary());

            var jser = new System.Web.Script.Serialization.JavaScriptSerializer();
            return Content(jser.Serialize(obj));
        }
    }


}