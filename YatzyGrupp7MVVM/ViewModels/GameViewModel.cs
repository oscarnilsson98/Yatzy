using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using YatzyGrupp7MVVM.Models;
using System.Windows.Input;
using YatzyGrupp7MVVM.Services;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using YatzyGrupp7MVVM.Views;
using System.Threading;
using System.Windows.Threading;
using YatzyGrupp7MVVM.Data;
using NAudio.Wave;

namespace YatzyGrupp7MVVM.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        #region Variables
        GameEngine gameEngine = new GameEngine();

        Dices[] dices = new Dices[5] { new Dices(), new Dices(), new Dices(), new Dices(), new Dices(), };

        const string styrd = "Styrd";
        int currentPlayer = 0;
        int throws = 0;
        int doubleEndBackstop = 0;

        #endregion

        #region Windowload
        public void LoadGame()
        {
            if (ActiveGame.Players != null)
            {
                firsttimehelp = true;
                throws = 0;
                currentPlayer = 0;
                BotActionlabel = "";
                doubleEndBackstop = 0;
                Music();
                HideScorecard();
                HighlightActivePlayer();
                ResetDices();
                HideDices();

                RaisePropertyChangedP1();
                RaisePropertyChangedP2();
                RaisePropertyChangedP3();
                RaisePropertyChangedP4();

                Helper();

                for (int i = 0; i < ActiveGame.Players.Count; i++)
                {
                    string s = ActiveGame.Players[i].Nickname.ToLower();
                    if (s.StartsWith("bot"))
                    {
                        ActiveGame.Players[i].Isbot = true;
                    }
                }


                if (ActiveGame.Players[currentPlayer].Isbot == true)
                {
                    BotTest();
                }
                else
                {
                    playerChangedString = $"Nu är det {ActiveGame.Players[currentPlayer].Nickname}'s tur";

                    ThrowsAmount();

                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedString"));
                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));

                    SetTimerUpdatePlayer();
                }
            }
        }


        private void AssignScoreCardData()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("ScoreCardData"));
            HighlightActivePlayer();
        }

        #endregion

        #region Properties       
        public ObservableCollection<Player> ScoreCardData
        {
            get
            {
                return ActiveGame.Players;
            }
            set
            {
                ActiveGame.Players = value;
                ClickCommandThrow.RaiseCanExecuteChanged();
                OnPropertyChanged(new PropertyChangedEventArgs("ScoreCardData"));
            }
        }

        private string playerChangedVisible = "Visible";
        public string PlayerChangedVisible
        {
            get { return playerChangedVisible; }
            set
            {
                playerChangedVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));
            }
        }

        private string playerChangedString;
        public string PlayerChangedString
        {
            get { return playerChangedString; }
            set
            {
                playerChangedString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedString"));
            }
        }

        private int highlightP1;
        public int HighlightP1
        {
            get
            {
                return highlightP1;
            }
            set
            {
                highlightP1 = value;
            }
        }

        private int highlightP2;
        public int HighlightP2
        {
            get
            {
                return highlightP2;
            }
            set
            {
                highlightP2 = value;
            }
        }

        private int highlightP3;
        public int HighlightP3
        {
            get
            {
                return highlightP3;
            }
            set
            {
                highlightP3 = value;
            }
        }

        private int highlightP4;
        public int HighlightP4
        {
            get
            {
                return highlightP4;
            }
            set
            {
                highlightP4 = value;
            }
        }

        #endregion

        #region Lock Buttons Methods

        private void LockOnes()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Ones_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Twos_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("TwosEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Ones;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();


            SetActivePlayer();
        }


        private void LockTwos()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Twos_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Threes_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("ThreesEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Twos;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();

            SetActivePlayer();
        }

        private void LockThrees()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Threes_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Fours_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("FoursEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Threes;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();

            SetActivePlayer();
        }

        private void LockFours()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Fours_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Fives_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("FivesEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Fours;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();

            SetActivePlayer();
        }

        private void LockFives()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Fives_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Sixes_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("SixesEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Fives;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();


            SetActivePlayer();
        }

        private void LockSixes()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Sixes_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Threes_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("ThreesEnabled"));
            }

            ActiveGame.Players[currentPlayer].scoreCard.Selected_score = ActiveGame.Players[currentPlayer].scoreCard.Sixes;
            ActiveGame.Players[currentPlayer].scoreCard.SelectedScore();

            SetActivePlayer();
        }

        private void LockOnePair()
        {
            ActiveGame.Players[currentPlayer].scoreCard.One_pair_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Two_pair_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("TwoPairEnabled"));
            }

            SetActivePlayer();
        }

        private void LockTwoPairs()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Two_pair_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Three_of_a_kind_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("ThreeOfAKindEnabled"));
            }

            SetActivePlayer();
        }

        private void LockThreeOfAKind()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Three_of_a_kind_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("FourOfAKindEnabled"));
            }

            SetActivePlayer();
        }

        private void LockFourOfAKind()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Small_straight_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("SmallStraightEnabled"));
            }

            SetActivePlayer();
        }

        private void LockSmallStraight()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Small_straight_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Large_straight_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("LargeStraightEnabled"));
            }

            SetActivePlayer();
        }

        private void LockLargeStraight()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Large_straight_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("FullHouseEnabled"));
            }

            SetActivePlayer();
        }

        private void LockFullHouse()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Chance_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("ChanceEnabled"));
            }

            SetActivePlayer();
        }

        private void LockChance()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Chance_enabled = false;

            if (ActiveGame.Gametype.Name == styrd)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled = true;
                ActiveGame.Players[currentPlayer].scoreCard.OnPropertyChanged(new PropertyChangedEventArgs("YatzyEnabled"));
            }

            SetActivePlayer();
        }

        private void LockYatzy()
        {
            ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled = false;
            SetActivePlayer();
        }

        #endregion

        #region Lock Buttons Commands

        #region Player 1

        #region 1-6
        private RelayCommand _lockOnesP1;
        public RelayCommand LockOnesP1
        {
            get
            {
                return _lockOnesP1 ?? (_lockOnesP1 = new RelayCommand(w => LockOnes(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockTwosP1;
        public RelayCommand LockTwosP1
        {
            get
            {
                return _lockTwosP1 ?? (_lockTwosP1 = new RelayCommand(w => LockTwos(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockThreesP1;
        public RelayCommand LockThreesP1
        {
            get
            {
                return _lockThreesP1 ?? (_lockThreesP1 = new RelayCommand(w => LockThrees(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockFoursP1;
        public RelayCommand LockFoursP1
        {
            get
            {
                return _lockFoursP1 ?? (_lockFoursP1 = new RelayCommand(w => LockFours(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockFivesP1;
        public RelayCommand LockFivesP1
        {
            get
            {
                return _lockFivesP1 ?? (_lockFivesP1 = new RelayCommand(w => LockFives(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockSixesP1;
        public RelayCommand LockSixesP1
        {
            get
            {
                return _lockSixesP1 ?? (_lockSixesP1 = new RelayCommand(w => LockSixes(), w => IsButtonEnabledPlayer1()));
            }
        }

        #endregion

        #region Rest

        private RelayCommand _lockOnePairP1;
        public RelayCommand LockOnePairP1
        {
            get
            {
                return _lockOnePairP1 ?? (_lockOnePairP1 = new RelayCommand(w => LockOnePair(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockTwoPairP1;
        public RelayCommand LockTwoPairP1
        {
            get
            {
                return _lockTwoPairP1 ?? (_lockTwoPairP1 = new RelayCommand(w => LockTwoPairs(), w => IsButtonEnabledPlayer1()));
            }
        }



        private RelayCommand _lockThreeOfAkindP1;
        public RelayCommand LockThreeOfAkindP1
        {
            get
            {
                return _lockThreeOfAkindP1 ?? (_lockThreeOfAkindP1 = new RelayCommand(w => LockThreeOfAKind(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockFourOfAKindP1;
        public RelayCommand LockFourOfAKindP1
        {
            get
            {
                return _lockFourOfAKindP1 ?? (_lockFourOfAKindP1 = new RelayCommand(w => LockFourOfAKind(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockSmallStraightP1;
        public RelayCommand LockSmallStraightP1
        {
            get
            {
                return _lockSmallStraightP1 ?? (_lockSmallStraightP1 = new RelayCommand(w => LockSmallStraight(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockLargeStraightP1;
        public RelayCommand LockLargeStraightP1
        {
            get
            {
                return _lockLargeStraightP1 ?? (_lockLargeStraightP1 = new RelayCommand(w => LockLargeStraight(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockFullHouseP1;
        public RelayCommand LockFullHouseP1
        {
            get
            {
                return _lockFullHouseP1 ?? (_lockFullHouseP1 = new RelayCommand(w => LockFullHouse(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockChanceP1;
        public RelayCommand LockChanceP1
        {
            get
            {
                return _lockChanceP1 ?? (_lockChanceP1 = new RelayCommand(w => LockChance(), w => IsButtonEnabledPlayer1()));
            }
        }

        private RelayCommand _lockYatzyP1;
        public RelayCommand LockYatzyP1
        {
            get
            {
                return _lockYatzyP1 ?? (_lockYatzyP1 = new RelayCommand(w => LockYatzy(), w => IsButtonEnabledPlayer1()));
            }
        }


        #endregion

        #endregion

        #region Player 2

        #region 1-6
        private RelayCommand _lockOnesP2;
        public RelayCommand LockOnesP2
        {
            get
            {
                return _lockOnesP2 ?? (_lockOnesP2 = new RelayCommand(w => LockOnes(), w => IsButtonEnabledPlayer2()));
            }

            set
            {
                _lockOnesP2 = value;
            }
        }

        private RelayCommand _lockTwosP2;
        public RelayCommand LockTwosP2
        {
            get
            {
                return _lockTwosP2 ?? (_lockTwosP2 = new RelayCommand(w => LockTwos(), w => IsButtonEnabledPlayer2()));
            }
        }

        private RelayCommand _lockThreesP2;
        public RelayCommand LockThreesP2
        {
            get
            {
                return _lockThreesP2 ?? (_lockThreesP2 = new RelayCommand(w => LockThrees(), w => IsButtonEnabledPlayer2()));
            }
        }

        private RelayCommand _lockFoursP2;
        public RelayCommand LockFoursP2
        {
            get
            {
                return _lockFoursP2 ?? (_lockFoursP2 = new RelayCommand(w => LockFours(), w => IsButtonEnabledPlayer2()));
            }
        }

        private RelayCommand _lockFivesP2;
        public RelayCommand LockFivesP2
        {
            get
            {
                return _lockFivesP2 ?? (_lockFivesP2 = new RelayCommand(w => LockFives(), w => IsButtonEnabledPlayer2()));
            }
        }

        private RelayCommand _lockSixesP2;
        public RelayCommand LockSixesP2
        {
            get
            {
                return _lockSixesP2 ?? (_lockSixesP2 = new RelayCommand(w => LockSixes(), w => IsButtonEnabledPlayer2()));
            }
        }


        #endregion

        #region Rest

        private RelayCommand _lockOnePairP2;
        public RelayCommand LockOnePairP2
        {
            get
            {
                return _lockOnePairP2 ?? (_lockOnePairP2 = new RelayCommand(w => LockOnePair(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockTwoPairP2;
        public RelayCommand LockTwoPairP2
        {
            get
            {
                return _lockTwoPairP2 ?? (_lockTwoPairP2 = new RelayCommand(w => LockTwoPairs(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockThreeOfAkindP2;
        public RelayCommand LockThreeOfAkindP2
        {
            get
            {
                return _lockThreeOfAkindP2 ?? (_lockThreeOfAkindP2 = new RelayCommand(w => LockThreeOfAKind(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockFourOfAKindP2;
        public RelayCommand LockFourOfAKindP2
        {
            get
            {
                return _lockFourOfAKindP2 ?? (_lockFourOfAKindP2 = new RelayCommand(w => LockFourOfAKind(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockSmallStraightP2;
        public RelayCommand LockSmallStraightP2
        {
            get
            {
                return _lockSmallStraightP2 ?? (_lockSmallStraightP2 = new RelayCommand(w => LockSmallStraight(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockLargeStraightP2;
        public RelayCommand LockLargeStraightP2
        {
            get
            {
                return _lockLargeStraightP2 ?? (_lockLargeStraightP2 = new RelayCommand(w => LockLargeStraight(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockFullHouseP2;
        public RelayCommand LockFullHouseP2
        {
            get
            {
                return _lockFullHouseP2 ?? (_lockFullHouseP2 = new RelayCommand(w => LockFullHouse(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockChanceP2;
        public RelayCommand LockChanceP2
        {
            get
            {
                return _lockChanceP2 ?? (_lockChanceP2 = new RelayCommand(w => LockChance(), w => IsButtonEnabledPlayer2()));
            }
        }



        private RelayCommand _lockYatzyP2;
        public RelayCommand LockYatzyP2
        {
            get
            {
                return _lockYatzyP2 ?? (_lockYatzyP2 = new RelayCommand(w => LockYatzy(), w => IsButtonEnabledPlayer2()));
            }
        }

        #endregion

        #endregion

        #region Player 3

        #region 1-6
        private RelayCommand _lockOnesP3;
        public RelayCommand LockOnesP3
        {
            get
            {
                return _lockOnesP3 ?? (_lockOnesP3 = new RelayCommand(w => LockOnes(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockTwosP3;
        public RelayCommand LockTwosP3
        {
            get
            {
                return _lockTwosP3 ?? (_lockTwosP3 = new RelayCommand(w => LockTwos(), w => IsButtonEnabledPlayer3()));
            }
        }


        private RelayCommand _lockThreesP3;
        public RelayCommand LockThreesP3
        {
            get
            {
                return _lockThreesP3 ?? (_lockThreesP3 = new RelayCommand(w => LockThrees(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockFoursP3;
        public RelayCommand LockFoursP3
        {
            get
            {
                return _lockFoursP3 ?? (_lockFoursP3 = new RelayCommand(w => LockFours(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockFivesP3;
        public RelayCommand LockFivesP3
        {
            get
            {
                return _lockFivesP3 ?? (_lockFivesP3 = new RelayCommand(w => LockFives(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockSixesP3;
        public RelayCommand LockSixesP3
        {
            get
            {
                return _lockSixesP3 ?? (_lockSixesP3 = new RelayCommand(w => LockSixes(), w => IsButtonEnabledPlayer3()));
            }
        }

        #endregion

        #region Rest

        private RelayCommand _lockOnePairP3;
        public RelayCommand LockOnePairP3
        {
            get
            {
                return _lockOnePairP3 ?? (_lockOnePairP3 = new RelayCommand(w => LockOnePair(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockTwoPairP3;
        public RelayCommand LockTwoPairP3
        {
            get
            {
                return _lockTwoPairP3 ?? (_lockTwoPairP3 = new RelayCommand(w => LockTwoPairs(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockThreeOfAkindP3;
        public RelayCommand LockThreeOfAkindP3
        {
            get
            {
                return _lockThreeOfAkindP3 ?? (_lockThreeOfAkindP3 = new RelayCommand(w => LockThreeOfAKind(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockFourOfAKindP3;
        public RelayCommand LockFourOfAKindP3
        {
            get
            {
                return _lockFourOfAKindP3 ?? (_lockFourOfAKindP3 = new RelayCommand(w => LockFourOfAKind(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockSmallStraightP3;
        public RelayCommand LockSmallStraightP3
        {
            get
            {
                return _lockSmallStraightP3 ?? (_lockSmallStraightP3 = new RelayCommand(w => LockSmallStraight(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockLargeStraightP3;
        public RelayCommand LockLargeStraightP3
        {
            get
            {
                return _lockLargeStraightP3 ?? (_lockLargeStraightP3 = new RelayCommand(w => LockLargeStraight(), w => IsButtonEnabledPlayer3()));
            }
        }



        private RelayCommand _lockFullHouseP3;
        public RelayCommand LockFullHouseP3
        {
            get
            {
                return _lockFullHouseP3 ?? (_lockFullHouseP3 = new RelayCommand(w => LockFullHouse(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockChanceP3;
        public RelayCommand LockChanceP3
        {
            get
            {
                return _lockChanceP3 ?? (_lockChanceP3 = new RelayCommand(w => LockChance(), w => IsButtonEnabledPlayer3()));
            }
        }

        private RelayCommand _lockYatzyP3;
        public RelayCommand LockYatzyP3
        {
            get
            {
                return _lockYatzyP3 ?? (_lockYatzyP3 = new RelayCommand(w => LockYatzy(), w => IsButtonEnabledPlayer3()));
            }
        }

        #endregion

        #endregion

        #region Player 4

        #region 1-6
        private RelayCommand _lockOnesP4;
        public RelayCommand LockOnesP4
        {
            get
            {
                return _lockOnesP4 ?? (_lockOnesP4 = new RelayCommand(w => LockOnes(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockTwosP4;
        public RelayCommand LockTwosP4
        {
            get
            {
                return _lockTwosP4 ?? (_lockTwosP4 = new RelayCommand(w => LockTwos(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockThreesP4;
        public RelayCommand LockThreesP4
        {
            get
            {
                return _lockThreesP4 ?? (_lockThreesP4 = new RelayCommand(w => LockThrees(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockFoursP4;
        public RelayCommand LockFoursP4
        {
            get
            {
                return _lockFoursP4 ?? (_lockFoursP4 = new RelayCommand(w => LockFours(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockFivesP4;
        public RelayCommand LockFivesP4
        {
            get
            {
                return _lockFivesP4 ?? (_lockFivesP4 = new RelayCommand(w => LockFives(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockSixesP4;
        public RelayCommand LockSixesP4
        {
            get
            {
                return _lockSixesP4 ?? (_lockSixesP4 = new RelayCommand(w => LockSixes(), w => IsButtonEnabledPlayer4()));
            }
        }

        #endregion

        #region Rest

        private RelayCommand _lockOnePairP4;
        public RelayCommand LockOnePairP4
        {
            get
            {
                return _lockOnePairP4 ?? (_lockOnePairP4 = new RelayCommand(w => LockOnePair(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockTwoPairP4;
        public RelayCommand LockTwoPairP4
        {
            get
            {
                return _lockTwoPairP4 ?? (_lockTwoPairP4 = new RelayCommand(w => LockTwoPairs(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockThreeOfAkindP4;
        public RelayCommand LockThreeOfAkindP4
        {
            get
            {
                return _lockThreeOfAkindP4 ?? (_lockThreeOfAkindP4 = new RelayCommand(w => LockThreeOfAKind(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockFourOfAKindP4;
        public RelayCommand LockFourOfAKindP4
        {
            get
            {
                return _lockFourOfAKindP4 ?? (_lockFourOfAKindP4 = new RelayCommand(w => LockFourOfAKind(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockSmallStraightP4;
        public RelayCommand LockSmallStraightP4
        {
            get
            {
                return _lockSmallStraightP4 ?? (_lockSmallStraightP4 = new RelayCommand(w => LockSmallStraight(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockLargeStraightP4;
        public RelayCommand LockLargeStraightP4
        {
            get
            {
                return _lockLargeStraightP4 ?? (_lockLargeStraightP4 = new RelayCommand(w => LockLargeStraight(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockFullHouseP4;
        public RelayCommand LockFullHouseP4
        {
            get
            {
                return _lockFullHouseP4 ?? (_lockFullHouseP4 = new RelayCommand(w => LockFullHouse(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockChanceP4;
        public RelayCommand LockChanceP4
        {
            get
            {
                return _lockChanceP4 ?? (_lockChanceP4 = new RelayCommand(w => LockChance(), w => IsButtonEnabledPlayer4()));
            }
        }

        private RelayCommand _lockYatzyP4;
        public RelayCommand LockYatzyP4
        {
            get
            {
                return _lockYatzyP4 ?? (_lockYatzyP4 = new RelayCommand(w => LockYatzy(), w => IsButtonEnabledPlayer4()));
            }
        }

        #endregion

        #endregion

        #region Booleans, activates buttons for active player

        private bool IsButtonEnabledPlayer1()
        {
            if (currentPlayer == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsButtonEnabledPlayer2()
        {
            if (currentPlayer == 1)
            {
                return true;
            }

            return false;
        }

        private bool IsButtonEnabledPlayer3()
        {
            if (currentPlayer == 2)
            {
                return true;
            }

            return false;
        }

        private bool IsButtonEnabledPlayer4()
        {
            if (currentPlayer == 3)
            {
                return true;
            }

            return false;
        }

        #endregion

        # region RaisePropertyChanged, reactivate buttons for active player

        private void RaisePropertyChangedP1()
        {
            LockOnesP1.RaiseCanExecuteChanged();
            LockTwosP1.RaiseCanExecuteChanged();
            LockThreesP1.RaiseCanExecuteChanged();
            LockFoursP1.RaiseCanExecuteChanged();
            LockFivesP1.RaiseCanExecuteChanged();
            LockSixesP1.RaiseCanExecuteChanged();
            LockOnePairP1.RaiseCanExecuteChanged();
            LockTwoPairP1.RaiseCanExecuteChanged();
            LockThreeOfAkindP1.RaiseCanExecuteChanged();
            LockFourOfAKindP1.RaiseCanExecuteChanged();
            LockSmallStraightP1.RaiseCanExecuteChanged();
            LockLargeStraightP1.RaiseCanExecuteChanged();
            LockFullHouseP1.RaiseCanExecuteChanged();
            LockChanceP1.RaiseCanExecuteChanged();
            LockYatzyP1.RaiseCanExecuteChanged();
        }

        private void RaisePropertyChangedP2()
        {
            LockOnesP2.RaiseCanExecuteChanged();
            LockTwosP2.RaiseCanExecuteChanged();
            LockThreesP2.RaiseCanExecuteChanged();
            LockFoursP2.RaiseCanExecuteChanged();
            LockFivesP2.RaiseCanExecuteChanged();
            LockSixesP2.RaiseCanExecuteChanged();
            LockOnePairP2.RaiseCanExecuteChanged();
            LockTwoPairP2.RaiseCanExecuteChanged();
            LockThreeOfAkindP2.RaiseCanExecuteChanged();
            LockFourOfAKindP2.RaiseCanExecuteChanged();
            LockSmallStraightP2.RaiseCanExecuteChanged();
            LockLargeStraightP2.RaiseCanExecuteChanged();
            LockFullHouseP2.RaiseCanExecuteChanged();
            LockChanceP2.RaiseCanExecuteChanged();
            LockYatzyP2.RaiseCanExecuteChanged();
        }

        private void RaisePropertyChangedP3()
        {
            LockOnesP3.RaiseCanExecuteChanged();
            LockTwosP3.RaiseCanExecuteChanged();
            LockThreesP3.RaiseCanExecuteChanged();
            LockFoursP3.RaiseCanExecuteChanged();
            LockFivesP3.RaiseCanExecuteChanged();
            LockSixesP3.RaiseCanExecuteChanged();
            LockOnePairP3.RaiseCanExecuteChanged();
            LockTwoPairP3.RaiseCanExecuteChanged();
            LockThreeOfAkindP3.RaiseCanExecuteChanged();
            LockFourOfAKindP3.RaiseCanExecuteChanged();
            LockSmallStraightP3.RaiseCanExecuteChanged();
            LockLargeStraightP3.RaiseCanExecuteChanged();
            LockFullHouseP3.RaiseCanExecuteChanged();
            LockChanceP3.RaiseCanExecuteChanged();
            LockYatzyP3.RaiseCanExecuteChanged();
        }

        private void RaisePropertyChangedP4()
        {
            LockOnesP4.RaiseCanExecuteChanged();
            LockTwosP4.RaiseCanExecuteChanged();
            LockThreesP4.RaiseCanExecuteChanged();
            LockFoursP4.RaiseCanExecuteChanged();
            LockFivesP4.RaiseCanExecuteChanged();
            LockSixesP4.RaiseCanExecuteChanged();
            LockOnePairP4.RaiseCanExecuteChanged();
            LockTwoPairP4.RaiseCanExecuteChanged();
            LockThreeOfAkindP4.RaiseCanExecuteChanged();
            LockFourOfAKindP4.RaiseCanExecuteChanged();
            LockSmallStraightP4.RaiseCanExecuteChanged();
            LockLargeStraightP4.RaiseCanExecuteChanged();
            LockFullHouseP4.RaiseCanExecuteChanged();
            LockChanceP4.RaiseCanExecuteChanged();
            LockYatzyP4.RaiseCanExecuteChanged();
        }

        #endregion

        #region Reset to 0 on unlocked
        private void ResetTo0()
        {
            if (ActiveGame.Players[currentPlayer].ScoreCard.Ones_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Ones = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Twos_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Twos = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Threes_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Threes = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Fours_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Fours = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Fives_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Fives = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Sixes_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Sixes = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.One_pair_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.One_pair = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Two_pair_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Two_pair = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Three_of_a_kind_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Three_of_a_kind = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Four_of_a_kind_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Four_of_a_kind = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Small_straight_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Small_straight = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Large_straight_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Large_straight = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Full_house_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Full_house = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Chance_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Chance = 0;
            }
            if (ActiveGame.Players[currentPlayer].ScoreCard.Yatzy_enabled == true)
            {
                ActiveGame.Players[currentPlayer].ScoreCard.Yatzy = 0;
            }
            ScoreCardData = ActiveGame.Players;
            PropertyChanged(this, new PropertyChangedEventArgs("ScoreCardData"));

        }
        #endregion

        #endregion

        #region Highlight Activeplayer
        private void HighlightActivePlayer()
        {
            if (currentPlayer == 0)
            {
                HighlightP1 = 20;
                HighlightP2 = 14;
                HighlightP3 = 14;
                HighlightP4 = 14;
            }
            else if (currentPlayer == 1)
            {
                highlightP1 = 14;
                highlightP2 = 20;
                highlightP3 = 14;
                highlightP4 = 14;
            }
            else if (currentPlayer == 2)
            {
                highlightP1 = 14;
                highlightP2 = 14;
                highlightP3 = 20;
                highlightP4 = 14;
            }
            else if (currentPlayer == 3)
            {
                highlightP1 = 14;
                highlightP2 = 14;
                highlightP3 = 14;
                highlightP4 = 20;
            }

            OnPropertyChanged(new PropertyChangedEventArgs("HighlightP1"));
            OnPropertyChanged(new PropertyChangedEventArgs("HighlightP2"));
            OnPropertyChanged(new PropertyChangedEventArgs("HighlightP3"));
            OnPropertyChanged(new PropertyChangedEventArgs("HighlightP4"));
        }
        #endregion

        #region Hide ScoreCard
        private void HideScorecard()
        {
            if (ActiveGame.Players.Count == 2)
            {
                visibleP3 = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP3"));

                visibleP4 = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP4"));
            }
            else if (ActiveGame.Players.Count == 3)
            {
                visibleP3 = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP3"));

                visibleP4 = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP4"));
            }
            else
            {
                visibleP3 = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP3"));

                visibleP4 = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleP4"));
            }
        }

        #region Bindings

        private string visibleP3 = "Visible";
        public string VisibleP3
        {
            get
            {
                return visibleP3;
            }
            set
            {
                visibleP3 = value;
            }
        }

        private string visibleP4 = "Visible";
        public string VisibleP4
        {
            get
            {
                return visibleP4;
            }
            set
            {
                visibleP4 = value;
            }
        }

        #endregion

        #endregion

        #region Throw Dice Command
        private RelayCommand _clickCommandThrow;
        public RelayCommand ClickCommandThrow
        {
            get
            {

                return _clickCommandThrow ?? (_clickCommandThrow = new RelayCommand(w => ThrowDice(), w => IsMaxThrowsUsed()));
            }

            set
            {
                _clickCommandThrow = value;
            }
        }

        private bool throwBtnActive = false;

        private void ThrowDice()
        {
            if (throws < 3)
            {
                throwBtnActive = true;
                throws++;
                SetTimerAni();
                ThrowsAmount();

                if (throws == 3)
                {
                    throwBtnActive = false;
                    ClickCommandThrow.RaiseCanExecuteChanged();
                    SetTimerAni();
                }
            }


        }

        private bool IsMaxThrowsUsed()
        {
            HideScorecard();
            HighlightActivePlayer();

            if (throws == 3 || throwBtnActive)
            {

                return false;

            }

            else
            {
                return true;
            }

        }

        private void ThrowDiceAfterAnimationMethod()
        {
            AssignScoreCardData();

            SetDiceImage();
            throwBtnActive = false;

            Helper();

        }

        private void ThrowDiceDuringAnimationMethod()
        {
            RollDice();

            CalcKind();
        }
        #endregion

        #region Roll
        private void RollDice()
        {
            for (int i = 0; i < dices.Count(); i++)
            {
                if (dices[i].Hold == false)
                {
                    dices[i].Rolled_number = gameEngine.ThrowDice();
                }

            }
        }
        #endregion

        #region Calculate Dices
        private void CalcKind()
        {
            if (ActiveGame.Players[currentPlayer].scoreCard.Ones_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Ones = gameEngine.CalcXofAKind(dices, 1);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Twos_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Twos = gameEngine.CalcXofAKind(dices, 2);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Threes_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Threes = gameEngine.CalcXofAKind(dices, 3);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Fours_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Fours = gameEngine.CalcXofAKind(dices, 4);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Fives_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Fives = gameEngine.CalcXofAKind(dices, 5);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Sixes_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Sixes = gameEngine.CalcXofAKind(dices, 6);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.One_pair_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.One_pair = gameEngine.CalcOnePair(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Two_pair_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Two_pair = gameEngine.CalcTwoPair(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Three_of_a_kind_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Three_of_a_kind = gameEngine.CalcThreeOfAkind(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind = gameEngine.CalcFourOfAkind(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Small_straight_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Small_straight = gameEngine.CalcSmallStraight(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Large_straight_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Large_straight = gameEngine.CalcLargeStraight(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Full_house = gameEngine.CalcFullHouse(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Chance_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Chance = gameEngine.CalcChance(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
            {
                ActiveGame.Players[currentPlayer].scoreCard.Yatzy = gameEngine.CalcYatzy(dices);
            }

        }

        private void ResetDices()
        {
            for (int i = 0; i < dices.Length; i++)
            {
                dices[i].Rolled_number = 0;
                dices[i].Hold = true;
            }
            SetDiceImage();

            Hold1();
            Hold2();
            Hold3();
            Hold4();
            Hold5();
        }

        private bool focusChanged1;
        public bool FocusChanged1
        {
            get { return focusChanged1; }
            set
            {
                focusChanged1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FocusChanged1"));
            }
        }

        #endregion

        #region Set Active Player
        private void SetActivePlayer()
        {
            FireworksVisible = "hidden";
            ResetTo0();
            ResetDices();

            if (currentPlayer != ActiveGame.Players.Count - 1)
            {
                currentPlayer++;
                throws = 0;
                HighlightActivePlayer();
                Helper();

                if (ActiveGame.Players[currentPlayer].Isbot == true && ActiveGame.IsGameFinished() == false)
                {
                    BotTest();
                }
                else
                {
                    playerChangedString = $"Nu är det {ActiveGame.Players[currentPlayer].Nickname}'s tur";

                    ThrowsAmount();

                    ClickCommandThrow.RaiseCanExecuteChanged();
                    RaisePropertyChangedP1();
                    RaisePropertyChangedP2();
                    RaisePropertyChangedP3();
                    RaisePropertyChangedP4();
                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedString"));
                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));

                    SetTimerUpdatePlayer();
                }
            }
            else if (currentPlayer == ActiveGame.Players.Count - 1)
            {
                currentPlayer = 0;
                throws = 0;
                HighlightActivePlayer();
                Helper();

                if (ActiveGame.Players[currentPlayer].Isbot == true)
                {
                    BotTest();
                }
                else
                {
                    playerChangedString = $"Nu är det {ActiveGame.Players[currentPlayer].Nickname}'s tur";

                    ThrowsAmount();

                    ClickCommandThrow.RaiseCanExecuteChanged();
                    RaisePropertyChangedP1();
                    RaisePropertyChangedP2();
                    RaisePropertyChangedP3();
                    RaisePropertyChangedP4();
                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedString"));
                    OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));


                    SetTimerUpdatePlayer();
                }


                firsttimehelp = false;

            }
            if (ActiveGame.IsGameFinished() == true && doubleEndBackstop == 0)
            {
                GameEnd();
                doubleEndBackstop = 1;
            }
        }

        private string AmountOfThrows;
        public string AmountOfthrows
        {
            get { return AmountOfThrows; }
            set
            {
                AmountOfThrows = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountOfThrows"));
            }
        }

        private void ThrowsAmount()
        {
            if (throws == 0)
            {
                AmountOfthrows = $"Du har 3 kast kvar";
            }
            if (throws == 1)
            {
                AmountOfthrows = $"Du har 2 kast kvar";
            }
            if (throws == 2)
            {
                AmountOfthrows = $"Du har 1 kast kvar";
            }
            if (throws == 3)
            {
                AmountOfthrows = $"Du har inga kast kvar";
            }
        }


        #region Timer Update Player
        DispatcherTimer dispatcherTimerUpdatePlayer = new DispatcherTimer();
        private void SetTimerUpdatePlayer()
        {
            playerChangedVisible = "Visible";
            OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));
            dispatcherTimerUpdatePlayer.Interval = new TimeSpan(0, 0, 0, 10, 0);
            dispatcherTimerUpdatePlayer.Start();
            dispatcherTimerUpdatePlayer.Tick += new EventHandler(dispatcherTimerUpdatePlayer_Tick);
        }

        protected void dispatcherTimerUpdatePlayer_Tick(object sender, EventArgs e)
        {
            playerChangedVisible = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("PlayerChangedVisible"));
            dispatcherTimerUpdatePlayer.Stop();
        }
        #endregion

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


        #endregion

        #region Dices

        #region Dice HoldState

        #region MoveDicesEvent

        #region  D1
        private string d1Visibility = "Visible";
        public string D1Visibility
        {
            get { return this.d1Visibility; }
            set
            {
                this.d1Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D1Visibility"));
            }
        }

        private string d1HoldVisibility = "Visible";
        public string D1HoldVisibility
        {
            get { return this.d1HoldVisibility; }
            set
            {
                this.d1HoldVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D1HoldVisibility"));
            }
        }
        #endregion

        #region D2

        private string d2Visibility = "Visible";
        public string D2Visibility
        {
            get { return this.d2Visibility; }
            set
            {
                this.d2Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D2Visibility"));
            }
        }

        private string d2HoldVisibility = "Visible";
        public string D2HoldVisibility
        {
            get { return this.d2HoldVisibility; }
            set
            {
                this.d2HoldVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D2HoldVisibility"));
            }
        }
        #endregion

        #region D3

        private string d3Visibility = "Visible";
        public string D3Visibility
        {
            get { return this.d3Visibility; }
            set
            {
                this.d3Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D3Visibility"));
            }
        }

        private string d3HoldVisibility = "Visible";
        public string D3HoldVisibility
        {
            get { return this.d3HoldVisibility; }
            set
            {
                this.d3HoldVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D3HoldVisibility"));
            }
        }
        #endregion

        #region D4

        private string d4Visibility = "Visible";
        public string D4Visibility
        {
            get { return this.d4Visibility; }
            set
            {
                this.d4Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D4Visibility"));
            }
        }

        private string d4HoldVisibility = "Visible";
        public string D4HoldVisibility
        {
            get { return this.d4HoldVisibility; }
            set
            {
                this.d4HoldVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D4HoldVisibility"));
            }
        }
        #endregion

        #region D5

        private string d5Visibility = "Visible";
        public string D5Visibility
        {
            get { return this.d5Visibility; }
            set
            {
                this.d5Visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D5Visibility"));
            }
        }

        private string d5HoldVisibility = "Visible";
        public string D5HoldVisibility
        {
            get { return this.d5HoldVisibility; }
            set
            {
                this.d5HoldVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("D5HoldVisibility"));
            }
        }
        #endregion

        #endregion

        #region Hold Dices Methods
        public void HideDices()
        {
            D1HoldVisibility = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("D1HoldVisibility"));
            D2HoldVisibility = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("D2HoldVisibility"));
            D3HoldVisibility = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("D3HoldVisibility"));
            D4HoldVisibility = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("D4HoldVisibility"));
            D5HoldVisibility = "Hidden";
            OnPropertyChanged(new PropertyChangedEventArgs("D5HoldVisibility"));
        }

        public void Hold1()
        {
            if (dices[0].Hold == false)
            {
                dices[0].Hold = true;

                D1Visibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D1Visibility"));
                D1HoldVisibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D1HoldVisibility"));
            }
            else if (dices[0].Hold == true)
            {
                dices[0].Hold = false;

                D1Visibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D1Visibility"));
                D1HoldVisibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D1HoldVisibility"));
            }
        }

        public void Hold2()
        {
            if (dices[1].Hold == false)
            {
                dices[1].Hold = true;

                D2Visibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D2Visibility"));
                D2HoldVisibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D2HoldVisibility"));
            }
            else if (dices[1].Hold == true)
            {
                dices[1].Hold = false;

                D2Visibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D2Visibility"));
                D2HoldVisibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D2HoldVisibility"));
            }
        }

        public void Hold3()
        {
            if (dices[2].Hold == false)
            {
                dices[2].Hold = true;

                D3Visibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D3Visibility"));
                D3HoldVisibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D3HoldVisibility"));
            }
            else if (dices[2].Hold == true)
            {
                dices[2].Hold = false;

                D3Visibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D3Visibility"));
                D3HoldVisibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D3HoldVisibility"));
            }
        }

        public void Hold4()
        {
            if (dices[3].Hold == false)
            {
                dices[3].Hold = true;

                D4Visibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D4Visibility"));
                D4HoldVisibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D4HoldVisibility"));
            }
            else if (dices[3].Hold == true)
            {
                dices[3].Hold = false;

                D4Visibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D4Visibility"));
                D4HoldVisibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D4HoldVisibility"));
            }
        }

        public void Hold5()
        {
            if (dices[4].Hold == false)
            {
                dices[4].Hold = true;

                D5Visibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D5Visibility"));
                D5HoldVisibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D5HoldVisibility"));
            }
            else if (dices[4].Hold == true)
            {
                dices[4].Hold = false;

                D5Visibility = "Visible";
                OnPropertyChanged(new PropertyChangedEventArgs("D5Visibility"));
                D5HoldVisibility = "Hidden";
                OnPropertyChanged(new PropertyChangedEventArgs("D5HoldVisibility"));
            }
        }
        #endregion

        #region Hold Dices Commands 
        private RelayCommand clickHold1;
        public RelayCommand ClickHold1
        {
            get
            {
                return clickHold1 ?? (clickHold1 = new RelayCommand(w => Hold1()));
            }
        }

        private RelayCommand clickHold2;
        public RelayCommand ClickHold2
        {
            get
            {
                return clickHold2 ?? (clickHold2 = new RelayCommand(w => Hold2()));
            }
        }

        private RelayCommand clickHold3;
        public RelayCommand ClickHold3
        {
            get
            {
                return clickHold3 ?? (clickHold3 = new RelayCommand(w => Hold3()));
            }
        }

        private RelayCommand clickHold4;
        public RelayCommand ClickHold4
        {
            get
            {
                return clickHold4 ?? (clickHold4 = new RelayCommand(w => Hold4()));
            }
        }

        private RelayCommand clickHold5;
        public RelayCommand ClickHold5
        {
            get
            {
                return clickHold5 ?? (clickHold5 = new RelayCommand(w => Hold5()));
            }
        }

        #endregion

        #endregion

        #region Dice Images

        private void SetDiceImage()
        {
            for (int i = 0; i < dices.Length; i++)
            {
                if (dices[i].Rolled_number == 0)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D0.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D0.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D0.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D0.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D0.png", UriKind.RelativeOrAbsolute));
                    }
                }

                else if (dices[i].Rolled_number == 1)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D1.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D1.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D1.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D1.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D1.png", UriKind.RelativeOrAbsolute));
                    }
                }


                else if (dices[i].Rolled_number == 2)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D2.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D2.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D2.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D2.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D2.png", UriKind.RelativeOrAbsolute));
                    }
                }
                else if (dices[i].Rolled_number == 3)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D3.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D3.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D3.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D3.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D3.png", UriKind.RelativeOrAbsolute));
                    }
                }
                else if (dices[i].Rolled_number == 4)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D4.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D4.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D4.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D4.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D4.png", UriKind.RelativeOrAbsolute));
                    }
                }
                else if (dices[i].Rolled_number == 5)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D5.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D5.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D5.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D5.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D5.png", UriKind.RelativeOrAbsolute));
                    }
                }
                else if (dices[i].Rolled_number == 6)
                {
                    if (i == 0)
                    {
                        ImageD1 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D6.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 1)
                    {
                        ImageD2 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D6.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 2)
                    {
                        ImageD3 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D6.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 3)
                    {
                        ImageD4 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D6.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (i == 4)
                    {
                        ImageD5 = new BitmapImage(new Uri("pack://application:,,,/Icons/DicesImages/D6.png", UriKind.RelativeOrAbsolute));
                    }
                }
            }
        }

        #region Bitmaps

        private BitmapImage imageD1;
        public BitmapImage ImageD1
        {
            get { return imageD1; }
            set
            {
                imageD1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageD1"));
            }
        }

        private BitmapImage imageD2;
        public BitmapImage ImageD2
        {
            get { return imageD2; }
            set
            {
                imageD2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageD2"));
            }
        }

        private BitmapImage imageD3;
        public BitmapImage ImageD3
        {
            get { return imageD3; }
            set
            {
                imageD3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageD3"));
            }
        }

        private BitmapImage imageD4;
        public BitmapImage ImageD4
        {
            get { return imageD4; }
            set
            {
                imageD4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageD4"));
            }
        }

        private BitmapImage imageD5;
        public BitmapImage ImageD5
        {
            get { return imageD5; }
            set
            {
                imageD5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageD5"));
            }
        }

        #endregion

        #endregion

        #region ThrowAnimation

        private void DiceRolls()
        {
            if (dices[0].Hold == false)
            {
                RollD1 = "Visible";
            }
            if (dices[1].Hold == false)
            {
                RollD2 = "Visible";
            }
            if (dices[2].Hold == false)
            {
                RollD3 = "Visible";
            }
            if (dices[3].Hold == false)
            {
                RollD4 = "Visible";
            }
            if (dices[4].Hold == false)
            {
                RollD5 = "Visible";
            }
        }

        #region AnimationBindings
        private string rollD1 = "Hidden";
        public string RollD1
        {
            get { return rollD1; }
            set
            {
                rollD1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RollD1"));
            }
        }

        private string rollD2 = "Hidden";
        public string RollD2
        {
            get { return rollD2; }
            set
            {
                rollD2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RollD2"));
            }
        }

        private string rollD3 = "Hidden";
        public string RollD3
        {
            get { return rollD3; }
            set
            {
                rollD3 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RollD3"));
            }
        }

        private string rollD4 = "Hidden";
        public string RollD4
        {
            get { return rollD4; }
            set
            {
                rollD4 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RollD4"));
            }
        }

        private string rollD5 = "Hidden";
        public string RollD5
        {
            get { return rollD5; }
            set
            {
                rollD5 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RollD5"));
            }
        }
        #endregion

        #region AnimationTimer
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private void SetTimerAni()
        {
            DiceRolls();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            ThrowDiceDuringAnimationMethod();
        }

        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();

            RollD1 = "Hidden";
            RollD2 = "Hidden";
            RollD3 = "Hidden";
            RollD4 = "Hidden";
            RollD5 = "Hidden";
            ThrowDiceAfterAnimationMethod();
        }
        #endregion

        #endregion

        #endregion

        #region  Game End
        private void GameEnd()
        {
            MusicEngine.StartStop();
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            MessageBoxViewModel messageBoxViewModel = new MessageBoxViewModel();
            MessageBoxViews messageBoxView = new MessageBoxViews();
            StartView startView = new StartView();
            int first = 0;
            string firsts = "";
            int second = 0;
            string seconds = "";
            int third = 0;
            string thirds = "";
            int fourth = 0;
            string fourths = "";
            string sendmsg = "";


            foreach (var player in ActiveGame.Players)
            {
                DbOperations.SetScore(player.ScoreCard.Total_score, ActiveGame.Game_Id, player.Player_id);
            }
            DbOperations.EndGame(ActiveGame.Game_Id, DateTime.Now);

            if (ActiveGame.Players.Count == 2)
            {

                if (ActiveGame.Players[0].ScoreCard.Total_score > ActiveGame.Players[1].ScoreCard.Total_score)
                {
                    first = ActiveGame.Players[0].ScoreCard.Total_score;
                    firsts = ActiveGame.Players[0].Nickname;
                    second = ActiveGame.Players[1].ScoreCard.Total_score;
                    seconds = ActiveGame.Players[1].Nickname;


                    sendmsg = $"Spelet är slut! \nVinnaren är {firsts} med {first} poäng  \n{seconds} fick {second} poäng ";
                }
                else if (ActiveGame.Players[0].ScoreCard.Total_score < ActiveGame.Players[1].ScoreCard.Total_score)
                {
                    first = ActiveGame.Players[1].ScoreCard.Total_score;
                    firsts = ActiveGame.Players[1].Nickname;
                    second = ActiveGame.Players[0].ScoreCard.Total_score;
                    seconds = ActiveGame.Players[0].Nickname;


                    sendmsg = $"Spelet är slut! \nVinnaren är {firsts} med {first} poäng  \n{seconds} fick {second} poäng ";
                }
                else if (ActiveGame.Players[0].ScoreCard.Total_score == ActiveGame.Players[1].ScoreCard.Total_score)
                {
                    first = ActiveGame.Players[1].ScoreCard.Total_score;
                    firsts = ActiveGame.Players[1].Nickname;
                    second = ActiveGame.Players[0].ScoreCard.Total_score;
                    seconds = ActiveGame.Players[0].Nickname;


                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {seconds} \nmed {first} poäng";
                }
                StringHandler.SetString(sendmsg);
                messageBoxView.Show();
            }

            if (3 == ActiveGame.Players.Count)
            {
                int place = 0;

                for (int i = 0; i < ActiveGame.Players.Count; i++)
                {

                    if (place == 0)
                    {
                        first = ActiveGame.Players[i].ScoreCard.Total_score;
                        firsts = ActiveGame.Players[i].Nickname;
                        place++;
                    }
                    else if (place == 1)
                    {
                        second = ActiveGame.Players[i].ScoreCard.Total_score;
                        seconds = ActiveGame.Players[i].Nickname;
                        place++;
                    }
                    else if (place == 2)
                    {
                        third = ActiveGame.Players[i].ScoreCard.Total_score;
                        thirds = ActiveGame.Players[i].Nickname;
                    }

                }

                if (first > second && first > third)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {firsts} med {first} poäng  \n{seconds} fick {second} poäng \n{thirds} fick {third} poäng";
                }
                else if (first < second && second < third)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {seconds} med {second} poäng \n{firsts} fick {first} poäng \n{thirds} fick {third} poäng ";
                }
                else if (first < third && second > third)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {thirds} med {third} poäng \n{firsts} fick {first} poäng \n{seconds} fick {second} poäng ";
                }
                else if (first == second && first != third && second != third)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {seconds} med {first} poäng \n{thirds} fick {third} poäng";
                }
                else if (first != second && first == third && second != third)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {thirds} med {first} poäng \n{seconds} fick {second} poäng";
                }
                else if (first != second && first != third && second == third)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {seconds} och {thirds} med {second} poäng \n{firsts} fick {first} poäng";
                }
                else if (first == second && first == third && second == third)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts}, {seconds} och {thirds} \nmed {first} poäng";
                }

                StringHandler.SetString(sendmsg);
                messageBoxView.Show();
            }

            if (4 == ActiveGame.Players.Count)
            {
                int place = 0;
                for (int i = 0; i < ActiveGame.Players.Count; i++)
                {

                    if (place == 0)
                    {
                        first = ActiveGame.Players[i].ScoreCard.Total_score;
                        firsts = ActiveGame.Players[i].Nickname;
                        place++;
                    }
                    else if (place == 1)
                    {
                        second = ActiveGame.Players[i].ScoreCard.Total_score;
                        seconds = ActiveGame.Players[i].Nickname;
                        place++;
                    }
                    else if (place == 2)
                    {
                        third = ActiveGame.Players[i].ScoreCard.Total_score;
                        thirds = ActiveGame.Players[i].Nickname;
                        place++;
                    }
                    else if (place == 3)
                    {
                        fourth = ActiveGame.Players[i].ScoreCard.Total_score;
                        fourths = ActiveGame.Players[i].Nickname;
                    }

                }

                #region 1 vinnare
                if (first > second && first > third && first > fourth)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {firsts} med {first} poäng  \n{seconds} fick {second} poäng \n{thirds} fick {third} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first < second && second > third && second > fourth)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {seconds} med {second} poäng  \n{firsts} fick {first} poäng \n{thirds} fick {third} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first < third && second < third && third > fourth)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {thirds} med {third} poäng \n{firsts} fick {first} poäng \n{seconds} fick {second} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first < fourth && second < fourth && third < fourth)
                {
                    sendmsg = $"Spelet är slut! \nVinnaren är {fourths} med {fourth} poäng \n{firsts} fick {first} poäng \n{seconds} fick {second} poäng \n{thirds} fick {third} poäng";
                }
                #endregion

                #region 2 vinnare
                else if (first == second && first != third && first != fourth && second != third && second != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {seconds} med {first} poäng \n{thirds} fick {third} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first != second && first == third && first != fourth && second != third && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {thirds} med {first} poäng \n{seconds} fick {second} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first != second && first != third && first == fourth && second != fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts} och {fourths} med {first} poäng \n{seconds} fick {second} poäng \n{thirds} fick {third} poäng";
                }
                else if (first != second && first != third && second == third && second != fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {seconds} och {thirds} med {second} poäng \n{firsts} fick {first} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first != second && first != fourth && second == third && second != fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {seconds} och {fourths} med {second} poäng \n{firsts} fick {first} poäng \n{thirds} fick {third} poäng";
                }
                else if (first != third && first != fourth && second == third && second != fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {thirds} och {fourths} med {third} poäng \n{firsts} fick {first} poäng \n{seconds} fick {second} poäng";
                }
                #endregion

                #region 3 vinnare
                else if (first == second && first == third && first != fourth && second == third && second != fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts}, {seconds} och {thirds} \nmed {first} poäng \n{fourths} fick {fourth} poäng";
                }
                else if (first == second && first == third && first != fourth && second != third && second == fourth && third != fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts}, {seconds} och {fourths} \nmed {first} poäng \n{thirds} fick {third} poäng";
                }
                else if (first != second && first == third && first == fourth && second != third && second != fourth && third == fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts}, {thirds} och {fourths} \nmed {first} poäng \n{seconds} fick {second} poäng";
                }
                else if (first != second && first == third && first == fourth && second != third && second != fourth && third == fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {seconds}, {thirds} och {fourths} \nmed {second} poäng \n{firsts} fick {first} poäng";
                }
                #endregion

                #region  4 vinnare 
                else if (first == second && first == third && first == fourth)
                {
                    sendmsg = $"Spelet är slut! \nDet blev delad plats mellan {firsts}, {seconds}, {thirds} och {fourths}\n med {first} poäng";
                }
                #endregion

                StringHandler.SetString(sendmsg);
                messageBoxView.Show();

            }
        }

        public void CloseCurrentWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Close();
        }

        #endregion

        #region Music

        private void Music()
        {
            Thread t = new Thread(MusicEngine.StartStop);
            string filename = "Media/Sounds/Mellow_Aether.wav";
            MusicEngine.file = filename;
            t.Start();

            SoundIconSwitch();
        }

        private RelayCommand _clickCommandPlus;
        public RelayCommand ClickCommandPlus
        {
            get
            {
                return _clickCommandPlus ?? (_clickCommandPlus = new RelayCommand(t => IncreaseVolume()));
            }
        }
        private void IncreaseVolume()
        {
            MusicEngine.IncreaseVolume();
        }

        private RelayCommand _clickCommandMinus;
        public RelayCommand ClickCommandMinus
        {
            get
            {
                return _clickCommandMinus ?? (_clickCommandMinus = new RelayCommand(t => DecreaseVolume()));
            }
        }
        private void DecreaseVolume()
        {
            MusicEngine.DecreaseVolume();
        }

        private RelayCommand _clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new RelayCommand(t => Mute()));
            }
        }

        private void Mute()
        {
            MusicEngine.Mute();
            SoundIconSwitch();
        }

        private RelayCommand _clickCommandSoundMenu;
        public RelayCommand ClickCommandSoundMenu
        {
            get
            {
                return _clickCommandSoundMenu ?? (_clickCommandSoundMenu = new RelayCommand(t => SoundMenu()));
            }
        }
        private void SoundMenu()
        {
            MusicEngine.DrawerPlayerMethod();
        }

        private RelayCommand _clickCommandCollaps;
        public RelayCommand ClickCommandCollaps
        {
            get
            {
                return _clickCommandCollaps ?? (_clickCommandCollaps = new RelayCommand(t => Collaps()));
            }
        }
        private void Collaps()
        {
            MusicEngine.DrawerPlayerMethod();
        }

        private void SoundIconSwitch()
        {
            if (MusicEngine.isMute == true)
            {
                Sound_icon = new BitmapImage(new Uri("pack://application:,,,/Icons/SoundIcons/Mute.png", UriKind.RelativeOrAbsolute));
            }
            else if (MusicEngine.isMute == false)
            {
                Sound_icon = new BitmapImage(new Uri("pack://application:,,,/Icons/SoundIcons/VolumeMenu.png", UriKind.RelativeOrAbsolute));
            }
        }

        private BitmapImage soundIcon;
        public BitmapImage Sound_icon
        {
            get { return soundIcon; }
            set
            {
                soundIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sound_icon"));
            }
        }
        #endregion

        #region Helper
        private string helperString;
        public string HelperString
        {
            get { return helperString; }
            set
            {
                helperString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HelperString"));
            }
        }

        Helper helper = new Helper();
        bool firsttimehelp;
        private void Helper()
        {
            string helperS;

            helperS = helper.WhatDoTo(dices, currentPlayer, throws);

            if (helperS == "YATZY!!!")
            {
                FireworksVisible = "visible";
            }

            if (firsttimehelp == true)
            {
                helperS += "\nDu väljer poäng genom att trycka på vad \ndu vill satsa på i poängtabellen ";
            }

            if (ActiveGame.Helper == true)
            {
                helperString = helperS;
                OnPropertyChanged(new PropertyChangedEventArgs("HelperString"));
                Helper_color = "Green";
            }
            else
            {
                helperString = "";
                OnPropertyChanged(new PropertyChangedEventArgs("HelperString"));
                Helper_color = "Red";
            }

        }
        private string fireworksVisible = "hidden";
        public string FireworksVisible
        {
            get { return this.fireworksVisible; }
            set
            {
                this.fireworksVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FireworksVisible"));
            }
        }

        #endregion

        #region Bot

        private string BotActionLabel;
        public string BotActionlabel
        {
            get { return BotActionLabel; }
            set
            {
                BotActionLabel = value;
            }
        }

        private void BotTest()
        {
            int botthrows = 0;

            Bot bot = new Bot();
            bool finishRound = false;

            while (finishRound == false)
            {

                List<int> actions = new List<int>();
                actions = bot.WhatDoTo(dices, currentPlayer, botthrows);

                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] == 0)
                    {
                        RollDice();

                        CalcKind();

                        AssignScoreCardData();

                        botthrows++;
                    }
                    else if (actions[i] == 1)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Ettor";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockOnes();
                        finishRound = true;
                    }
                    else if (actions[i] == 2)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Tvåor";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockTwos();
                        finishRound = true;
                    }
                    else if (actions[i] == 3)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Treor";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockThrees();
                        finishRound = true;
                    }
                    else if (actions[i] == 4)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Fyror";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockFours();
                        finishRound = true;
                    }
                    else if (actions[i] == 5)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Femmor";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockFives();
                        finishRound = true;
                    }
                    else if (actions[i] == 6)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Sexor";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockSixes();
                        finishRound = true;
                    }
                    else if (actions[i] == 7)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Ett Par";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockOnePair();
                        finishRound = true;
                    }
                    else if (actions[i] == 8)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Två Par";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockTwoPairs();
                        finishRound = true;
                    }
                    else if (actions[i] == 9)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Triss";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockThreeOfAKind();
                        finishRound = true;
                    }
                    else if (actions[i] == 10)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Fyrtal";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockFourOfAKind();
                        finishRound = true;
                    }
                    else if (actions[i] == 11)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Liten Stege";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockSmallStraight();
                        finishRound = true;
                    }
                    else if (actions[i] == 12)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Stor Stege";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockLargeStraight();
                        finishRound = true;
                    }
                    else if (actions[i] == 13)
                    {
                        LockFullHouse();
                        finishRound = true;
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Kåk";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                    }
                    else if (actions[i] == 14)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde Chans";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockChance();
                        finishRound = true;
                    }
                    else if (actions[i] == 15)
                    {
                        BotActionlabel = $"{ActiveGame.Players[currentPlayer].Nickname} Valde YATZY";
                        OnPropertyChanged(new PropertyChangedEventArgs("BotActionLabel"));
                        LockYatzy();
                        finishRound = true;
                    }

                    else if (actions[i] == 51 && dices[0].Hold == false)
                    {
                        ClickHold1.Execute(clickHold1);
                    }
                    else if (actions[i] == 52 && dices[1].Hold == false)
                    {
                        ClickHold2.Execute(clickHold2);
                    }
                    else if (actions[i] == 53 && dices[2].Hold == false)
                    {
                        ClickHold3.Execute(clickHold3);
                    }
                    else if (actions[i] == 54 && dices[3].Hold == false)
                    {
                        ClickHold4.Execute(clickHold4);
                    }
                    else if (actions[i] == 55 && dices[4].Hold == false)
                    {
                        ClickHold5.Execute(clickHold5);
                    }
                }

            }
        }
        #endregion

        #region ExitGame
        private RelayCommand _exitGameCommand;
        public RelayCommand ExitGameCommand
        {
            get
            {
                return _exitGameCommand ?? (_exitGameCommand = new RelayCommand(w => ExitGameMessageBox(), w => CanExecute));
            }
        }

        public void ExitGameMessageBox()
        {
            if (MessageBox.Show("Avslutar du spelet raderas omgången", "Avsluta spelet?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                ExitGame();
            }
        }

        private void RemoveGame()
        {
            DbOperations.ExitGame(ActiveGame.Game_Id);
        }

        private void ExitGame()
        {

            RemoveGame();

            MusicEngine.StartStop();

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            LobbyView lobbyView = new LobbyView();

            lobbyView.Show();

            window.Close();

        }

        public bool CanExecute
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Rules
        private RelayCommand _openRulesCommand;
        public RelayCommand OpenRulesCommand
        {
            get
            {
                return _openRulesCommand ?? (_openRulesCommand = new RelayCommand(w => OpenRules(), w => CanExecute));
            }
        }

        private void OpenRules()
        {
            RulesView rulesView = new RulesView();

            rulesView.Show();
        }
        #endregion

        #region  Helper On Off Button
        private RelayCommand _helperOnOffCommand;
        public RelayCommand HelperOnOffCommand
        {
            get
            {
                return _helperOnOffCommand ?? (_helperOnOffCommand = new RelayCommand(w => HelperOnOff(), w => CanExecute));
            }
        }

        private void HelperOnOff()
        {
            if (ActiveGame.Helper == true)
            {
                ActiveGame.Helper = false;

                helperString = "";
                OnPropertyChanged(new PropertyChangedEventArgs("HelperString"));

                Helper_color = "Red";
            }
            else if (ActiveGame.Helper == false)
            {
                ActiveGame.Helper = true;

                Helper();

                Helper_color = "Green";
            }
        }

        private string helperColor;
        public string Helper_color
        {
            get { return helperColor; }
            set
            {
                helperColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Helper_color"));
            }
        }
        #endregion
    }
}
