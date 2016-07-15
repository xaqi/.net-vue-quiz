using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NATServer.Startup))]
namespace NATServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			//ConfigureAuth(app);
			app.MapSignalR();
        }
    }
}
