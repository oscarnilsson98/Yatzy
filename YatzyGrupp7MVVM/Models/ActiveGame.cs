using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace YatzyGrupp7MVVM.Models
{
    public static class ActiveGame
    {

        public static ObservableCollection<Player> Players { get; set; }
        public static int Game_Id { get; set; }
        public static DateTime Start { get; set; }
        public static DateTime End { get; set; }
        public static GameType Gametype { get; set; }
        public static bool Helper { get; set; }


        public static void SetActiveGame(ObservableCollection<Player> players, int game_id, DateTime start, DateTime end, GameType gameType)
        {

            Players = players;
            Game_Id = game_id;
            Start = start;
            End = end;
            Gametype = gameType;

            //Kallar på metod i Scorecard som sätter villkoren för spel med styrd Yatzy

            if (gameType.Name == "Styrd")
            {
                foreach (Player player in Players)
                {
                    player.ScoreCard.StyrdYatzy();
                }

            }



        }      
        

           
            
           

        

        //Kollar om alla scores är valda för varje spelare, returnerar true om det stämmer
        public static bool IsGameFinished()
        {
            int i = 0;


            foreach (var player in Players)
            {

                if (player.ScoreCard.IsAllButtonsSubmitted() == true)
                {
                    i++;
                }

            }

            if (i == Players.Count)
            {
                return true;
            }

            else
            {
                return false;
            }

        }


    }
}
