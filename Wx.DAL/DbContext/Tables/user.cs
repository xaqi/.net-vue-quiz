using System;
using System.Data.Entity;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace Wx.DAL
{
    public class user
    {
        public int userId { get; set; }
        public string openId { get; set; }
        public string name { get; set; }
        public string header { get; set; }
        public double golds { get; set; }


        public string qr_ticket { get; set; }
        public string qr_url { get; set; }
        //update when user invited, for qrcode recycle
        public DateTime qr_create_time { get; set; }
        public DateTime qr_last_invite_time { get; set; }

        public int? invite_by_userId { get; set; }
        public int? invite_by_channelId { get; set; }

        public int login_count { get; set; }
        public DateTime first_login_time { get; set; }
        public DateTime last_login_time { get; set; }

        public string status { get; set; }
        //fk
        public virtual ICollection<user_login_history> user_login_historys { get; set; }
        public virtual ICollection<user_golds_record> user_golds_records { get; set; }
    }

    public class user_invite_channel
    {
        public int user_invite_channelId { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string qr_ticket { get; set; }
        public string qr_url { get; set; }
        public DateTime qr_create_time { get; set; }
        public int user_count { get; set; }
    }

    public class user_login_history
    {
        public int user_login_historyId { get; set; }
        public DateTime loginTime { get; set; }
        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }
    }
    public class user_login_bonus
    {
        public int user_login_bonusId { get; set; }
        public double golds { get; set; }
        public double remain_golds { get; set; }
        public DateTime create_time { get; set; }


        /// <summary>
        /// 1:new user 88 gold; 2:daily login 30 golds
        /// </summary>
        public int bonus_typeId { get; set; }

        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }
    }
    public class user_golds_record
    {
        public int user_golds_recordId { get; set; }
        public double amount { get; set; }
        public double changeAmount { get; set; }
        public string detail { get; set; }
        public DateTime recordTime { get; set; }
        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }
    }

    public class user_invite_log
    {
        public int user_invite_logId { get; set; }
        public string openId { get; set; }
        public int? invite_by_user_id { get; set; }
        public int? invite_by_channel_id { get; set; }
        public string event_type { get; set; }
        public DateTime createTime { get; set; }
    }


}
