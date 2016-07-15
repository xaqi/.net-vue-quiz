using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;


namespace Wx.DAL
{
    public class roulette_property_type
    {
        public int roulette_property_typeId { get; set; }
        public string name { get; set; }
        public int displayOrder { get; set; }
        //fk
        public int game_typeId { get; set; }
        public virtual game_type game_type { get; set; }

        public virtual ICollection<roulette_property> roulette_propertys { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class roulette_property
    {
        public int roulette_propertyId { get; set; }
        public string name { get; set; }
        public int displayOrder { get; set; }
        //fk
        public int roulette_property_typeId { get; set; }
        public virtual roulette_property_type roulette_property_type { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class roulette_hero
    {
        public int roulette_heroId { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        //fk
        public int game_typeId { get; set; }
        public virtual game_type game_type { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class roulette_hero_property
    {
        public int roulette_hero_propertyId { get; set; }
        //fk
        public int roulette_heroId { get; set; }
        public int roulette_propertyId { get; set; }
        public virtual roulette_hero hero { get; set; }
        public virtual roulette_property property { get; set; }
        public bool IsDeleted { get; set; }
    }



    public class roulette_submit
    {
        public int roulette_submitId { get; set; }
        public string propertys { get; set; }
        public double golds { get; set; }
        public string sscPeriod { get; set; }
        public DateTime createTime { get; set; }

        //fk
        public int userId { get; set; }
        public virtual user user { get; set; }
        public int game_typeId { get; set; }

        public int matched_hero_count { get; set; }
        public int total_hero_count { get; set; }
        public double odds { get; set; }

        public string ssc_no { get; set; }
        public int correct_heroId { get; set; }
        public DateTime resolve_time { get; set; }

        public string correct_propertys { get; set; }
        public Enums.RouletteSubmitStatus rouletteSubmitStatus { get; set; }

    }

    public class ssc_history
    {
        public int ssc_historyId { get; set; }
        public string period { get; set; }
        public string no { get; set; }
        public string heroInfo { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
    }

}
