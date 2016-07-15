using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wx.DAL;
using Wx.BLL;
using Wx.Common;
using System.Linq.Dynamic;


namespace Wx.Portal.Controllers.API
{
    public class QuizController : ApiController
    {
        public object getList(int ps = 10, int p = 1)
        {
            using (var dc = new DC())
            {
                var q = dc.quizs.OrderByDescending(r => r.quizId).
                Select(r => new
                {
                    r.quizId,
                    betname = r.bet.name,
                    gameName = r.bet.game.name,
                    r.subject,
                    r.quizstatus,
                    options = r.options.Select(k => k.subject)
                });
                return q.ToPagedList();
            }

        }
        public object save(Dictionary<string, object> dict)
        {
            if (dict.ContainsKey("options"))
            {
                dict.Remove("options");
            }
            var quiz = JsonHelper.Deserialize<quiz>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (quiz.quizId > 0)
                {
                    var row = dc.quizs.FirstOrDefault(r => r.quizId == quiz.quizId);
                    row.subject = quiz.subject;
                    row.quizstatus = quiz.quizstatus;
                    row.displayOrder = quiz.displayOrder;
                }
                else
                {
                    dc.quizs.Add(quiz);
                }
                dc.SaveChanges();
            }
            return dict;

        }
        [HttpPost]
        public object delete(Dictionary<string, object> dict)
        {
            using (var dc = new DC())
            {
                var id = int.Parse(dict["id"].ToString());
                var quiz = dc.quizs.FirstOrDefault(r => r.quizId == id);
                dc.quizs.Remove(quiz);
                return dc.SaveChanges();
            }

        }

    }

}
