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
using System.Collections.Generic;

namespace Wx.Common.Helpers
{
    public class CacheHelper
    {
        public static T Get<T>(string key, Func<T> func)
        {
            var item = (T)HttpRuntime.Cache.Get(key);
            if (item == null)
            {
                item = func();
                HttpRuntime.Cache.Insert(key, item);
            }
            return item;
        }

        public static void ClearAll()
        {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            var keys = new List<string> { };
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            keys.ForEach(key => HttpRuntime.Cache.Remove(key));
        }

    }
}
