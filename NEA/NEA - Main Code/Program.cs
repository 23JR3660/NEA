using System; //test
using System.Collections.Generic; //test2
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA___Main_Code
{
    internal class Program
    {
        static int SortKey = 1; //global key to organise the order of multiple tests completed in one instance
        static int[] makeSet(bool evenlydis, bool ordered, int size, int range)
        {
            int[] set = new int[size];
            Random rnd = new Random();
            if (evenlydis && ordered)
            {
                for (int i = 0; i < size; i++)
                {
                    set[i] = i;
                }
                return set;
            }
            if (evenlydis)
            {
                for (int i = 0; i < size; ++i)
                {
                    set[i] = i;
                }
                return shuffle(set);
            }
            for (int i = 0; i < size; ++i)
            {
                set[i] = rnd.Next(1, range);
            }
            return set;

        }
        static int[] shuffle(int[] set) //completes a random swap one time for every element that the array has
        {
            Random rnd = new Random();
            int temp;
            int index;
            for (int i = 0; i < set.Length; i++)
            {
                index = rnd.Next(0, set.Length);
                temp = set[i];
                set[i] = set[index];
                set[index] = temp;
            }
            return set;
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
            for(int i = 0; i < set.Length; i++)
            {
               list.Add(set[i]);
            }
            return list;
        }

        static int[] partitionArray(int[] set, int startIndex, int endIndex) // returns the section of an array between the startIndex and endIndex
        {
            int[] setSection = new int[endIndex - startIndex + 1];
            for(int i = 0; i < setSection.Length; i++)
            {
                setSection[i] = set[i + startIndex];
            }
            return setSection;
        }
        static int[] doCountingSort(int[] set, int range) //carries out a full counting sort with a given integer array and returns sorted array
        {

            int[] occurences = new int[range + 1];
            for (int i = 1; i < occurences.Length; i++)
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
        static int[] doBubbleSort(int[] set, int range) // carries out a bubble sort with a given integer array and returns sorted array
        {
            while (BubblePass(ref set)) ; //will run passes of bubble sort until false is returned signalling a swap was not neccessary
            return set;
        }
        static int[] doMergeSort(int[] set) // recursively carries out a Merge sort
        {
            int[] left;
            int[] right;
            int[] result = new int[set.Length];
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
            result = merge(left, right);
            return result;
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
        static void StoreResult(string SortName, int range, int[] set, float timeTaken, long memoryUsed)
        {

            using (StreamWriter writer = new StreamWriter("AlgorithmLog.txt", true))
            {
                writer.WriteLine(SortKey + ")" + SortName + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");
                Console.WriteLine(SortKey + ")" + SortName + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");

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
        static bool BubblePass(ref int[] set) //carries out a single pass of bubble sort and returns a boolean for if it had to do a swap
        {
            bool swapMade = false;
            int temp;
            for (int i = 0; i < set.Length - 1; i++)
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
        static void Main(string[] args)
        {
            int[] setty = makeSet(true, false, 10, 10);
            printSet(setty);
            setty = doMergeSort(setty);
            printSet(setty);
            Console.ReadKey();
        }
    }
}

