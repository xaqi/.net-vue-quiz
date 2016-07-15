using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;


namespace Wx.DAL
{
    public class utConfig
    {
        public int utConfigId { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }

    public class sys_log
    {
        public int sys_logId { get; set; }
        public string detail { get; set; }
        public DateTime create_time { get; set; }
    }

    public class sys_art
    {
        public int sys_artId { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string search_value { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public string update_user { get; set; }
    }
}
