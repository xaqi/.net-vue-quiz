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
using System.Web.Caching;
using System.Web;

namespace Wx.Common.Helpers
{

    public class HotConfig
    {
        static List<HotConfig> hotConfigs = new List<HotConfig> { };
        static DateTime initTime = DateTime.MinValue;
        public static List<HotConfig> HotConfigs(DateTime dt)
        {
            if (initTime.Date != dt.Date)
            {
                Init(dt);
                initTime = dt;
            }
            return hotConfigs;
        }
        static void Init(DateTime dt)
        {
            hotConfigs = new List<HotConfig> {
            new HotConfig{ MinHour=0, MaxHour=5, Rate=0.1*5},
            new HotConfig{ MinHour=5, MaxHour=8, Rate=0.15*3},
            new HotConfig{ MinHour=8, MaxHour=18, Rate=0.3*10},
            new HotConfig{ MinHour=18, MaxHour=22, Rate=0.8*4},
            new HotConfig{ MinHour=22, MaxHour=24, Rate=0.1*2}
        }.OrderBy(r => r.MinHour).ToList();
            hotConfigs.ForEach(hc =>
            {
                hc.MinRand = (hotConfigs.Where(r => r.MinHour < hc.MinHour).Sum(r => r.Rate)) / hotConfigs.Sum(r => r.Rate);
                hc.MaxRand = (hotConfigs.Where(r => r.MinHour < hc.MinHour).Sum(r => r.Rate) + hc.Rate) / hotConfigs.Sum(r => r.Rate);
                hc.MinPersonIndex = hc.MinRand * 20000;
                hc.MaxPersonIndex = hc.MaxRand * 20000;
                hc.StartTime = dt.Date.AddHours(hc.MinHour);
                hc.EndTime = dt.Date.AddHours(hc.MaxHour);
                hc.TimeSpanSenconds = (hc.EndTime - hc.StartTime).TotalSeconds;
            });
        }
        public int MinHour { get; set; }
        public int MaxHour { get; set; }
        public double Rate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double MinRand { get; set; }
        public double MaxRand { get; set; }
        public double MinPersonIndex { get; set; }
        public double MaxPersonIndex { get; set; }
        public double TimeSpanSenconds { get; set; }
    }
    public class ZJHelper
    {
        public static SortedList<double, double> Cache = new SortedList<double, double> { };
        static ZJHelper()
        {
            Init(DateTime.Now.Date);
        }
        static DateTime DT = DateTime.Now.Date;
        static void Init(DateTime dt)
        {
            DT = dt.Date;
            Init(3111, DT, 30);
        }
        public static ZJLog GetLog(DateTime dt)
        {
            if (dt.Date != DT)
            {
                Init(dt);
            }
            var second = (dt - dt.Date).TotalSeconds;
            var q = Cache.Where(r => r.Key < second);
            var money = q.Sum(r => r.Value);
            var personCount = q.Count();
            return new ZJLog { Money = money, PeopleCount = personCount, dt = dt };
        }
        public static object Init(int totalPersonCount, DateTime dt, double evMoney)
        {
            Cache.Clear();
            Func<int, int> iRand = sd => new Random(sd).Next();
            Func<int, double> fRand = sd => (double)iRand(sd) / (double)int.MaxValue; int currentPersonCount = 0;
            double currentMoney = 0;
            var dts = new List<int> { };
            var day = dt.DayOfYear;
            var seed = 44232 + day;
            for (int i = 0; i < totalPersonCount; i++)
            {
                seed = iRand(seed);
                var rand = fRand(seed);
                var rand2 = fRand(seed + i);
                var hc = HotConfig.HotConfigs(dt).First(r => r.MinRand <= rand && r.MaxRand >= rand);
                var time = hc.StartTime.AddSeconds(hc.TimeSpanSenconds * rand2);

                var timeSeconds = (time - dt.Date).TotalSeconds;


                var log = (int)Math.Log(1 / rand);
                log = Math.Min(log, 5);
                var avmoney = 100 * (Math.Pow(2, log));
                currentPersonCount++;
                currentMoney += avmoney * 2 * rand;
                //Cache.Add(timeSeconds, new ZJLog { Senconds=timeSeconds, Money=currentMoney, PeopleCount=currentPersonCount });
                Cache[timeSeconds] = avmoney * 2 * rand;
            }

            //		maxRand.Dump("MR");
            //dts.OrderByDescending(r => r).GroupBy(r => r.Hour).Select(r => new { r.Key, c = r.Count() }).Dump();
            return new { currentPersonCount, currentMoney };
        }
    }
    public class ZJLog
    {
        public int PeopleCount { get; set; }
        public double Money { get; set; }
        public DateTime dt { get; set; }
    }
}
