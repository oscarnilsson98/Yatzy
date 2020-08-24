using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatzyGrupp7MVVM.Models
{
    class Helper
    {

        GameEngine gameEngine = new GameEngine();

        private string helper;

        public string WhatDoTo(Dices[] dices, int currentPlayer, int throws)
        {
            int number;

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

            if (throws == 0)
            {
                return helper = $"Kasta för att starta din runda";
            }
            else if (throws < 3)
            {
                if (numberYatzy != 0)
                {
                    return helper = $"YATZY!!!";
                }

                else if (numberFullHouse != 0)
                {
                    return helper = $"Du borde satsa på kåk";
                }

                else if (numberLargeStraight != 0)
                {
                    return helper = $"Du borde satsa på stor straight";
                }

                else if (numberSmallStraight != 0)
                {
                    return helper = $"Du borde satsa på liten straight";
                }

                else if (numberTwoPair != 0)
                {
                    if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == true)
                    {
                        return helper = $"Du borde hålla på två par och försöka på kåk";
                    }
                    else
                    {
                        return helper = $"Du borde satsa på två par";
                    }
                }

                else if (numberOneToSixHighest >= 2)
                {
                    number = numberOneToSixHighest;

                    return helper = $"Du borde spara dina {number} och kasta vidare";
                }

                else if (numberFourOfAKind != 0)
                {
                    if (ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
                    {
                        return helper = $"Du kan välja fyrtal men om du kastar \n igen kan du få yatzy";
                    }
                    else
                    {
                        return helper = $"Du borde satsa på fyrtal";
                    }
                }

                else if (numberThreeOfAKind != 0)
                {
                    if (ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled == true && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
                    {
                        return helper = $"Du kan välja triss men om du kastar igen \n har du chans att få fyrtal eller yatzy";
                    }
                    else if (ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled == false && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
                    {
                        return helper = $"Du kan välja triss men om kastar igen \n har du chans att få yatzy";
                    }
                    else if (ActiveGame.Players[currentPlayer].scoreCard.Four_of_a_kind_enabled == true && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == false)
                    {
                        return helper = $"Du kan välja triss men om du kastar \n igen har du chans att få fyrtal";
                    }
                    else
                    {
                        return helper = $"Du borde satsa på triss";
                    }
                }

                else if (numberOnePair != 0)
                {
                    if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == true && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
                    {
                        return helper = $"Du borde hålla på ett par men kasta igen \n med resten för att få kåk eller yatzy";
                    }
                    if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == false && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == true)
                    {
                        return helper = $"Du borde hålla på ett par men kasta igen \n med resten för att få yatzy";
                    }
                    if (ActiveGame.Players[currentPlayer].scoreCard.Full_house_enabled == true && ActiveGame.Players[currentPlayer].scoreCard.Yatzy_enabled == false)
                    {
                        return helper = $"Du borde hålla på ett par men kasta igen \n med resten för att få kåk";
                    }
                    else
                    {
                        return helper = $"Du borde satsa på ett par";
                    }

                }

                else
                {
                    return helper = $"Du borde kasta igen";
                }
            }
            else
            {
                if (numberYatzy != 0)
                {
                    return helper = $"YATZY!!!";
                }

                else if (numberFullHouse != 0)
                {
                    return helper = $"Du borde satsa på kåk";
                }

                else if (numberLargeStraight != 0)
                {
                    return helper = $"Du borde satsa på stor straight";
                }

                else if (numberSmallStraight != 0)
                {
                    return helper = $"Du borde satsa på liten straight";
                }

                else if (numberOneToSixHighest >= 3)
                {
                    number = numberOneToSixHighest;

                    return helper = $"Du borde satsa på {number}";
                }

                else if (numberFourOfAKind != 0)
                {
                    return helper = $"Du borde satsa på fyrtal";
                }

                else if (numberTwoPair != 0)
                {
                    return helper = $"Du borde satsa på två par";
                }

                else if (numberThreeOfAKind != 0)
                {
                    return helper = $"Du borde satsa på tretal";
                }

                else if (numberOnePair != 0)
                {
                    return helper = $"Du borde satsa på ett par";
                }

                else if (numberChance != 0)
                {
                    return helper = $"Du borde satsa på chans och få {numberChance} poäng";
                }

                else if (numberOneToSixLowest >= 1)
                {
                    number = numberOneToSixLowest;

                    return helper = $"Du borde satsa på {number}";
                }

                else
                {
                    return helper = $"Du får stryka någonting (trycka i 0) \n ";
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

            if (HighorLow == "high")
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
