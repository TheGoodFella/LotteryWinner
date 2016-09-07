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

        static void Main(string[] args)
        {
            //thread list
            List<Thread> threads = new List<Thread>();
            
            //minimum number of tha lottery range
            int lotteryMin = 1;
            
            //maximum number of tha lottery range
            int lotteryMax = 90;

            //numbers I chose
            int[] myNumbers;

            Console.Write("choose your numbers separated by a comma: ");
            myNumbers = FillMyNumbers(Console.ReadLine());

            foreach (var item in myNumbers)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadKey();
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
                n[i] = int.Parse(Snumbers[i]);
            }
            return n;
        }

        static void IncreaseCounter()
        {
            //critical section
            lock(locker)
            {

            }
        }
    }
}
