using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;


namespace Wx.DAL
{


	public class game_type
	{
		public int game_typeId { get; set; }
		public string enName { get; set; }
		public string cnName { get; set; }
		public int displayOrder { get; set; }

		//fk
		public virtual ICollection<game> games { get; set; }
	}
	public class game
	{
		public int gameId { get; set; }
		public string name { get; set; }
		public int displayOrder { get; set; }
		public DateTime createTime { get; set; }

		public int copyFromId { get; set; }



		//fk
		public int game_typeId { get; set; }
		public virtual game_type game_type { get; set; }

		public virtual team team1 { get; set; }
		public virtual team team2 { get; set; }

		public virtual ICollection<bet> bets { get; set; }

	}
	public class bet
	{
		public int betId { get; set; }
		public string name { get; set; }
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
		public int displayOrder { get; set; }
		public Enums.BetStatus betstatus { get; set; }
		public int copyFromId { get; set; }


		//fk
		public int gameId { get; set; }
		public virtual game game { get; set; }
		public virtual team winnerTeam { get; set; }

		public virtual ICollection<quiz> quizs { get; set; }
	}
	public class team
	{
		public int teamId { get; set; }
		public string name { get; set; }
		public string avatar { get; set; }
	}
	public class quiz
	{
		public int quizId { get; set; }
		public string subject { get; set; }
		public int displayOrder { get; set; }

		public bool isPublic { get; set; } // as a template when ispublic
		public Enums.QuizStatus quizstatus { get; set; }
		public int copyFromId { get; set; }

		//fk
		public int betId { get; set; }
		public virtual bet bet { get; set; }
		public virtual ICollection<option> options { get; set; }

	}


	public class option
	{
		public int optionId { get; set; }
		public string subject { get; set; }
		public Enums.OddTypes oddType { get; set; }
		public double fixedOdds { get; set; }
		public int displayOrder { get; set; }
		public bool isDel { get; set; }
		public Enums.OptionStatus optionstatus { get; set; }
		public int copyFromId { get; set; }
		//fk
		public int quizId { get; set; }
		public virtual quiz quiz { get; set; }
		public virtual ICollection<user_option> user_options { get; set; }

	}
	public class user_option
	{
		public int user_optionId { get; set; }
		//rate
		public double odds { get; set; }
		public double golds { get; set; }
		public int headCount { get; set; }
		public DateTime matchTime { get; set; }

		public DateTime createTime { get; set; }

		public Enums.UserOptionStatus UserOptionStatus { get; set; }


		//fk
		public int userId { get; set; }
		public virtual user user { get; set; }
		public int gameId { get; set; }
		public virtual game game { get; set; }
		public int quizId { get; set; }
		public virtual quiz quiz { get; set; }
		public int optionId { get; set; }
		public virtual option option { get; set; }
	}




}
