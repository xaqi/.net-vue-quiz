using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wx.Common;
using Wx.DAL;
using Wx;
using System.Transactions;

namespace Wx.BLL
{
    public class QuizBLL
    {
        public virtual ReturnMessage Submit(int userId, int optionId, double odds, double golds)
        {
            golds = double.Parse(golds.ToString("0.00"));
            if (golds <= 0)
            {
                return new ReturnMessage { success = false, message = "请至少投注1金币。" };
            }
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var user = dc.users.FirstOrDefault(r => r.userId == userId);
                    if (user.golds < golds)
                    {
                        return new ReturnMessage { success = false, message = "余额不足。" };
                    }
                    var option = dc.options.FirstOrDefault(r => r.optionId == optionId);
                    var quiz = option.quiz;
                    var bet = quiz.bet;
                    var game = bet.game;
                    if (quiz.quizstatus != Enums.QuizStatus.竞猜中)
                    {
                        return new ReturnMessage { success = false, message = "已停止投注。" };

                    }
                    var user_option = new user_option
                    {
                        userId = userId,
                        gameId = game.gameId,
                        quizId = quiz.quizId,
                        optionId = optionId,
                        odds = odds,
                        golds = golds,
                        createTime = DateTime.Now
                    };
                    dc.user_options.Add(user_option);
                    user.golds -= golds;

                    if (user.invite_by_userId != null)
                    {
                        var inviteUser = dc.users.FirstOrDefault(r => r.userId == user.invite_by_userId);
                        if (inviteUser != null)
                        {
                            inviteUser.golds += golds * 0.03;
                            dc.user_golds_records.Add(new user_golds_record
                            {
                                changeAmount = golds * 0.03,
                                amount = user.golds + golds * 0.03,
                                recordTime = DateTime.Now,
                                detail = $"战友竞猜奖励{(user.golds + golds * 0.03).ToString("0.00")}",
                                userId = inviteUser.userId
                            });
                        }
                    }
                    var record = new user_golds_record
                    {
                        amount = user.golds,
                        changeAmount = 0 - golds,
                        userId = userId,
                        detail = string.Format("竞猜 {2}", option.quiz.bet.game.name, option.quiz.bet.name, option.quiz.subject, option.subject, golds),
                        recordTime = DateTime.Now
                    };
                    dc.user_golds_records.Add(record);
                    dc.SaveChanges();
                    tran.Complete();
                }
            }
            return new ReturnMessage { success = true, message = "竞猜成功！" };
        }

        public virtual ReturnMessage CompleteQuiz(int correctOptionId, Enums.QuizStatus quizStatus)
        {
            using (var tran = new TransactionScope())
            {
                using (var dc = new DC())
                {
                    var correctOption = dc.options.FirstOrDefault(r => r.optionId == correctOptionId);
                    var quiz = correctOption.quiz;
                    var otherOptions = quiz.options.Where(r => r.optionId != correctOptionId).ToList();
                    if (otherOptions.Any(r => r.optionstatus == Enums.OptionStatus.正确)) return new ReturnMessage { success = false, message = "结算失败,已有正确答案!" };
                    correctOption.optionstatus = Enums.OptionStatus.正确;
                    var quizId = quiz.quizId;
                    var user_options = dc.user_options.Where(r => r.quizId == quizId).ToList();
                    var allLoserMoney = user_options.Where(r => r.optionId != correctOptionId).Sum(r => r.golds);
                    var allWinnerMoney = user_options.Where(r => r.optionId == correctOptionId).Sum(r => r.golds);

                    var q = (from a in dc.user_options
                             join b in dc.users on a.userId equals b.userId
                             where a.quizId == quizId && a.UserOptionStatus == Enums.UserOptionStatus.竞猜中
                             select new { user_option = a, user = b }).ToList();
                    q.ForEach(uq =>
                        {
                            var isWin = uq.user_option.optionId == correctOptionId;
                            uq.user_option.option.optionstatus = isWin ? Enums.OptionStatus.正确 : Enums.OptionStatus.错误;
                            uq.user_option.UserOptionStatus = isWin ? Enums.UserOptionStatus.成功 : Enums.UserOptionStatus.失败;
                            var changeAmount = ((allLoserMoney * 0.9 + allWinnerMoney) / allWinnerMoney * uq.user_option.golds);
                            uq.user_option.odds = changeAmount / uq.user_option.golds;
                            if (isWin)
                            {
                                uq.user.golds += changeAmount;
                                var record = new user_golds_record
                                {
                                    userId = uq.user.userId,
                                    recordTime = DateTime.Now,
                                    amount = uq.user.golds,
                                    changeAmount = changeAmount,
                                    detail = string.Format("竞猜{0}获胜，金币+{1}", uq.user_option.option.quiz.bet.name, changeAmount.ToString("0.00"))
                                };
                                dc.user_golds_records.Add(record);
                            }
                        });
                    dc.SaveChanges();
                    tran.Complete();
                    return new ReturnMessage { success = true, message = "结算成功！" };
                }
            }

        }
    }
}
