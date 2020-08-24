using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatzyGrupp7MVVM.Models
{
    class GameEngine
    {
        Random rnd = new Random();

        public int ThrowDice()
        {
            int diceThrow = rnd.Next(1, 7);

            return diceThrow;
        }

        #region Calculate Methods
        public int CalcXofAKind(Dices[] dices, int num)
        {
            int totalTempNumber = 0;

            for (int d = 0; d < dices.Count(); d++)
            {
                if (dices[d].Rolled_number == num)
                {
                    totalTempNumber += num;
                }
            }

            return totalTempNumber;
        }

        public int CalcOnePair(Dices[] dices)
        {

            int totalTempNumber = 0;
            int onePairSum = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i] != dices[d])
                    {

                        if (onePairSum < dices[i].Rolled_number + dices[d].Rolled_number)
                        {
                            onePairSum = dices[i].Rolled_number + dices[d].Rolled_number;
                            totalTempNumber = onePairSum;
                        }

                    }

                }
            }

            return totalTempNumber;

        }


        public int CalcTwoPair(Dices[] dices)
        {
            int totalTempNumber = 0;
            int pair1 = 0;
            int pair2 = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i] != dices[d])
                    {
                        if (pair1 == 0)
                        {
                            pair1 = dices[i].Rolled_number + dices[d].Rolled_number;
                            dices = dices.Where(val => val != dices[i] && val != dices[d]).ToArray();

                        }

                        else                                                
                        {

                            pair2 = dices[i].Rolled_number + dices[d].Rolled_number;

                        }

                    }
                }
            }

            if (pair1 != 0 && pair2 != 0 && pair1 != pair2)
            {
                totalTempNumber = pair1 + pair2;
            }


            return totalTempNumber;
        }

        public int CalcThreeOfAkind(Dices[] dices)
        {
            int totalTempNumber = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    for (int y = 0; y < dices.Length; y++)
                    {
                        if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i].Rolled_number == dices[y].Rolled_number && dices[d].Rolled_number == dices[y].Rolled_number && dices[i] != dices[d] && dices[i] != dices[y] && dices[d] != dices[y])
                        {
                            totalTempNumber = dices[i].Rolled_number + dices[d].Rolled_number + dices[y].Rolled_number;
                        }
                    }
                }
            }

            return totalTempNumber;

        }

        public int CalcFourOfAkind(Dices[] dices)
        {
            int totalTempNumber = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    for (int y = 0; y < dices.Length; y++)
                    {
                        for (int l = 0; l < dices.Length; l++)
                        {
                            if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i].Rolled_number == dices[y].Rolled_number && dices[d].Rolled_number == dices[y].Rolled_number && dices[l].Rolled_number == dices[i].Rolled_number && dices[l].Rolled_number == dices[d].Rolled_number && dices[l].Rolled_number == dices[y].Rolled_number && dices[i] != dices[d] && dices[i] != dices[y] && dices[d] != dices[y] && dices[l] != dices[i] && dices[l] != dices[d] && dices[l] != dices[y])
                            {
                                totalTempNumber = dices[i].Rolled_number + dices[d].Rolled_number + dices[y].Rolled_number + dices[l].Rolled_number;
                            }
                        }
                    }
                }
            }

            return totalTempNumber;

        }

        public int CalcSmallStraight(Dices[] dices)
        {
            int numToTest = 0;
            int[] numbers = new int[] { 1, 2, 3, 4, 5 };

            for (int i = 0; i < dices.Length; i++)
            {
                for (int n = 0; n < numbers.Length; n++)
                {
                    if (dices[i].Rolled_number == numbers[n])
                    {
                        numbers[n] = 0;
                        numToTest += dices[i].Rolled_number;
                    }
                }
            }

            if (numToTest == 15)
            {
                return 15;
            }
            else
            {
                return 0;
            }
        }

        public int CalcLargeStraight(Dices[] dices)
        {
            int numToTest = 0;
            int[] numbers = new int[] { 2, 3, 4, 5, 6 };

            for (int i = 0; i < dices.Length; i++)
            {
                for (int n = 0; n < numbers.Length; n++)
                {
                    if (dices[i].Rolled_number == numbers[n])
                    {
                        numbers[n] = 0;
                        numToTest += dices[i].Rolled_number;
                    }
                }
            }

            if (numToTest == 20)
            {
                return 20;
            }
            else
            {
                return 0;
            }
        }

        public int CalcFullHouse(Dices[] dices)
        {
            int threeKind = 0;
            int pair = 0;
            int checkNumber = 0;
            int totalTempNumber = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    for (int y = 0; y < dices.Length; y++)
                    {
                        if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i].Rolled_number == dices[y].Rolled_number && dices[d].Rolled_number == dices[y].Rolled_number && dices[i] != dices[d] && dices[i] != dices[y] && dices[d] != dices[y])
                        {
                            threeKind = dices[i].Rolled_number + dices[d].Rolled_number + dices[y].Rolled_number;
                            checkNumber = dices[i].Rolled_number;
                        }
                    }
                }
            }

            for (int i = 0; i < dices.Length; i++)
            {
                for (int d = 0; d < dices.Length; d++)
                {
                    if (dices[i].Rolled_number == dices[d].Rolled_number && dices[i] != dices[d] && dices[i].Rolled_number != checkNumber)
                    {
                        pair = dices[i].Rolled_number + dices[d].Rolled_number;
                    }
                }
            }

            if (threeKind != 0 && pair != 0)
            {
                totalTempNumber = threeKind + pair;
            }

            return totalTempNumber;
        }

        public int CalcChance(Dices[] dices)
        {
            int total = 0;
            for (int i = 0; i < dices.Length; i++)
            {
                total += dices[i].Rolled_number;
            }
            return total;
        }

        public int CalcYatzy(Dices[] dices)
        {
            int t = 0;
            int[] tempArray = new int[5];
            int totalTempNumber = 0;

            for (int i = 0; i < dices.Length; i++)
            {
                if (dices[i].Rolled_number == dices[0].Rolled_number && dices[i].Rolled_number == dices[1].Rolled_number && dices[i].Rolled_number == dices[2].Rolled_number && dices[i].Rolled_number == dices[3].Rolled_number && dices[i].Rolled_number == dices[4].Rolled_number)
                {
                    tempArray[t] = dices[i].Rolled_number * 5;
                    t++;
                }
            }

            for (int p = 0; p < tempArray.Length; p++)
            {
                if (tempArray[p] > totalTempNumber)
                {
                    totalTempNumber = tempArray[p];
                }
            }
            if (totalTempNumber != 0)
            {
                totalTempNumber = 50;
            }

            return totalTempNumber;
        }

        #endregion    

    }
}
