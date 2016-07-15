using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Text.RegularExpressions;
using Wx.DAL;

namespace Wx.Common.Helpers
{
    public class SSCHelper
    {
        public static List<SSCItem> GetSSCList(DateTime dt)
        {
            return SSCItem.GetSSCList(dt);
        }

        public static List<SSCItem> SSCItemList = new List<SSCItem> { };
        public static bool IsOnFetching = false;
        public static int DelaySeconds = 80;


        public static List<SSCItem> FetchSSC(DateTime dt)
        {
            var outputs = new List<string> { };
            if (SSCItemList.Count(r => r.startTime.Date == dt.Date) < 10)
            {
                SSCItemList = SSCHelper.GetSSCList(dt);
                outputs.Add("update new date");
            }
            var toResolvedItems = SSCItemList.Where(r => string.IsNullOrWhiteSpace(r.no) && r.endTime.AddSeconds(DelaySeconds) < DateTime.Now);
            if (!toResolvedItems.Any())
            {
                outputs.Add("not time to resolve");
                return SSCItemList;
            }
            if (IsOnFetching) return SSCItemList;
            try
            {
                IsOnFetching = true;
                var api = string.Format("http://chart.cp.360.cn/kaijiang/kaijiang?lotId=255401&spanType=2&span={0}_{0}", dt.ToString("yyyy-MM-dd"));
                var wc = new WebClient();
                var html = wc.DownloadString(api);
                var regex = new Regex(@"<td class='gray'>(\d+)</td><td class='red big'>(\d+)</td>");
                var list = regex.Matches(html).Cast<Match>().Select(r => new
                {
                    index = int.Parse(r.Groups[1].Value),
                    number = int.Parse(r.Groups[2].Value),
                }).ToList();
                list.ForEach(r => SSCItemList.Where(p => r.index == int.Parse(p.period.Substring(p.period.Length - 3))).ToList().ForEach(p => p.no = r.number.ToString()));

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsOnFetching = false;
            }

            return SSCItemList;
        }

        //public static void Resolve()
        //{
        //    using (var dc = new DC())
        //    {

        //        //dc.roulette_submits.Where(r=>r.)
        //    }
        //}

    }



}
