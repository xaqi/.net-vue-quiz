using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Wx.Web
{
	[HubName("roulette")]
	public class rouletteHub : Hub
	{
		//public roulette()
		//{
		//	AllClients = Clients.All;

		//}
		public static dynamic AllClients { get; set; }
		public static void SendMessage(string typeName, params object[] args)
		{
			if (AllClients == null) return;
			AllClients.message(typeName, args);

		}

		[HubMethodName("Hello")]
		public void Hello()
		{
			AllClients = Clients.All;
			Clients.All.message("answer", new object[] { 100 });
		}

		public override System.Threading.Tasks.Task OnConnected()
		{
			AllClients = Clients.All;
			Clients.Caller.message("welcome", new object[] { "cc" });
			return base.OnConnected();
		}
	}
}