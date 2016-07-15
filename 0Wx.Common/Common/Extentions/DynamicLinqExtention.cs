using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Linq.Dynamic;
using System.Web.Script.Serialization;
using GT.Tools;


namespace GT.Tools
{
    public static class DynamicLinqExtention
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public static IQueryable<object> FuzzySearch(this IQueryable<object> query, string keyword, IEnumerable<string> fuzzySearchFields)
        {
            var propertyPaths = GetPropertyPaths(query.ElementType);
            var fuzzyFormat = string.Join(" OR ", fuzzySearchFields.Select(p => propertyPaths[p] + ".Contains(@0)").ToArray());
            if (!string.IsNullOrWhiteSpace(fuzzyFormat) && !string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(fuzzyFormat, keyword.Trim());
            }
            return query;
        }
        /// <summary>
        /// 高级查询
        /// </summary>
        public static IQueryable<object> AdvancedSearch(this IQueryable<object> query, string advancedQuery = null)
        {
            if (query == null || advancedQuery == null) return query;
            var advArr = new JavaScriptSerializer().Deserialize<Dictionary<string, object>[]>(advancedQuery);
            var propertyPaths = GetPropertyPaths(query.ElementType);
            advArr.ToList().ForEach(filter =>
            {
                var key = filter["k"] as string;
                var value = filter["v"];
                var oper = filter["o"] as string;
                if (key == null || value == null || oper == null) return;
                var operFomarts = new Dictionary<string, string>
                {
                    {"contains","{0}.Contains(@0)"},
                    {"startswith","{0}.StartsWith(@0)"},
                    {"endswith","{0}.EndsWith(@0)"},
                    {"=","{0}=(@0)"},
                    {">=","{0}>=(@0)"},
                    {"<=","{0}<=(@0)"},
                    {">","{0}>(@0)"},
                    {"<","{0}<(@0)"}
                };
                if (oper.ToLower() == "datebetween")
                {
                    var txtDate = value.ToString().Trim();
                    DateTime? dt1 = null, dt2 = null;
                    if (txtDate.StartsWith("到"))
                    {
                        dt2 = DateTime.Parse(txtDate.Replace("到", "")).AddDays(1);
                    }
                    else if (txtDate.EndsWith("到"))
                    {
                        dt1 = DateTime.Parse(txtDate.Replace("到", ""));
                    }
                    else if (txtDate.Contains("到"))
                    {
                        dt1 = DateTime.Parse(txtDate.Split(new string[] { "到" }, StringSplitOptions.None)[0]);
                        dt2 = DateTime.Parse(txtDate.Split(new string[] { "到" }, StringSplitOptions.None)[1]).AddDays(1);
                    }
                    else
                    {
                        dt1 = DateTime.Parse(value.ToString());
                        dt2 = dt1.Value.AddDays(1);

                    }
                    if (dt1 != null)
                    {
                        query = query.Where(string.Format("{0}>=(@0)", propertyPaths[key]), dt1);
                    }
                    if (dt2 != null)
                    {
                        query = query.Where(string.Format("{0}<(@0)", propertyPaths[key]), dt2);
                    }
                }

                if (!operFomarts.ContainsKey(oper.ToLower()))
                {
                    return;
                }
                if (propertyPaths.ContainsKey(key))
                {
                    query = query.Where(string.Format(operFomarts[oper.ToLower()], propertyPaths[key]), value);
                }
            });
            return query;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public static IQueryable<object> Sort(this IQueryable<object> query, string sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                var sorts = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, string>[]>(sort);
                var propertyPaths = GetPropertyPaths(query.ElementType);
                if (sort != null && sort.Length > 0)
                {
                    query = query.OrderBy(string.Join(",", sorts.Select(r => propertyPaths[r["property"]] + " " + r["direction"])));
                }
            }
            return query;
        }

        private static Dictionary<string, string> GetPropertyPaths(Type elementType)
        {
            var dict = new Dictionary<string, string>();
            elementType.GetProperties().ToList().ForEach(p =>
                {
                    if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
                    {
                        p.PropertyType.GetProperties().Where(r => !r.PropertyType.IsClass || r.PropertyType == typeof(string)).ToList().ForEach(r => dict[r.Name] = p.Name + "." + r.Name);
                    }
                    else
                    {
                        dict[p.Name] = p.Name;
                    }
                });
            return dict;
        }

    }
}