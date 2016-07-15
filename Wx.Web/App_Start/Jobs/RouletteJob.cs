using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBackgrounder;
using System.Threading.Tasks;
using Wx.DAL;
using Wx.Enums;
using Wx.BLL;
using Wx.Web;

namespace Wx.Web.App_Start
{

    public class RouletteJob : Job
    {

        public RouletteJob()
            : base("RouletteJob Job", TimeSpan.FromSeconds(1 * 15), TimeSpan.FromSeconds(30))
        {
        }

        static DateTime lastTimer = DateTime.Now.AddSeconds(-100);
        static int LastFetchedSSCCount = 0;
        public override Task Execute()
        {
            return new Task(() =>
            {
                //SysBLL.Log("start job");
                var fetchedSSCCount = Wx.Common.Helpers.SSCHelper.FetchSSC(DateTime.Now.Date).Where(r => !string.IsNullOrWhiteSpace(r.no)).Count();
                if (fetchedSSCCount == LastFetchedSSCCount) return;
                SysBLL.Log("start resolve");
                LastFetchedSSCCount = fetchedSSCCount;
                var rbll = new RouletteBLL();
                rbll.Resolve(true);
            });
        }
    }
}