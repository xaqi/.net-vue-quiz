using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBackgrounder;
using System.Threading.Tasks;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Wx.Web.App_Start.WebBackgrounderSetup), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Wx.Web.App_Start.WebBackgrounderSetup), "Shutdown")]
namespace Wx.Web.App_Start
{

	public static class WebBackgrounderSetup
	{
		static readonly JobManager _jobManager = CreateJobWorkersManager();

		public static void Start()
		{
			_jobManager.Start();
		}

		public static void Shutdown()
		{
			_jobManager.Dispose();
		}

		private static JobManager CreateJobWorkersManager()
		{
			var jobs = new IJob[]
        {
			new RouletteJob()
        };
			if (System.Configuration.ConfigurationManager.AppSettings["Environment"] != "prod")
			{
				jobs = new IJob[] { };
			}
			var coordinator = new SingleServerJobCoordinator();
			var manager = new JobManager(jobs, coordinator);
			//manager.Fail(ex => Logger.Error("Web Job Blow Up.", ex));
			return manager;
		}
	}

}