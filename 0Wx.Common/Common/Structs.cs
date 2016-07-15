using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wx.Common
{
	public class ReturnMessage
	{
		public bool success { get; set; }
		public string message { get; set; }
		public object data { get; set; }
	}
}
