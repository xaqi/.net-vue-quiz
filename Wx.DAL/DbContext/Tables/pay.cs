using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using Wx.Enums;
using System.Xml;
using System.Xml.Serialization;

namespace Wx.DAL
{
    public class pay
    {
        public int payId { get; set; }
        public double rmb { get; set; }
        public double golds { get; set; }
        public DateTime payTime { get; set; }
        public string tip { get; set; }

        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }

        public int? pay_jsapi_package_requestId { get; set; }
        public virtual pay_jsapi_package_request pay_jsapi_package_request { get; set; }
        public int? pay_jsapi_notifyId { get; set; }
        public virtual pay_jsapi_notify pay_jsapi_notify { get; set; }


    }


    public class redeem
    {
        public int redeemId { get; set; }
        public double golds { get; set; }
        public double rmb { get; set; }
        public DateTime createTime { get; set; }
        public string detail { get; set; }
        public string alipay { get; set; }
        public string phone { get; set; }
        public RedeemStatus redeemStatus { get; set; }


        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }


    }

    public class pay_jsapi_package_request
    {
        public int pay_jsapi_package_requestId { get; set; }
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string body { get; set; }
        public string out_trade_no { get; set; }
        public int total_fee { get; set; }
        public string spbill_create_ip { get; set; }
        public string notify_url { get; set; }
        public string trade_type { get; set; }


        public string prepayId { get; set; }
        public string openid { get; set; }


        public DateTime createTime { get; set; }
        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }

        public int? pay_jsapi_notifyId { get; set; }
        public virtual pay_jsapi_notify pay_jsapi_notify { get; set; }

    }


    [XmlRoot("xml")]
    public class pay_jsapi_notify
    {
        public int pay_jsapi_notifyId { get; set; }
        [XmlElement("result_code")]
        public string result_code { get; set; }
        [XmlElement("fee_type")]
        public string fee_type { get; set; }
        [XmlElement("return_code")]
        public string return_code { get; set; }
        [XmlElement("total_fee")]
        public string total_fee { get; set; }
        [XmlElement("mch_id")]
        public string mch_id { get; set; }
        [XmlElement("cash_fee")]
        public string cash_fee { get; set; }
        [XmlElement("openid")]
        public string openid { get; set; }
        [XmlElement("transaction_id")]
        public string transaction_id { get; set; }
        [XmlElement("sign")]
        public string sign { get; set; }
        [XmlElement("bank_type")]
        public string bank_type { get; set; }
        [XmlElement("appid")]
        public string appid { get; set; }
        [XmlElement("time_end")]
        public string time_end { get; set; }
        [XmlElement("trade_type")]
        public string trade_type { get; set; }
        [XmlElement("nonce_str")]
        public string nonce_str { get; set; }
        [XmlElement("is_subscribe")]
        public string is_subscribe { get; set; }
        [XmlElement("out_trade_no")]
        public string out_trade_no { get; set; }

        [XmlIgnore]
        public DateTime createTime { get; set; }

        [XmlIgnore]
        public int status { get; set; } //0 pending, 1 successed, -1 failed
                                        //fk
        [XmlIgnore]
        public int userId { get; set; }
        [XmlIgnore]
        public virtual user user { get; set; }

        [XmlIgnore]
        public int? pay_jsapi_package_requestId { get; set; }
        [XmlIgnore]
        public virtual pay_jsapi_package_request pay_jsapi_package_request { get; set; }
    }
}
