using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using System.Data;
using YatzyGrupp7MVVM.Models;
using System.Collections.ObjectModel;

namespace YatzyGrupp7MVVM.Data
{

	static class DbOperations
	{
		#region GetPlayerNicknames
		public static List<Player> GetPlayerNicknames()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"SELECT nickname FROM player;").ToList();

				return output;
			}
		}
		#endregion

		#region Leaderboards

		// topplist classic
		public static List<Player> GetHighestScoreLatest7DaysClassic()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"WITH cte AS(
													   SELECT game_player.game_id, game_player.player_id, game.started_at, game.gametype_id, 
													   SUM(game_player.score) OVER(PARTITION BY game_player.player_id) as Total_Score
													   FROM game_player
													   INNER JOIN game ON game.gametype_id = game.gametype_id and game.started_at = game.started_at AND game_player.game_id = game.game_id 
													   INNER JOIN player ON game_player.player_id = player.player_id
													   WHERE gametype_id = 1 AND started_at >= now() - interval '7 day')
	   
													   SELECT *
													   FROM
													   (SELECT distinct(cte.player_id), cte.Total_Score, firstname, nickname, lastname,
													   DENSE_RANK () OVER (ORDER BY cte.Total_Score DESC) as rank  FROM cte
													   INNER JOIN player on cte.player_id = player.player_id
													   WHERE total_score is not null
													   ORDER BY rank) subquery where rank <= 5;").ToList();

				return output;
			}
		}

		// topplist Styrd
		public static List<Player> GetHighestScoreLatest7DaysStyrd()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"WITH cte AS(
													   SELECT game_player.game_id, game_player.player_id, game.started_at, game.gametype_id, 
													   SUM(game_player.score) OVER(PARTITION BY game_player.player_id) as Total_Score
													   FROM game_player
													   INNER JOIN game ON game.gametype_id = game.gametype_id and game.started_at = game.started_at AND game_player.game_id = game.game_id 
													   INNER JOIN player ON game_player.player_id = player.player_id
													   WHERE gametype_id = 2 AND started_at >= now() - interval '7 day')
	   
													   SELECT *
													   FROM
													   (SELECT distinct(cte.player_id), cte.Total_Score, firstname, nickname, lastname,
													   DENSE_RANK () OVER (ORDER BY cte.Total_Score DESC) as rank  FROM cte
													   INNER JOIN player on cte.player_id = player.player_id
													   WHERE total_score is not null
													   ORDER BY rank) subquery where rank <= 5;").ToList();

				return output;
			}
		}

		// flest spelade klassisk
		public static List<Player> GetMostPlayed()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"WITH CTE AS (SELECT
														player_id
														, COUNT(player_id) AS Total_Games
	
														FROM game_player
														GROUP BY player_id
														ORDER BY Total_Games DESC)

														SELECT *
														FROM
														(SELECT distinct(cte.player_id), cte.Total_Games, firstname, nickname, lastname,
														 DENSE_RANK() OVER(ORDER BY Total_Games DESC) AS rank
														FROM CTE
														INNER JOIN player on cte.player_id = player.player_id
														ORDER BY RANK)
														subquery where rank <= 5;").ToList();
				return output;
			}
		}

		public static List<Player> GetMostWins()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"
													   with cte AS(
														SELECT *,COUNT(*) OVER(PARTITION BY player_id) as Total_Wins FROM
	
														(SELECT *, 
														MAX(score) OVER(PARTITION BY game_id) as win
													   
														FROM game_player
														WHERE score IS NOT null) subquery WHERE score = win)
	
													 
													   SELECT * FROM
													   (SELECT distinct(cte.player_id), cte.Total_Wins, firstname, nickname, lastname, 
													   DENSE_RANK() OVER(ORDER BY Total_Wins DESC) AS rank
													   FROM CTE 
													   INNER JOIN PLAYER on cte.player_id = player.player_id
													   ORDER BY RANK) subquery WHERE rank <= 5;").ToList();

				  return output;
			}
		}

		

		#endregion

		#region GetGameType

		public static List <GameType> GetGametypes()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<GameType>($@"SELECT gametype_id, gametype,name FROM gametype").ToList();

				return output;
			}
		}

		#endregion 



		#region AddNewplayer

		public static void AddNewPlayer(string firstname, string nickname, string lastname)
		{
   
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"INSERT INTO player (firstname, nickname, lastname) VALUES (@Firstname, @Nickname, @Lastname)", new { Firstname = firstname, Nickname = nickname, Lastname = lastname });

			}
		}


		#endregion

		#region GetPlayerNickname
		public static Player GetPlayerByNickname(string nickname)
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.QuerySingle<Player>($@"SELECT * FROM PLAYER WHERE NICKNAME = @Nickname;", new { Nickname = nickname });

				return output;
			}
		}
		#endregion

		#region GetPlayersStatus
		public static List<Player> PlayersInActiveGame()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"SELECT game_player.game_id, game.gametype_id, gametype.name as Gametype, game_player.player_id, nickname, started_at, ended_at 
														FROM game_player 
														INNER JOIN game ON game_player.game_id = game.game_id
														INNER JOIN player ON game_player.player_id = player.player_id
														INNER JOIN gametype ON game.gametype_id = gametype.gametype_id AND gametype.name = gametype.name
														WHERE ended_at IS NULL ORDER BY 1;").ToList();

				return output;
			}
		}

		public static List<Player> GetAvailablePlayers()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.Query<Player>($@"WITH cte AS(
													   SELECT game_player.game_id, game_player.player_id, started_at, ended_at 
													   FROM game_player 
													   INNER JOIN game ON game_player.game_id = game.game_id 
													   WHERE ended_at IS NULL 
													   order BY player_id)

													   SELECT * FROM player
													   WHERE player_id NOT IN (
													   SELECT cte.player_id FROM cte
													   ) order by nickname ASC;").ToList();
				return output;
			}
		}
		#endregion

		#region GameQuerys
		public static void CreateNewGameId(int gametype)
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"INSERT INTO game(gametype_id) VALUES (@Gametype)", new { Gametype = gametype });
			   
			}
		}

		public static Game GetGameId()
		{
			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				var output = connection.QuerySingle<Game>($@"SELECT game_id FROM game 
														 WHERE game_id = (SELECT MAX(game_id) 
														 FROM game);");

				return output;
			}
		}

		public static void StartNewGame(int gameid, int playerid)
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"INSERT INTO game_player(game_id, player_id) VALUES (@Gameid, @Playerid)", new { Gameid = gameid, Playerid = playerid });

			}
		}
		public static void SetScore(int score, int gameid, int playerid)
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"UPDATE game_player set score = @Score WHERE player_id = @Playerid and game_id = @Gameid", new { Score = score, Gameid = gameid, Playerid = playerid });

			}
		}
		public static void EndGame(int gameid, DateTime endtime )
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"UPDATE game set ended_at = @Endtime WHERE game_id = @Gameid", new { Gameid = gameid, Endtime = endtime });

			}
		}

		public static void ExitGame(int gameid)
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"DELETE FROM game_player WHERE game_id = @Gameid", new { Gameid = gameid});

			}
		}

		public static void EndGameOlderThan2H()
		{

			using (IDbConnection connection = new NpgsqlConnection(ConnectionString.ConnVal("dbConn")))
			{
				connection.Execute($@"WITH cte AS(SELECT game_id, started_at, ended_at FROM game WHERE ended_at IS NULL AND started_at <= now() - interval '2 hour' ORDER BY 1)
									  DELETE FROM game_player USING cte WHERE cte.game_id = game_player.game_id; ");

			}
		}
		#endregion

	  


	}
}
