namespace Wx.DAL.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using Wx.Enums;
	using Wx.Common;

	internal sealed class Configuration : DbMigrationsConfiguration<Wx.DAL.DC>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}

		protected override void Seed(Wx.DAL.DC context)
		{
			if (!context.sys_arts.Any())
			{
				context.sys_arts.AddOrUpdate(p => p.name,
				new sys_art { name = "lol竞猜规则", search_value = "bet/about/lol", create_time = DateTime.Now },
				new sys_art { name = "dota竞猜规则", search_value = "bet/about/dota", create_time = DateTime.Now },
				new sys_art { name = "lol关于", search_value = "about/lol", create_time = DateTime.Now },
				new sys_art { name = "dota关于", search_value = "about/dota", create_time = DateTime.Now },
				new sys_art { name = "招募规则", search_value = "about/invite", create_time = DateTime.Now }
				);
				context.sys_arts.AddOrUpdate(p => p.name,
					new sys_art { name = "英雄猜规则", search_value = "lol/roulette/about", create_time = DateTime.Now },
					new sys_art { name = "DOTA猜规则", search_value = "dota/roulette/about", create_time = DateTime.Now }
					);
				context.SaveChanges();
			}

			context.SaveChanges();

			if (!context.game_types.Any())
			{
				context.game_types.AddOrUpdate(p => p.enName,
					new game_type { enName = "lol", cnName = "lol" },
					new game_type { enName = "dota2", cnName = "dota2" }
					);
				context.teams.AddOrUpdate(p => p.name,
					new team { name = "system", avatar = "/content/front/design/images/user-inco.png" },
					new team { name = "宝哥", avatar = "/content/front/design/images/user-img.jpg" },
					new team { name = "T1", avatar = "/content/front/design/images/user01.png" },
					new team { name = "T2", avatar = "/content/front/design/images/user02.png" }
					);
				context.SaveChanges();
			}
			if (!context.roulette_heros.Any())
			{
				RouletteMigrationHelper.InitLolAndDotaRoulette(context);
			}
			context.users.AddOrUpdate(r => r.userId, new user
			{
				userId = 1,
				first_login_time = DateTime.Now,
				golds = 10000,
				header = "http://wx.qlogo.cn/mmopen/K91ThKL00OWzSsBNKO0e2PDV4rk5xm6TthapBcKP8Eia051aRUTkUDkibnPXrtNmgrUiaxwlSuuXdiaV3iafhpicNicDseyD6TQOmsA/0/",
				name = "游客",
				openId = "omczmwO6bkBPRoK-lhjb1VB7rmw0",
				qr_create_time = DateTime.Now,
				qr_ticket = "gQGq8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0pUdjZFTTdrU21FelZta1lIUk5kAAIEh_ZPVwMEAAAAAA==",
				qr_url = "http://weixin.qq.com/q/JTv6EM7kSmEzVmkYHRNd",
				status = ""
			});
		}
	}
}
