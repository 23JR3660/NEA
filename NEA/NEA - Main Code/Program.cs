using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace NEA___Main_Code
{
    internal class Program
    {
        static int SortID = 1; //global key to organise the order of multiple tests completed in one instance
        static int[] makeSet(bool evenlyDistributed, bool ordered, int size, int range)
        {
            int[] set = new int[size];
            Random rnd = new Random();
            if (evenlyDistributed && ordered)
            {
                for (int i = 0; i < size; i++)
                {
                    set[i] = i + 1; //i+1 is inserted so as to not include 0 in sets for the sake of readability
                }
                return set;
            }
            if (evenlyDistributed)
            {
                for (int i = 0; i < size; ++i)
                {
                    set[i] = i + 1;
                }
                shuffle(set);
                return set;
            }
            for (int i = 0; i < size; ++i)
            {
                set[i] = rnd.Next(1, range);
            }
            return set;

        }
        static void swap(int[] set, int indexOne, int indexTwo)
        {
            int temp;
            temp = set[indexOne];
            set[indexOne] = set[indexTwo];
            set[indexTwo] = temp;
        }
        static void shuffle(int[] set) //completes a random swap one time for every element that the array has
        {
            Random rnd = new Random();
            int index;
            for (int i = 0; i < set.Length; i++)
            {
                index = rnd.Next(0, set.Length);
                swap(set, i, index);
            }
        }
        static void printSet(int[] set)
        {
            for (int i = 0; i < set.Length; i++)
            {
                Console.Write(set[i] + "|");
            }
            Console.WriteLine();
        }

        static int[] intListToArray(List<int> set) // converts an integer list to an integer array
        {
            int[] array = new int[set.Count];
            for (int i = 0; i < set.Count; i++)
            {
                array[i] = set[i];
            }
            return array;
        }
        static List<int> intArrayToList(int[] set) //converts an integer array to an integer list
        {
            List<int> list = new List<int>();
            for (int i = 0; i < set.Length; i++)
            {
                list.Add(set[i]);
            }
            return list;
        }
        static int[] doCountingSort(int[] set) //carries out a full counting sort with a given integer array and returns the sorted array
        {
            int maxValue = 0;
            for(int i = 0; i < set.Length; i++)
            {
                if(set[i] > maxValue) maxValue = set[i];
            }
            int[] occurences = new int[maxValue + 1];
            for (int i = 0; i < occurences.Length; i++)
            {
                occurences[i] = 0;
            }
            for (int i = 0; i < set.Length; i++)
            {
                occurences[set[i]]++;
            }
            List<int> sorted = new List<int>();
            for (int i = 0; i < occurences.Length; i++)
            {
                for (int j = 0; j < occurences[i]; j++)
                {
                    sorted.Add(i);
                }
            }
            return intListToArray(sorted);
        }
        
        
        static int[] merge(int[] left, int[] right)
        {

            int resultLength = right.Length + left.Length;
            int[] result = new int[resultLength];
            int indexLeft = 0, indexRight = 0, indexResult = 0;

            while (indexLeft < left.Length || indexRight < right.Length)
            {
                if (indexLeft < left.Length && indexRight < right.Length)
                {
                    if (left[indexLeft] <= right[indexRight])
                    {
                        result[indexResult] = left[indexLeft];
                        indexLeft++;
                        indexResult++;
                    }
                    else
                    {
                        result[indexResult] = right[indexRight];
                        indexRight++;
                        indexResult++;
                    }
                }
                else if (indexLeft < left.Length)
                {
                    result[indexResult] = left[indexLeft];
                    indexLeft++;
                    indexResult++;
                }
                else if (indexRight < right.Length)
                {
                    result[indexResult] = right[indexRight];
                    indexRight++;
                    indexResult++;
                }
            }
            return result;
        }
        static int[] doMergeSort(int[] set) // recursively carries out a Merge sort
        {
            int[] left, right;
            if (set.Length <= 1) //base case for when the set has been split into single elements
                return set;


            int midPoint = set.Length / 2;
            left = new int[midPoint];
            if (set.Length % 2 == 0)
                right = new int[midPoint];
            else
                right = new int[midPoint + 1];

            for (int i = 0; i < midPoint; i++)
            {
                left[i] = set[i];
            }

            int x = 0;

            for (int i = midPoint; i < set.Length; i++)
            {
                right[x] = set[i];
                x++;
            }

            left = doMergeSort(left);
            right = doMergeSort(right);
            return merge(left, right);
        }

        static int partition(int[] set, int low, int high) //completes the pivoting function and returns an int for the position of the pivot
        {
            int pivot = set[low];//pivot chosen as first number

            int k = high;
            for (int i = high; i > low; i--)
            {
                if (set[i] > pivot)
                {
                    swap(set, i, k);
                    k--;
                }
            }
            swap(set, k, low);
            return k;
        }
        static void doQuickSort(int[] set, int low, int high) //low and high are the indices between which the quickSort is to be carried out in a given instance
        {
            if (low < high)
            {
                int index = partition(set, low, high);
                doQuickSort(set, low, index - 1);
                doQuickSort(set, index + 1, high);
            }
        }
        
        static bool BubblePass(int[] set, int endPoint) //carries out a single pass of bubble sort and returns a boolean for if it had to do a swap
        {
            bool swapMade = false;
            int temp;
            for (int i = 0; i < endPoint; i++)
            {
                if (set[i] > set[i + 1])
                {
                    temp = set[i];
                    set[i] = set[i + 1];
                    set[i + 1] = temp;
                    swapMade = true;
                }
            }
            return swapMade;
        }
        static void doBubbleSort(int[] set) // carries out a bubble sort with a given integer array
        {
            for(int i = set.Length-1;i > 0; i--)
            {
                BubblePass(set, i);
            }
        }
        static void StoreResult(int SortKey, int range, int[] set, float timeTaken, long memoryUsed)
        {
            string[] SortNames = { "Bubble Sort", "Quick Sort", "Merge Sort", "Counting Sort" };
            using (StreamWriter writer = new StreamWriter("AlgorithmLog.txt", true))
            {
                writer.WriteLine(SortID + ")" + SortNames[SortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");
                Console.WriteLine(SortID + ")" + SortNames[SortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");

            }
            using (StreamWriter writer = new StreamWriter("SortedAlgorithms.txt", true))
            {
                writer.Write(SortKey + ")");
                for (int i = 0; i < set.Length; i++)
                {
                    writer.Write(set[i] + "|");
                }
                writer.WriteLine(); writer.WriteLine();
            }
            using (StreamWriter writer = new StreamWriter("times.txt", true))
            {
                writer.WriteLine(timeTaken);
            }
            SortKey++;
        }
        static void demoTest() //demo test for all currently programmed algorithms
        {
            int[] setty;
            Stopwatch sw = new Stopwatch();

            //Bubble Sort

            setty = makeSet(true, false, 10000, 10000); //first number is the length of array and the second number is the range of numbers generated. Do not change the booleans in this case
            sw.Start();
            doBubbleSort(setty);
            sw.Stop();

            //Results of Bubble Sort

            Console.WriteLine(SortID + ")" + "Bubble Sort" + " Size " + setty.Length + " Completed in " + sw.ElapsedMilliseconds + "ms"); SortID++;
            sw.Reset();


            Console.ReadKey();



            //Merge Sort

            setty = makeSet(true, false, 10000, 10000); //first number is the length of array and the second number is the range of numbers generated. Do not change the booleans in this case
            sw.Start();
            setty = doMergeSort(setty);
            sw.Stop();

            //Results of Merge Sort

            Console.WriteLine(SortID + ")" + "Merge Sort" + " Size " + setty.Length + " Completed in " + sw.ElapsedMilliseconds + "ms"); SortID++;
            sw.Reset();

            Console.ReadKey();



            //Quick Sort

            setty = makeSet(true, false, 10000, 10000); //first number is the length of array and the second number is the range of numbers generated. Do not change the booleans in this case
            sw.Start();
            doQuickSort(setty, 0, setty.Length - 1); //the 2nd and 3rd parameters only exist because this subroutine is recursive. Do NOT change them here
            sw.Stop();

            //Results of Quick Sort

            Console.WriteLine(SortID + ")" + "Quick Sort" + " Size " + setty.Length + " Completed in " + sw.ElapsedMilliseconds + "ms"); SortID++;
            sw.Reset();


            Console.ReadKey();



            //Counting Sort

            setty = makeSet(true, false, 10000, 10000); //first number is the length of array and the second number is the range of numbers generated. Do not change the booleans in this case
            sw.Start();
            setty = doCountingSort(setty);
            sw.Stop();

            //Results of Counting Sort

            Console.WriteLine(SortID + ")" + "Counting Sort" + " Size " + setty.Length + " Completed in " + sw.ElapsedMilliseconds + "ms"); SortID++;
            sw.Reset();

            Console.ReadKey();
            Console.WriteLine("All tests are over.");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            //Counting Sort
            int[] setty;
            Stopwatch sw = new Stopwatch();
            
            setty = makeSet(true, false, 100000, 100000); //first number is the length of array and the second number is the range of numbers generated. Do not change the booleans in this case
            printSet(setty);
            sw.Start();
            doBubbleSort(setty);
            sw.Stop();
            printSet(setty);

            //Results of Counting Sort

            Console.WriteLine(SortID + ")" + "Bubble Sort" + " Size " + setty.Length + " Completed in " + sw.ElapsedMilliseconds + "ms"); SortID++;
            sw.Reset();

            Console.ReadKey();
        }
    }
}

