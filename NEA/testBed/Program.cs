using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testBed
{
    internal class Program
    {
        static void printSet(int[] set)
        {
            for (int i = 0; i < set.Length; i++)
            {
                Console.Write(set[i] + "|");
            }
            Console.WriteLine();
        }
        static void changeSet(int[] set)
        {
            set[0] = 3; set[1] = 4; set[2] = 5;
        }
        static int[] stitch(int[] firstArray, int middleValue, int[] secondArray) //stitches together two arrays and a value in the order inputted
        {
            int[] newArray = new int[firstArray.Length + 1 + secondArray.Length];
            for (int i = 0; i < firstArray.Length; i++)
            {
                newArray[i] = firstArray[i];
            }
            newArray[firstArray.Length] = middleValue;

            int x = 0;
            for (int i = firstArray.Length + 1; i < newArray.Length; i++)
            {
                newArray[i] = secondArray[x];
                x++;
            }
            return newArray;
        }
        static void Main(string[] args)
        {
            int[] ints1 = { 1, 2, 3 };
            printSet(ints1);
            Console.ReadKey();
            Console.WriteLine("4");
            Console.ReadKey();
            int[] ints2 = { 5, 6, 7 };
            printSet(ints2);
            Console.ReadKey();
            printSet(stitch(ints1,4,ints2));
            Console.ReadKey();
        }
    }
}
