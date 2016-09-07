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


            PrintMyNumbersInfo(myNumbers, lotteryMax, lotteryMin);
            


            Draw(myNumbers, lotteryMin, lotteryMax);

            Console.ReadKey();
        }

        /// <summary>
        /// print some informations about my numbers
        /// </summary>
        /// <param name="myNumbers"></param>
        /// <param name="lotteryMax"></param>
        /// <param name="lotteryMin"></param>
        static void PrintMyNumbersInfo(int[] myNumbers, int lotteryMax, int lotteryMin)
        {
            Array.Sort(myNumbers);
            Console.WriteLine("\n----------My numbers");
            foreach (var item in myNumbers)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("amount: " + myNumbers.Length);
            Console.WriteLine("max: " + lotteryMax);
            Console.WriteLine("min: " + lotteryMin);
            Console.WriteLine("----------END");
        }

        /// <summary>
        /// print some information about the drawn
        /// </summary>
        /// <param name="numsDrawn"></param>
        /// <param name="guessed"></param>
        /// <param name="partialGuessed"></param>
        static void PrintNumbersDrawnInfo(int[] numsDrawn, bool guessed, int partialGuessed)
        {
            Array.Sort(numsDrawn);
            Console.WriteLine("\n----------numbers drawn");
            Console.WriteLine("numbers drawn: ");
            foreach (var item in numsDrawn)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("numbers guessed: " + partialGuessed);
            Console.WriteLine("all guessed: " + guessed);
            Console.WriteLine("----------END");
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

        static void Draw(int[] myN, int min, int max)
        {
            Random rand = new Random(new Random().Next());
            int[] numsDrawn = new int[myN.Length];
            bool cicle = false;

            #region  draw
            for (int i = 0; i < numsDrawn.Length; i++)
            {
                do
                {
                    int temp = rand.Next(min, max);
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
            #endregion

            #region  check
            bool guessed = true;
            int partialGuessed = 0;
            for (int i = 0; i < myN.Length; i++)
            {
                bool doCicle = true;
                for (int x = 0; x < numsDrawn.Length && doCicle; x++)
                {
                    if (myN[i] == numsDrawn[x])
                    {
                        guessed = true;  //my number exists in the number drawn, so I can exit the cicle
                        partialGuessed++;
                        doCicle = false;
                    }
                    else
                        guessed = false;
                }
            }
            #endregion


            PrintNumbersDrawnInfo(numsDrawn, guessed, partialGuessed);
        }

        static void IncreaseCounter()
        {
            //critical section
            lock(locker)
            {
                attempts++;
            }
        }
    }
}
