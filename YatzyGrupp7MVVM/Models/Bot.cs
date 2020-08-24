using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatzyGrupp7MVVM.ViewModels;

namespace YatzyGrupp7MVVM.Models
{
    class Bot
    {

        GameEngine gameEngine = new GameEngine();
        GameViewModel gameViewModel = new GameViewModel();

        public List<int> WhatDoTo(Dices[] dices, int currentPlayer, int throws)
        {
            List<int> actions = new List<int>();

            int numberOneToSixLowest = 0;
            int numberOneToSixHighest = 0;
            int numberOnePair = 0;
            int numberTwoPair = 0;
            int numberThreeOfAKind = 0;
            int numberFourOfAKind = 0;
            int numberSmallStraight = 0;
            int numberLargeStraight = 0;
            int numberFullHouse = 0;
            int numberChance = 0;
            int numberYatzy = 0;

            numberOneToSixLowest = OneToSix(dices, currentPlayer, "low");
            numberOneToSixHighest = OneToSix(dices, currentPlayer, "high");

            #region  Calculations
            if (ActiveGame.Players[currentPlayer].scoreCard.One_pair_enabled == true)
            {
                numberOnePair = gameEngine.CalcOnePair(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Two_pair_enabled == true)
            {
                numberTwoPair = gameEngine.CalcTwoPair(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Three_of_a_kind_enabled == true)
            {
                numberThreeOfAKind = gameEngine.CalcThreeOfAkind(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled == true)
            {
                numberFourOfAKind = gameEngine.CalcFourOfAkind(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Small_straight_enabled == true)
            {
                numberSmallStraight = gameEngine.CalcSmallStraight(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Large_straight_enabled == true)
            {
                numberLargeStraight = gameEngine.CalcLargeStraight(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == true)
            {
                numberFullHouse = gameEngine.CalcFullHouse(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Chance_enabled == true)
            {
                numberChance = gameEngine.CalcChance(dices);
            }
            if (ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
            {
                numberYatzy = gameEngine.CalcYatzy(dices);
            }
            #endregion

            if (throws < 3)
            {
                if (numberYatzy != 0)
                {
                    actions.Add(15);
                    return actions;
                }

                else if (numberFullHouse != 0)
                {
                    actions.Add(13);
                    return actions;
                }

                else if (numberLargeStraight != 0)
                {
                    actions.Add(12);
                    return actions;
                }

                else if (numberSmallStraight != 0)
                {
                    actions.Add(11);
                    return actions;
                }

                else if (numberTwoPair != 0)
                {
                    for (int d = 0; d < dices.Length; d++)
                    {
                        if (dices[d].Rolled_number == numberOneToSixHighest)
                        {
                            actions.Add(d + 51);
                        }
                    }
                    for (int d = 0; d < dices.Length; d++)
                    {
                        if (dices[d].Rolled_number == numberOneToSixLowest)
                        {
                            actions.Add(d + 51);
                        }
                    }
                    actions.Add(0);
                    return actions;
                }

                else if (numberOneToSixHighest >= 3)
                {
                    for (int d = 0; d < dices.Length; d++)
                    {
                        if (dices[d].Rolled_number == numberOneToSixHighest)
                        {
                            actions.Add(d + 51);
                        }
                    }
                    actions.Add(0);
                    return actions;
                }

                else if (numberFourOfAKind != 0)
                {
                    actions.Add(10);
                    return actions;
                }

                else if (numberThreeOfAKind != 0)
                {
                    actions.Add(9);
                    return actions;
                }

                else if (numberOnePair != 0)
                {
                    for (int d = 0; d < dices.Length; d++)
                    {
                        if (dices[d].Rolled_number == numberOneToSixHighest)
                        {
                            actions.Add(d + 51);
                        }
                    }
                    actions.Add(0);
                    return actions;

                }

                else
                {
                    actions.Add(0);
                    return actions;
                }
            }


            else
            {
                if (numberYatzy != 0)
                {
                    actions.Add(15);
                    return actions;
                }

                else if (numberFullHouse != 0)
                {
                    actions.Add(13);
                    return actions;
                }

                else if (numberLargeStraight != 0)
                {
                    actions.Add(12);
                    return actions;
                }

                else if (numberSmallStraight != 0)
                {
                    actions.Add(11);
                    return actions;
                }

                else if (numberOneToSixHighest >= 3)
                {
                    actions.Add(numberOneToSixHighest);
                    return actions;
                }

                else if (numberFourOfAKind != 0)
                {
                    actions.Add(10);
                    return actions;
                }

                else if (numberTwoPair != 0)
                {
                    actions.Add(8);
                    return actions;
                }

                else if (numberThreeOfAKind != 0)
                {
                    actions.Add(9);
                    return actions;
                }

                else if (numberOnePair != 0)
                {
                    actions.Add(7);
                    return actions;
                }

                else if (numberOneToSixLowest >= 2)
                {
                    actions.Add(numberOneToSixLowest);
                    return actions;
                }

                else if (numberChance != 0)
                {
                    actions.Add(14);
                    return actions;
                }

                else
                {

                    if (ActiveGame.Players[currentPlayer].ScoreCard.Full_house_enabled == true)
                    {
                        actions.Add(13);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Large_straight_enabled == true)
                    {
                        actions.Add(12);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Small_straight_enabled == true)
                    {
                        actions.Add(11);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Four_of_a_kind_enabled == true)
                    {
                        actions.Add(10);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Two_pair_enabled == true)
                    {
                        actions.Add(8);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Three_of_a_kind_enabled == true)
                    {
                        actions.Add(9);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.One_pair_enabled == true)
                    {
                        actions.Add(7);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Chance_enabled == true)
                    {
                        actions.Add(14);
                        return actions;
                    }

                    else if (ActiveGame.Players[currentPlayer].ScoreCard.Yatzy_enabled == true)
                    {
                        //stryka yatzy
                        actions.Add(15);
                        return actions;
                    }

                    else
                    {
                        if (ActiveGame.Players[currentPlayer].ScoreCard.Ones_enabled == true)
                        {
                            actions.Add(1);
                        }
                        else if (ActiveGame.Players[currentPlayer].ScoreCard.Twos_enabled == true)
                        {
                            actions.Add(2);
                        }
                        else if (ActiveGame.Players[currentPlayer].ScoreCard.Threes_enabled == true)
                        {
                            actions.Add(3);
                        }
                        else if (ActiveGame.Players[currentPlayer].ScoreCard.Fours_enabled == true)
                        {
                            actions.Add(4);
                        }
                        else if (ActiveGame.Players[currentPlayer].ScoreCard.Fives_enabled == true)
                        {
                            actions.Add(5);
                        }
                        else
                        {
                            actions.Add(6);
                        }

                        return actions;
                    }
                }
            }




        }

        private int OneToSix(Dices[] dices, int currentPlayer, string HighorLow)
        {
            List<int> ones = new List<int>();
            List<int> twos = new List<int>();
            List<int> threes = new List<int>();
            List<int> fours = new List<int>();
            List<int> fives = new List<int>();
            List<int> sixes = new List<int>();

            for (int i = 0; i < dices.Length; i++)
            {
                if (dices[i].Rolled_number == 1 && ActiveGame.Players[currentPlayer].scoreCard.Ones_enabled == true)
                {
                    ones.Add(1);
                }
                else if (dices[i].Rolled_number == 2 && ActiveGame.Players[currentPlayer].scoreCard.Twos_enabled == true)
                {
                    twos.Add(2);
                }
                else if (dices[i].Rolled_number == 3 && ActiveGame.Players[currentPlayer].scoreCard.Threes_enabled == true)
                {
                    threes.Add(3);
                }
                else if (dices[i].Rolled_number == 4 && ActiveGame.Players[currentPlayer].scoreCard.Fours_enabled == true)
                {
                    fours.Add(4);
                }
                else if (dices[i].Rolled_number == 5 && ActiveGame.Players[currentPlayer].scoreCard.Fives_enabled == true)
                {
                    fives.Add(5);
                }
                else if (dices[i].Rolled_number == 6 && ActiveGame.Players[currentPlayer].scoreCard.Sixes_enabled == true)
                {
                    sixes.Add(6);
                }
            }

            int[] highestcount = new int[] { ones.Count, twos.Count, threes.Count, fours.Count, fives.Count, sixes.Count };

            if (HighorLow  == "high")
            {
                int m = highestcount.Max();
                int p = Array.IndexOf(highestcount, m);

                for (int i = 0; i < highestcount.Length; i++)
                {
                    if (2 <= highestcount[i] && p != i && m > highestcount[i])
                    {
                        m = highestcount[i];
                        p = Array.IndexOf(highestcount, m);
                    }
                }

                p++;

                return p;
            }
            else
            {
                int m = highestcount.Max();

                int p = Array.IndexOf(highestcount, m);

                for (int i = 0; i < highestcount.Length; i++)
                {
                    if (2 <= highestcount[i] && p != i && m < highestcount[i])
                    {
                        m = highestcount[i];
                        p = Array.IndexOf(highestcount, m);
                    }
                }

                p++;

                return p;
            }

        }
    }
}
