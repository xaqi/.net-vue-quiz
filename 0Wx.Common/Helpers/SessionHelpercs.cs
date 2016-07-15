using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wx.Common.Helpers
{
	public class SessionHelper
	{
		public bool IsWx()
		{
			var hc = HttpContext.Current;
			return hc != null && HttpContext.Current.Request.Headers["User-Agent"].Contains("Wechat");
		}

		public object this[string index]
		{
			get
			{
				var hc = HttpContext.Current;
				if (hc == null) return null;
				return hc.Session[index];// ?? HttpRuntime.Cache[index];
			}
			set
			{
				var hc = HttpContext.Current;
				if (hc == null) return;
				hc.Session[index] = value;
				//HttpRuntime.Cache[index] = value;

			}
		}
	}
}
