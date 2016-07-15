namespace Wx.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections;
    using System.Collections.Generic;

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DC : DbContext
    {
        /*
		  Update-Database -Verbose -Force
		*/
        public DC()
            : base("name=DC." +
            (
            System.Configuration.ConfigurationSettings.AppSettings.AllKeys.Contains("Environment") ? System.Configuration.ConfigurationSettings.AppSettings["Environment"] : "local")
            )
        {
        }


        //public DC()
        //    : base("name=DC.local")
        //{
        //}

        public DC(string env)
            : base("name=DC." + env)
        {

        }

        public DbSet<game_type> game_types { get; set; }
        public DbSet<game> games { get; set; }
        public DbSet<team> teams { get; set; }
        public DbSet<bet> bets { get; set; }

        public DbSet<quiz> quizs { get; set; }
        public DbSet<option> options { get; set; }
        public DbSet<user_option> user_options { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<user_login_history> user_login_historys { get; set; }
        public DbSet<user_login_bonus> user_login_bonuses { get; set; }

        public DbSet<user_golds_record> user_golds_records { get; set; }
        public DbSet<user_invite_log> user_invite_logs { get; set; }
        public DbSet<user_invite_channel> user_invite_channels { get; set; }


        public DbSet<pay> pays { get; set; }
        public DbSet<redeem> redeems { get; set; }
        public DbSet<pay_jsapi_package_request> pay_jsapi_package_requests { get; set; }
        public DbSet<pay_jsapi_notify> pay_jsapi_notifys { get; set; }
        //pay_jsapi_package_request

        public DbSet<roulette_property_type> roulette_property_types { get; set; }
        public DbSet<roulette_hero> roulette_heros { get; set; }
        public DbSet<roulette_property> roulette_propertys { get; set; }
        public DbSet<roulette_hero_property> roulette_hero_propertys { get; set; }
        public DbSet<roulette_submit> roulette_submits { get; set; }
        public DbSet<ssc_history> ssc_historys { get; set; }


        public DbSet<sys_log> sys_logs { get; set; }
        public DbSet<sys_art> sys_arts { get; set; }


        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<pay_jsapi_notify>().HasOptional(r => r.pay_jsapi_package_request).WithMany().HasForeignKey(r => r.pay_jsapi_package_requestId);
            modelBuilder.Entity<pay_jsapi_package_request>().HasOptional(r => r.pay_jsapi_notify).WithMany().HasForeignKey(r => r.pay_jsapi_notifyId);
            modelBuilder.Entity<pay>().HasOptional(r => r.pay_jsapi_package_request).WithMany().HasForeignKey(r => r.pay_jsapi_package_requestId);
            modelBuilder.Entity<pay>().HasOptional(r => r.pay_jsapi_notify).WithMany().HasForeignKey(r => r.pay_jsapi_notifyId);

            base.OnModelCreating(modelBuilder);
        }



    }

}
