using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Wx.Common
{
	public class JsonHelper
	{
		public static string Serialize(object obj)
		{
			return new JavaScriptSerializer().Serialize(obj);
		}
		public static T Deserialize<T>(string json)
		{
			return new JavaScriptSerializer().Deserialize<T>(json);
		}
		public static T Deserialize<T>(System.Collections.IDictionary dict)
		{
			return new JavaScriptSerializer().Deserialize<T>(JsonHelper.Serialize(dict));
		}
	}

}
