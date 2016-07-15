using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wx.Enums
{
	public enum BetStatus
	{
		竞猜中 = 0,
		已封盘 = 1,
		已结束 = 2,
		流局 = 3
	}
	public enum RouletteSubmitStatus
	{
		待定 = 0,
		正确 = 1,
		错误 = 2,
		流局 = 3
	}
	public enum QuizStatus
	{
		竞猜中 = 0,
		已封盘 = 1,
		已结束 = 2,
		流局 = 3
	}
	public enum OptionStatus
	{
		待定 = 0,
		正确 = 1,
		错误 = 2,
		流局 = 3
	}
	public enum UserOptionStatus
	{
		竞猜中 = 0,
		成功 = 1,
		失败 = 2,
		流局 = 3
	}
	public enum OddTypes
	{
		Fixed = 0,
		Dynamic = 1
	}
	public enum RedeemStatus
	{
		UnPayed = 0,
		Payed = 1,
		Canceled = 2
	}

}
