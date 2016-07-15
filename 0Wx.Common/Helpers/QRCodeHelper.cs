using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing.Imaging;
using System.Net;
using System.Web.Script.Serialization;

namespace Wx.Common.Helpers
{
    public class QRCodeHelper
    {
        public static bool GetQRCode(string strContent, MemoryStream ms)
        {
            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M;
            string Content = strContent;
            var qz = QuietZoneModules.Four;
            QuietZoneModules QuietZones = QuietZoneModules.Two;
            int ModuleSize = 12;
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (encoder.TryEncode(Content, out qr))
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
            }
            else
            {
                return false;
            }
            return true;
        }


        static string access_token = null;
        static DateTime expire_time = DateTime.MinValue;
        public static QrTicket QrCode(string appId, string secret, string scene_str)
        {
            var wc = new WebClient();
            var Jser = new JavaScriptSerializer();
            if (expire_time < DateTime.Now)
            {
                var tokenJson = wc.DownloadString($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={secret}");
                var dictToken = Jser.Deserialize<Dictionary<string, object>>(tokenJson);
                access_token = dictToken["access_token"].ToString();
                expire_time = DateTime.Now.AddSeconds(int.Parse(dictToken["expires_in"].ToString()) / 2);
            }
            var ticketRequestData = Jser.Deserialize<Dictionary<string, object>>(@"{""action_name"": ""QR_LIMIT_STR_SCENE"", ""action_info"": {""scene"": {""scene_str"": ""###""}}}".Replace("###", scene_str));
            var qrJson = PostJson($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={access_token}", ticketRequestData);
            var qrTicket = Jser.Deserialize<QrTicket>(PostJson($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={access_token}", ticketRequestData));
            return qrTicket;
            //var imgUrl = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={dictTicket["ticket"]}";
            //return imgUrl;
        }

        public static string PostJson(string url, object data = null)
        {
            var wc = new System.Net.WebClient();
            var jser = new JavaScriptSerializer();
            byte[] jsonData = Encoding.UTF8.GetBytes(jser.Serialize(data));
            wc.Headers.Add("Accept", "application/json");
            wc.Headers.Add("Content-Type", "application/json");
            return Encoding.UTF8.GetString(wc.UploadData(url, "POST", jsonData));
        }
    }

    public class QrTicket
    {
        public string ticket { get; set; }
        public string url { get; set; }
    }
}
