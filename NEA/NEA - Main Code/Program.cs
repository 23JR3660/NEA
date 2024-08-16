using System; //test
using System.Collections.Generic; //test2
using System.IO;
using System.Runtime.InteropServices;

namespace NEA___Main_Code
{
    internal class Program
    {
        static int SortKey = 1; //global key to organise the order of multiple tests completed in one instance
        public struct arrayPair
        {
            public int[] left, right;
        }
        static int[] makeSet(bool evenlyDistributed, bool ordered, int size, int range)
        {
            int[] set = new int[size];
            Random rnd = new Random();
            if (evenlyDistributed && ordered)
            {
                for (int i = 0; i < size; i++) 
                {
                    set[i] = i+1; //i+1 is inserted so as to not include 0 in sets for the sake of readability
                }
                return set;
            }
            if (evenlyDistributed)
            {
                for (int i = 0; i < size; ++i)
                {
                    set[i] = i+1; 
                }
                shuffle(set);
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
            int temp;
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
            for(int i = 0; i < set.Length; i++)
            {
               list.Add(set[i]);
            }
            return list;
        }
        static int[] stitch(int[] firstArray, int middleValue, int[] secondArray) //stitches together two arrays and a value in the order inputted
        {
            int[] newArray = new int[firstArray.Length + 1 + secondArray.Length];
            for(int i = 0; i < firstArray.Length; i++)
            {
                newArray[i] = firstArray[i];
            }
            newArray[firstArray.Length] = middleValue;

            int x = 0;
            for(int i = firstArray.Length + 1;  i < newArray.Length; i++)
            {
                newArray[i] = secondArray[x];
                x++;
            }
            return newArray;
        }
        static int[] stitch(int[] array, int value) //stitches together an array and a value in the order inputted
        {
            int[] newArray = new int[array.Length + 1];
            for(int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            newArray[newArray.Length-1] = value;
            return newArray;
        }
        static int[] stitch( int value, int[] array) //stitches together an array and a value in the order inputted
        {
            int[] newArray = new int[array.Length + 1];
            newArray[0] = value;
            for (int i = 1; i < newArray.Length; i++)
            {
                newArray[i] = array[i-1];
            }
            return newArray;
        }
        static void doCountingSort(int[] set, int range) //carries out a full counting sort with a given integer array. type is void since inputting an array takes the reference
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
        }
        static void doBubbleSort(int[] set, int range) // carries out a bubble sort with a given integer array
        { 
            while (BubblePass(ref set)) ; //will run passes of bubble sort until false is returned signalling a swap was not necessary
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
        static arrayPair pivot(int[] set) //returns the a set of arrays for the numbers to the left and right of the pivot (first number in array)
        {
            int pivotValue = set[0]; 
            List<int> left = new List<int>(); //list is used here for the ability to add as the program progresses, useful for adding lower numbers then pivot then higher numbers
            List<int> right = new List<int>();

            arrayPair result = new arrayPair();

            int pivotTwinsInserted = 1; int pivotTwinsPresent = 0; //the term pivot twins is used here to denote any value equal to the pivot so as to keep track of numbers the same value as the pivot

            for(int i = 1; i < set.Length; i++)
            {
                if(set[i] == pivotValue) pivotTwinsPresent++;
            }
            for(int i  = 1; i < set.Length; i++)
            {
                if(set[i] < pivotValue) left.Add(set[i]);
                if(set[i] == pivotValue && pivotTwinsInserted < pivotTwinsPresent) left.Add(set[i]);
            }
            for(int i = 1; i < set.Length; i++)
            {
                if (set[i] > pivotValue) right.Add(set[i]); 
            }
            result.left = intListToArray(left); result.right = intListToArray(right);
            printSet(stitch(result.left, pivotValue, result.right));
            return result;
        }
        static int[] doQuickSort(int[] set)
        {
            if(set.Length <= 1) return set; //base case for empty array
            int pivotValue = set[0];
            
            arrayPair leftAndRight = pivot(set);
            if(leftAndRight.left.Length == 0) return doQuickSort(leftAndRight.right);
            if (leftAndRight.right.Length == 0) return doQuickSort(leftAndRight.left);
            return doQuickSort(stitch(doQuickSort(leftAndRight.left),pivotValue,doQuickSort(leftAndRight.right)));
        }
        static int partition(int[] set, int low, int high)
        {
            int pivot = set[low];//pivot chosen as first number

            int i = (low-1);
            for(int j = low; j <= high -1; j++)
            {
                if (set[j] < pivot)
                {
                    i++;
                    swap(set,i,j);
                }
            }
            swap(set, i + 1, low);
            return i + 1;
        }
        static void quickSort(int[] set, int low, int high)
        {
            if(low < high)
            {
                int index = partition(set, low, high);
                quickSort(set, low, index - 1);
                quickSort(set, index _ 1, high);
            }
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
            Console.Readkey();
            quickSort(setty, 0, setty.Length - 1);
            printSet(setty);
            Console.ReadKey();
        }
    }
}

