using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;

namespace GT.Tools
{
    public static class DictionaryExtention
    {
        public static T As<T>(this Dictionary<string, object> dict) where T : class,new()
        {
            var jser = new JavaScriptSerializer();
            return jser.Deserialize<T>(jser.Serialize(dict));
        }

        public static object GetKey(this Dictionary<string, object> dict, string key, object defaultValue = null)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            if (dict.Keys.Select(r => r.ToLower()).Contains(key.ToLower()))
            {
                return dict[dict.Keys.Where(r => r.ToLower() == key.ToLower()).FirstOrDefault()];
            }
            return defaultValue;
        }

        public static Dictionary<string, object> AsDictionary(this object obj)
        {
            var T = obj.GetType();
            var dict = new Dictionary<string, object>();
            if (obj == null)
            {
                return dict;
            }
            T.GetProperties().ToList().ForEach(p => dict[p.Name] = p.GetValue(obj, null));
            return dict;
        }

        public static Hashtable AsHashtable(this Dictionary<string, object> dict)
        {
            var hash = new Hashtable();
            dict.Keys.ToList().ForEach(key => hash[key] = dict[key]);
            return hash;
        }

    }
}