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
    public class OptionController : ApiController
    {
        public object save(Dictionary<string, object> dict)
        {
            var option = JsonHelper.Deserialize<option>(JsonHelper.Serialize(dict));
            using (var dc = new DC())
            {
                if (option.optionId > 0)
                {
                    var row = dc.options.FirstOrDefault(r => r.optionId == option.optionId);
                    row.subject = option.subject;
                }
                else
                {
                    dc.options.Add(option);
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
                var option = dc.options.FirstOrDefault(r => r.optionId == id);
                dc.options.Remove(option);
                return dc.SaveChanges();
            }

        }

        public object Resolve(Dictionary<string, object> dict)
        {
            var optionId = int.Parse(dict["optionId"].ToString());
            var bll = new QuizBLL();
            return bll.CompleteQuiz(optionId, Enums.QuizStatus.已结束);
        }

    }

}
