using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wx.Common
{
    public static class Extentions
    {
        public static double Round(this double dbl, int decimals)
        {
            return Math.Round(dbl, decimals);
        }
        public static Dictionary<string, object> AsDictionary(this object obj)
        {
            if (obj == null) return new Dictionary<string, object> { };
            return obj.GetType().GetProperties().Select(r => new { r.Name, Value = r.GetValue(obj) }).ToDictionary(r => r.Name, r => r.Value);
        }
    }


    public static class IQExtentions
    {
        public static PagedListResult ToPagedList(this IQueryable<object> query)
        {
            var request = System.Web.HttpContext.Current.Request;
            var p = int.Parse(request.QueryString["p"] ?? "1");
            var ps = int.Parse(request.QueryString["ps"] ?? "20");
            var total = query.Count();
            var rows = query.Skip((p - 1) * ps).Take(ps).ToList();
            var totalPages = ((total + 1) / ps) + 1;
            return new PagedListResult { total = total, rows = rows, totalPages = totalPages };

        }

    }
    public class PagedListResult
    {
        public int total { get; set; }
        public List<object> rows { get; set; }
        public int totalPages { get; set; }
    }
}
