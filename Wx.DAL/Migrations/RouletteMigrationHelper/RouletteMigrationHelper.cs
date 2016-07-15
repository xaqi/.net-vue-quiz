using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.Enums;
using Wx.Common;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Wx.Enums;
using Wx.Common;


namespace Wx.DAL
{
    public class RouletteMigrationHelper
    {
        public static void InitLolAndDotaRoulette(DC context)
        {
            #region ====data 1=====
            var data1 = @"1  黑暗之女 安妮  4800  远程
2  寒冰射手 艾希  450  远程
3  牛头酋长 阿利斯塔  1350  近战
4  卡牌大师 崔斯特  4800  远程
5  战争女神 希维尔  450  远程
6  迅捷斥候 提莫  6300  远程
7  审判天使 凯尔  450  远程
8  末日使者 费德提克  1350  远程
9  雪人骑士 努努  450  近战
10  众星之子 索拉卡  450  远程
11  流浪法师 瑞兹  450  远程
12  无极剑圣 易  450  近战
13  嗜血猎手 沃里克  3150  近战
14  麦林炮手 崔丝塔娜  1350  远程
15  亡灵勇士 赛恩  1350  近战
16  武器大师 贾克斯  3150  近战
17  堕落天使 莫甘娜  1350  远程
18  炼金术士 辛吉德  1350  近战
19  时光守护者 基兰  450  远程
20  蛮族之王 泰达米尔  4800  近战
21  寡妇制造者 伊芙琳  1350  近战
22  瘟疫之源 图奇  4800  远程
23  死亡颂唱者 卡尔萨斯  4800  远程
24  虚空恐惧 科加斯  3150  近战
25  殇之木乃伊 阿木木  1350  近战
26  披甲龙龟 拉莫斯  3150  近战
27  冰晶凤凰 艾尼维亚  4800  远程
28  邪恶小法师 维迦  1350  远程
29  虚空行者 卡萨丁  3150  近战
30  宝石骑士 塔里克  3150  近战
31  海洋之灾 普朗克  3150  近战
32  蒸汽机器人 布里茨  3150  近战
33  风暴之怒 迦娜  1350  远程
34  熔岩巨兽 墨菲特  1350  近战
35  祖安狂人 蒙多  1350  近战
36  英勇投弹手 库奇  6300  远程
37  不祥之刃 卡特琳娜  3150  近战
38  沙漠死神 内瑟斯  3150  近战
39  恶魔小丑 萨科  4800  近战
40  大发明家 黑默丁格  3150  远程
41  兽灵行者 乌迪尔  3150  近战
42  狂野女猎手 奈德丽  6300  远程
43  钢铁大使 波比  450  近战
44  战争之王 潘森  3150  近战
45  酒桶 古拉加斯  3150  近战
46  金属大师 莫德凯撒  3150  近战
47  探险家 伊泽瑞尔  4800  远程
48  暮光之眼 慎  3150  近战
49  狂暴之心 凯南  4800  远程
50  德玛西亚之力 盖伦  3150  近战
51  暗影之拳 阿卡丽  3150  近战
52  虚空先知 玛尔扎哈  4800  远程
53  狂战士 奥拉夫  3150  近战
54  深渊巨口 克格莫  6300  远程
55  德邦总管 赵信  3150  近战
56  猩红收割者 弗拉基米尔  3150  远程
57  哨兵之殇 加里奥  3150  近战
58  首领之傲 厄加特  1350  远程
59  赏金猎人 厄运小姐  3150  远程
60  琴瑟仙女 娑娜  3150  远程
61  策士统领 斯维因  4800  远程
62  光辉女郎 拉克丝  3150  远程
63  诡术妖姬 乐芙兰  4800  远程
64  刀锋意志 艾瑞莉娅  6300  近战
65  巨魔之王 特朗德尔  3150  近战
66  魔蛇之拥 卡西奥佩娅  6300  远程
67  皮城女警 凯特琳  6300  远程
68  荒漠屠夫 雷克顿  4800  近战
69  天启者 卡尔玛  3150  远程
70  扭曲树精 茂凯  4800  近战
71  德玛西亚皇子 嘉文四世  4800  近战
72  永恒梦魇 魔腾  4800  近战
73  盲僧 李青  4800  近战
74  复仇焰魂 布兰德  4800  远程
75  机械公敌 兰博  4800  近战
76  暗夜猎手 薇恩  4800  远程
77  发条魔灵 奥莉安娜  6300  远程
78  掘墓者 约里克  3150  近战
79  曙光女神 蕾欧娜  4800  近战
80  齐天大圣 孙悟空  6300  近战
81  水晶先锋 斯卡纳  4800  近战
82  刀锋之影 泰隆  6300  近战
83  放逐之刃 锐雯  6300  近战
84  远古巫灵 泽拉斯  4800  远程
85  法外狂徒 格雷福斯  6300  远程
86  龙血武姬 希瓦娜  6300  近战
87  潮汐海灵 菲兹  6300  近战
88  雷霆咆哮 沃利贝尔  6300  近战
89  九尾妖狐 阿狸  6300  远程
90  机械先驱 维克托  4800  远程
91  凛冬之怒 瑟庄妮  4800  近战
92  爆破鬼才 吉格斯  6300  远程
93  深海泰坦 诺提勒斯  4800  近战
94  无双剑姬 菲奥娜  6300  近战
95  仙灵女巫 璐璐  6300  远程
96  战争之影 赫卡里姆  6300  近战
97  惩戒之箭 韦鲁斯  6300  远程
98  诺克萨斯之手 德莱厄斯  6300  近战
99  荣耀行刑官 德莱文  6300  远程
100  未来守护者 杰斯  6300  远程
101  荆棘之兴 婕拉  4800  远程
102  皎月女神 黛安娜  6300  近战
103  傲之追猎者 雷恩加尔  6300  近战
104  暗黑元首 辛德拉  6300  远程
105  虚空掠夺者 卡兹克  6300  近战
106  蜘蛛女皇 伊利斯  4800  远程
107  影流之主 劫  6300  近战
108  唤潮鲛姬 娜美  4800  远程
109  皮城执法官 蔚  6300  近战
110  魂锁典狱长 锤石  6300  远程
111  德玛西亚之翼 奎因  6300  远程
112  生化魔人 扎克  4800  近战
113  冰霜女巫 丽桑卓  6300  远程
114  暗裔剑魔 亚托克斯  6300  近战
115  圣枪游侠 卢锡安  6300  远程
116  暴走萝莉 金克丝  6300  远程
117  疾风剑豪 亚索  6300  近战
118  虚空之眼 维克兹  6300  远程
119  弗雷尔卓德之心 布隆  6300  近战
120  迷失之牙 纳尔  6300  远程
121  沙漠皇帝 阿兹尔  6300  远程
122  复仇之矛 卡莉丝塔  6300  远程
123  虚空遁地兽 雷克塞  6300  近战
124  星界游神 巴德  6300  远程
125  时间刺客 艾克  6300  近战
126  河流之王 塔姆•肯奇  6300  近战
127  永猎双子 千珏  6300  远程
128  海兽祭司 俄洛伊  6300  近战
129  戏命师 烬  6300  远程
";
            #endregion

            #region ==========data2==========
            var data2 = @"1	沉默术士	天辉	智力	远程
2	殁境神蚀者	夜魇	智力	远程
3	寒冬飞龙	夜魇	智力	远程
4	戴泽	夜魇	智力	远程
5	食人魔法师	天辉	智力	近程
6	暗影恶魔	夜魇	智力	远程
7	祸乱之源	夜魇	智力	远程
8	蝙蝠骑士	夜魇	智力	远程
9	先知	天辉	智力	远程
10	巫妖	夜魇	智力	远程
11	希拉克	夜魇	智力	远程
12	杰奇洛	天辉	智力	远程
13	远古冰魂	夜魇	智力	远程
14	黑暗贤者	夜魇	智力	近程
15	神谕者	天辉	智力	远程
16	莉娜	天辉	智力	远程
17	陈	天辉	智力	远程
18	风行者	天辉	智力	远程
19	帕格纳	夜魇	智力	远程
20	祈求者	夜魇	智力	远程
21	痛苦女王	夜魇	智力	远程
22	谜团	夜魇	智力	远程
23	风暴之灵	天辉	智力	远程
24	死灵飞龙	夜魇	智力	远程
25	暗影萨满	天辉	智力	远程
26	死亡先知	夜魇	智力	远程
27	恶魔巫师	夜魇	智力	远程
28	瘟疫法师	夜魇	智力	远程
29	地精工程师	天辉	智力	远程
30	天怒法师	天辉	智力	远程
31	术士	夜魇	智力	远程
32	帕克	天辉	智力	远程
33	光之守卫	天辉	智力	远程
34	巫医	夜魇	智力	远程
35	宙斯	天辉	智力	远程
36	水晶室女	天辉	智力	远程
37	干扰者	天辉	智力	远程
38	拉比克	天辉	智力	远程
39	地精修补匠	天辉	智力	远程
40	魅惑魔女	天辉	智力	远程
41	弧光守望者	天辉	敏捷	远程
42	幻影长矛手	天辉	敏捷	近程
43	复仇之魂	天辉	敏捷	远程
44	娜迦海妖	天辉	敏捷	近程
45	血魔	夜魇	敏捷	近程
46	矮人直升机	天辉	敏捷	远程
47	圣堂刺客	天辉	敏捷	远程
48	矮人狙击手	天辉	敏捷	远程
49	影魔	夜魇	敏捷	远程
50	变体精灵	天辉	敏捷	远程
51	力丸	天辉	敏捷	近程
52	育母蜘蛛	夜魇	敏捷	近程
53	熊战士	天辉	敏捷	近程
54	剃刀	夜魇	敏捷	远程
55	恐惧利刃	夜魇	敏捷	近程
56	赏金猎人	天辉	敏捷	近程
57	司夜刺客	夜魇	敏捷	近程
58	露娜	天辉	敏捷	远程
59	冥界亚龙	夜魇	敏捷	远程
60	幽鬼	夜魇	敏捷	近程
61	剧毒术士	夜魇	敏捷	远程
62	米拉娜	天辉	敏捷	远程
63	德鲁伊	天辉	敏捷	远程
64	克林克兹	夜魇	敏捷	远程
65	幻影刺客	夜魇	敏捷	近程
66	虚空假面	夜魇	敏捷	近程
67	敌法师	天辉	敏捷	近程
68	美杜莎	夜魇	敏捷	远程
69	灰烬之灵	天辉	敏捷	近程
70	主宰	天辉	敏捷	近程
71	巨魔战将	天辉	敏捷	远程
72	米波	夜魇	敏捷	近程
73	编织者	夜魇	敏捷	远程
74	斯拉克	夜魇	敏捷	近程
75	卓尔游侠	天辉	敏捷	远程
76	半人马战行者	天辉	力量	近程
77	树精卫士	天辉	力量	近程
78	马格纳斯	夜魇	力量	近程
79	军团指挥官	天辉	力量	近程
80	大地之灵	天辉	力量	近程
81	暗夜魔王	夜魇	力量	近程
82	刚被兽	天辉	力量	近程
83	斧王	夜魇	力量	近程
84	斯拉达	夜魇	力量	近程
85	龙骑士	天辉	力量	近程
86	巨牙海民	天辉	力量	近程
87	亚巴顿	夜魇	力量	近程
88	沙王	夜魇	力量	近程
89	冥魂大帝	夜魇	力量	近程
90	酒仙	天辉	力量	近程
91	发条地精	天辉	力量	近程
92	狼人	夜魇	力量	近程
93	噬魂者	夜魇	力量	近程
94	斯温	天辉	力量	近程
95	帕吉	夜魇	力量	近程
96	潮汐猎人	夜魇	力量	近程
97	裂魂人	夜魇	力量	近程
98	全能骑士	天辉	力量	近程
99	地精撕裂者	天辉	力量	近程
100	混沌骑士	夜魇	力量	近程
101	末日	夜魇	力量	近程
102	昆卡	天辉	力量	近程
103	撼地者	天辉	力量	近程
104	兽王	天辉	力量	近程
105	上古巨神	天辉	力量	近程
106	凤凰	天辉	力量	远程
107	不朽尸王	夜魇	力量	近程
108	哈斯卡	天辉	力量	远程
109	小小	天辉	力量	近程
110	精灵守卫	天辉	力量	远程
111	炼金术士	天辉	力量	近程
112	肉山	肉山	力量	近程
";
            #endregion



            InitRoulette(context, data1.Replace("近战", "近程"), new Dictionary<string, int> { { "金币", 2 }, { "近远程", 3 }, { "字数",4} }, 1, 1, r =>
             {
                 var money = int.Parse(r[2]);
                 if (money >= 6300)
                 {
                     r[2] = "6300及以上";
                 }
                 if (money <= 1350)
                 {
                     r[2] = "1350及以下";
                 }
                 var nameLen = r[1].Length;
                 var nameLenStr = "三个字";
                 if (nameLen >= 4)
                 {
                     nameLenStr = "四个字及以上";
                 }
                 if (nameLen <= 2)
                 {
                     nameLenStr = "两个字及以下";
                 }
                 r.Add(nameLenStr);
             });
            InitRoulette(context, data2.Replace("近战", "近程"), new Dictionary<string, int> { { "阵营", 1 }, { "主属性", 2 }, { "攻击类型", 3 } }, 2, 0, r => { });
        }

