DROP PROCEDURE IF EXISTS wx.proc_copy_game;
CREATE PROCEDURE wx.`proc_copy_game`(_gameId int)
BEGIN

  declare n_gameId int;
  DECLARE txn_error INTEGER DEFAULT 0;
  DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET txn_error=1;
  
  
  create temporary table new_games (select * from games where gameId=_gameId);
  create temporary table new_bets (select * from bets where gameId=_gameId);
  create temporary table new_quizs (select * from quizs where betId in (select betId from new_bets));
  create temporary table new_options (select * from options where quizId in (select quizId from new_quizs));
  
  START TRANSACTION;
  
  update new_bets set copyFromId=betId;
  update new_quizs set copyFromId=quizId;
  update new_options set copyFromId=optionId;
  
  -- insert game
    insert into games (name, displayOrder, gameTypeId, team1_teamId, team2_teamId, copyFromId, createTime) 
      select name, displayOrder, gameTypeId, team1_teamId, team2_teamId, gameId, now() as createTime from new_games;
      
    set n_gameId=@@IDENTITY;
    
  -- insert bets
    insert into bets (name, startTime, endTime, displayOrder, betstatus, gameId, winnerTeam_teamId, copyFromId) 
      select name, startTime, endTime, displayOrder, betstatus, n_gameId, winnerTeam_teamId, betId from new_bets;
    update new_bets as a set betId=(select betId from bets as b where b.gameId=n_gameId and b.copyFromId=a.betId);
    update new_quizs as a set betId=(select betId from new_bets as b where b.copyFromId=a.betId);
      
  -- insert quizs
    insert into quizs (subject, displayOrder, isPublic, quizstatus, betId, copyFromId)
      select subject, displayOrder, isPublic, quizstatus, betId, quizId from new_quizs;
      
    update new_quizs as a set quizId=(select b.quizId from quizs as b where b.betId in(select betId from new_bets) and b.copyFromId=a.quizId);
    update new_options as a set quizId=(select quizId from new_quizs as b where b.copyFromId=a.quizId);
    
  -- insert options
  	insert into options (subject, oddType, fixedOdds, displayOrder, isDel, optionstatus, copyFromId, quizId)
		select subject, oddType, fixedOdds, displayOrder, isDel, optionstatus, copyFromId, quizId from new_options;
  
  select txn_error;
  IF txn_error=1 THEN ROLLBACK; ELSE COMMIT;
  END IF;
  
  drop temporary table if exists new_games;
  drop temporary table if exists new_bets;
  drop temporary table if exists new_quizs;
  drop temporary table if exists new_options;
  
END;

call proc_copy_game(2);