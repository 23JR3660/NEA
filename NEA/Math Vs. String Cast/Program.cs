using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Math_Vs.String_Cast
{
    internal class Program
    {
        static double findSum(double[] array)
        {
            double sum = 0;
            for(int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }
        static void Main(string[] args)
        {
            Random rng = new Random();

            int randomNumber, digitToBeFound, digitValue;
            Stopwatch sw = new Stopwatch();

            double[] stringCastAverageTimes = new double[10000]; //times taken for performing a string cast and indexing function 1000 times
            double[] mathAverageTimes = new double[10000]; //times taken for performing the modulus and the div functions with powers of ten to find the digit 1000 times

            for(int a = 0; a<10; a++)
            {
                for (int i = 0; i < 10000; i++) //Computes the total time to find the nth digit of a number 10000 times
                {
                    sw.Start();
                    for (int j = 0; j < 10000; j++) //Find the nth digit of a number for 1000 randomly generated numbers
                    {
                        randomNumber = rng.Next(1, 10000);
                        digitToBeFound = randomNumber.ToString().Length - rng.Next(0, randomNumber.ToString().Length);
                        digitValue = (int)((randomNumber % Math.Pow(10, digitToBeFound) / Math.Pow(10, digitToBeFound - 1)));
                        //Console.WriteLine($"The digit in the  {Math.Pow(10,digitToBeFound - 1)}s column of {randomNumber} is {digitValue}");
                    }
                    sw.Stop(); 
                    if (i % 1000 == 0 && i != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Time taken for 10000 operations with string cast is: " + sw.ElapsedMilliseconds + "ms");
                        stringCastAverageTimes[i] = sw.ElapsedMilliseconds;
                        sw.Restart();
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Average time taken for 10000 operations with math operation is: " + findSum(stringCastAverageTimes) / 10000);

                System.Threading.Thread.Sleep(3000);

                for (int i = 0; i < 10000; i++) //Computes the total time to find the nth digit of a number 1000 times
                {
                    sw.Start();
                    for (int j = 0; j < 10000; j++) //Find the nth digit of a number for 1000 randomly generated numbers
                    {
                        randomNumber = rng.Next(1, 10000);
                        digitToBeFound = rng.Next(0, randomNumber.ToString().Length);
                        digitValue = int.Parse(randomNumber.ToString().Substring(digitToBeFound, 1));
                        //Console.WriteLine($"The digit in the index [{digitToBeFound}] of {randomNumber} is {digitValue}");
                    }
                    sw.Stop();
                    if(i %  1000 == 0 && i != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Time taken for 10000 operations with string cast is: " + sw.ElapsedMilliseconds + "ms");
                        stringCastAverageTimes[i] = sw.ElapsedMilliseconds;
                        sw.Restart();
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Average time taken for 10000 operations with string cast is: " + findSum(stringCastAverageTimes) / 10000);

                System.Threading.Thread.Sleep(3000);
            }
            

            

        }
    }
    
}
