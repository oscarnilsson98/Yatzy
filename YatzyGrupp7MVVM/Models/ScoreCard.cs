using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace YatzyGrupp7MVVM.Models
{
    public class ScoreCard : INotifyPropertyChanged
    {

        #region Variables
        int selectedScore = 0;
        const string styrd = "Styrd";
        #endregion

        #region Properties
        public int Ones { get; set; }
        public bool Ones_enabled { get; set; } = true;
        public int Twos { get; set; }
        public bool Twos_enabled { get; set; } = true; 
        public int Threes { get; set; }
        public bool Threes_enabled { get; set; } = true;
        public int Fours { get; set; }
        public bool Fours_enabled { get; set; } = true;
        public int Fives { get; set; }
        public bool Fives_enabled { get; set; } = true;
        public int Sixes { get; set; }
        public bool Sixes_enabled { get; set; } = true;
        public int Number_sum
        {
            get
            {                             
                return selectedScore;                                         
            }

            set
            {
                selectedScore = value;
            }
        }
        public int Selected_score { get; set; }
        public bool Number_sum_enabled { get; set; } = true;
        public int Bonus
        {
            get
            {
                int bonusp;

                if (ActiveGame.Gametype.Name == styrd)
                {
                    if (Number_sum >= 42)
                    {
                        bonusp = 50;
                    }

                    else
                    {
                        bonusp = 0;
                    }
                    return bonusp;
                }


                else
                {
                    if (Number_sum >= 63)
                    {
                        bonusp = 50;
                    }
                    else
                    {
                        bonusp = 0;
                    }
                    return bonusp;
                }
                
            }          
        }
        public int One_pair { get; set; }
        public bool One_pair_enabled { get; set; } = true;
        public int Two_pair { get; set; }
        public bool Two_pair_enabled { get; set; } = true;
        public int Three_of_a_kind { get; set; }
        public bool Three_of_a_kind_enabled { get; set; } = true;
        public int Four_of_a_kind { get; set; }
        public bool Four_of_a_kind_enabled { get; set; } = true;
        public int Small_straight { get; set; }
        public bool Small_straight_enabled { get; set; } = true;
        public int Large_straight { get; set; }
        public bool Large_straight_enabled { get; set; } = true;
        public int Full_house { get; set; }
        public bool Full_house_enabled { get; set; } = true;
        public int Chance { get; set; }
        public bool Chance_enabled { get; set; } = true;
        public int Yatzy { get; set; }
        public bool Yatzy_enabled { get; set; } = true;
        public int Total_score
        {
            get
            {
                if (IsAllButtonsSubmitted() == true)
                {
                    return Number_sum + Bonus + One_pair + Two_pair + Three_of_a_kind + Four_of_a_kind + Small_straight + Large_straight + Full_house + Chance + Yatzy;
                }
            
                else
                {
                    return 0;
                }
            }
        
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region Methods
        public void StyrdYatzy()
        {
                Twos_enabled = false;
                Threes_enabled = false;
                Fours_enabled = false;
                Fives_enabled = false;
                Sixes_enabled = false;
                One_pair_enabled = false;
                Two_pair_enabled = false;
                Three_of_a_kind_enabled = false;
                Four_of_a_kind_enabled = false;
                Small_straight_enabled = false;
                Large_straight_enabled = false;
                Full_house_enabled = false;
                Chance_enabled = false;
                Yatzy_enabled = false;
                      
        }
      
        public bool IsAllButtonsSubmitted()
        {
            if (One_pair_enabled == false && Two_pair_enabled == false && Three_of_a_kind_enabled == false &&
                Four_of_a_kind_enabled == false && Small_straight_enabled == false && Large_straight_enabled == false &&
                Full_house_enabled == false && Chance_enabled == false && Yatzy_enabled == false &&
                Ones_enabled == false && Twos_enabled == false && Threes_enabled == false && Fours_enabled == false 
                && Fives_enabled == false && Sixes_enabled == false)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void SelectedScore()
        {
            selectedScore = Selected_score + Number_sum;           
        }
    }

    #endregion
}