        public static void InitRoulette(DC context, string data, Dictionary<string, int> proIndexs, int gameTypeId, int nameIndex, Action<List<string>> f)
        {

            var proTypes = proIndexs.Keys.Select(ptName => new roulette_property_type { name = ptName, game_typeId = gameTypeId, displayOrder = 0 }).ToList();
            proTypes.ForEach(proType => context.roulette_property_types.AddOrUpdate(r => r.name, proType));
            context.SaveChanges();

            var arr = data.Split('\n').Select(r => r.Trim())
               .Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Replace("\t", " ")
               .Split(' ')).Select(r => r.Skip(1).Where(p => !string.IsNullOrWhiteSpace(p)).ToList()).ToList();
            arr.ForEach(item => f(item));
            var props = proTypes.SelectMany(ptype =>
            {
                var proIndex = proIndexs[ptype.name];
                var proValues = arr.Select(r => r[proIndex]).Distinct();
                return proValues.Select(proValue => new roulette_property { name = proValue, roulette_property_typeId = ptype.roulette_property_typeId, displayOrder = 0 });
            }).ToList();
            props.ForEach(prop => context.roulette_propertys.Add(prop));
            context.SaveChanges();
            arr.ToList().ForEach(r =>
                {
                    var hero = new roulette_hero { name = r[nameIndex], description = string.Join(",", r), game_typeId = gameTypeId };
                    context.roulette_heros.AddOrUpdate(p => p.name, hero);
                    context.SaveChanges();
                    proIndexs.Keys.ToList().ForEach(key =>
                        {
                            var pName = key;
                            var pValue = r[proIndexs[key]];
                            var propId = props.Where(p => p.name == pValue).FirstOrDefault().roulette_propertyId;
                            context.roulette_hero_propertys.AddOrUpdate(p => p.roulette_hero_propertyId, new roulette_hero_property { roulette_heroId = hero.roulette_heroId, roulette_propertyId = propId });
                        });
                });
            context.SaveChanges();
        }




    }

}




