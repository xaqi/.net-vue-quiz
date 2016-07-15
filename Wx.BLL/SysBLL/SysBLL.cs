using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.DAL;
using Wx;
using System.Transactions;

namespace Wx.BLL
{
    public class SysBLL
    {
        public static SysBLL Instance
        {
            get
            {
                return new SysBLL();
            }
        }


        public static void Log(string detail)
        {
            using (var dc = new DC())
            {
                dc.sys_logs.Add(new sys_log { detail = detail, create_time = DateTime.Now });
                dc.SaveChanges();
            }
        }


    }
}
