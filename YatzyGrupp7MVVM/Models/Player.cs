using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatzyGrupp7MVVM.Models
{
    public class Player
    {
        public int Player_id { get; set; }
        public string Firstname { get; set; }
        public string Nickname { get; set; }
        public string Lastname { get; set; }
        public int Total_score { get; set; }
        public int Total_wins { get; set; }
        public int Total_games { get; set; }
        public int Game_id { get; set; }
        public string Gametype { get; set; }
        public bool Isbot { get; set; } = false;
        public int Rank { get; set; }

        public ScoreCard scoreCard;
        public ScoreCard ScoreCard
        {
            get
            {
                return scoreCard;
            }
            set
            {
                scoreCard = value;
            }
        }
       
        public Player()
        {
            scoreCard = new ScoreCard();
        }
    }
}
