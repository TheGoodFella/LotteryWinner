using System;
using System.Threading;
using System.Collections.Generic;

namespace LotteryWinner
{
    class Program
    {
        /// <summary>
        /// locker obj
        /// </summary>
        static private Object locker = new Object();

        static private char separator = ',';

        static UInt64 attempts = 0;

        static Random rando = new Random();

        static void Main(string[] args)
        {
            #region variables
            //thread list
            List<Thread> threads = new List<Thread>();
            
            //minimum number of tha lottery range
            int lotteryMin;
            
            //maximum number of tha lottery range
            int lotteryMax;

            //numbers I chose
            int[] myNumbers;

            int myNumbersSize;

            #endregion

            lotteryMin = int.Parse(Prompt("\nminimum number drawn: "));
            lotteryMax = int.Parse(Prompt("\nmaximum number drawn: "));

            myNumbers = FillMyNumbers(Prompt("\nchoose your numbers separated by a comma: "));
            myNumbersSize = myNumbers.Length;

            Check(myNumbers, lotteryMin, lotteryMax);

            Console.ReadKey();
        }

        /// <summary>
        /// print some informations about my numbers
        /// </summary>
        /// <param name="myNumbers"></param>
        /// <param name="lotteryMax"></param>
        /// <param name="lotteryMin"></param>
        static void PrintMyNumbersInfo(int[] myNumbers, int lotteryMax, int lotteryMin, int[] numsDrawn, int partialGuessed, UInt64 attempts)
        {
            Console.WriteLine("\n----------My numbers");
            foreach (var item in myNumbers)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("amount: " + myNumbers.Length);
            Console.WriteLine("max: " + lotteryMax);
            Console.WriteLine("min: " + lotteryMin);
            Console.WriteLine("----------END");
            
            Console.WriteLine("\n----------numbers drawn");
            Console.WriteLine("numbers drawn: ");
            foreach (var item in numsDrawn)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("numbers guessed: " + partialGuessed);
            Console.WriteLine("----------END");

            Console.Write(attempts + " attempts");
        }

        /// <summary>
        /// print text and return the text written before pressing ENTER
        /// </summary>
        /// <param name="text"></param>
        /// <returns>text returned</returns>
        static string Prompt(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        /// <summary>
        /// create an array filled by the number I chose, written in the string separated by comma 
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        static int[] FillMyNumbers(string numbers)
        {
            string[] Snumbers = numbers.Split(separator);
            int[] n = new int[Snumbers.Length];

            for (int i = 0; i < Snumbers.Length; i++)
            {
                n[i] = int.Parse(Snumbers[i].Trim());
            }
            return n;
        }

        static int[] Draw(int[] myN, int min, int max)
        {
            
            int[] numsDrawn = new int[myN.Length];
            bool cicle = false;

            for (int i = 0; i < numsDrawn.Length; i++)
            {
                do
                {

                    

                    int temp = rando.Next(min, max);
                    if (Array.IndexOf(numsDrawn, temp) == -1)
                    {
                        numsDrawn[i] = temp;
                        cicle = false;
                    }
                    else
                    {
                        cicle = true;
                    }
                } while (cicle);
            }

            return numsDrawn;
        }

        static void Check(int[] myN, int min, int max)
        {
            Console.WriteLine("Likely To Win: " + LikelyToWin(min, max, myN.Length));
            Console.WriteLine("Loading...");
            
            int partialGuessed = 0;
            int[] numsDrawn = null;
            while (partialGuessed < myN.Length)
            {
                numsDrawn = null;
                numsDrawn = Draw(myN, min, max);

                IncreaseCounter(); //aaand +1 attempt

                partialGuessed = 0;

                bool doCicle = true;
                for (int i = 0; i < myN.Length; i++)
                {
                    doCicle = true;
                    for (int x = 0; x < numsDrawn.Length && doCicle; x++)
                    {
                        if (myN[i] == numsDrawn[x])
                        {
                            partialGuessed++;
                            doCicle = false;
                        }
                    }
                }
            }
            
            PrintMyNumbersInfo(myN, max, min, numsDrawn, partialGuessed, attempts);
        }

        static double LikelyToWin(int min,int max, int amountOfNChose)
        {
            int range = max - min + 1;
            return Math.Pow(range, amountOfNChose);
        }

        static void IncreaseCounter()
        {
            //critical section
            lock (locker)
            {
                attempts++;
            }
        }
    }
}
