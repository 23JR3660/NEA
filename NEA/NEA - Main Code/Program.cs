using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace NEA___Main_Code
{
    internal class Program
    {
        static int sortID = 1; // Global key to organise the order of multiple tests completed in the same instance.

        // Returns an integer array based on the parameters provided.
        static int[] makeSet(bool evenlyDistributed, bool preSorted, bool preInverseSorted, int size, int range) 
        {
            int[] set = new int[size];
            Random rnd = new Random();

            if (preSorted || evenlyDistributed)
            {
                for (int i = 0; i < size; i++)
                {
                    set[i] = i + 1; // Fills the array with values from 1 to 'size' inclusive in that order.
                }
                if(evenlyDistributed)shuffle(set); // Shuffles this if not pre-sorted.
                
            }

            else if (preInverseSorted)
            {
                for (int i = 0; i < size; i++)
                {
                    set[size - i -1] = i + 1;  // Fills the array with values from 'size' to 1 inclusive in that order (reverse order).
                }
            }

            else
            {
                for (int i = 0; i < size; ++i)
                {
                    set[i] = rnd.Next(1, range +1); // If none of the other conditions are met, the only remaining condition is the set being random. This returns a set populated with numbers from 1 to 'size' inclusive.
                }
            }
            
            return set;

        }

        // Swaps the values at the given indices.
        static void swap(int[] set, int indexOne, int indexTwo)
        {
            int temp;
            temp = set[indexOne]; // Temporarily store the value at indexOne.

            set[indexOne] = set[indexTwo]; // Replace value at indexOne with value at indexTwo.

            set[indexTwo] = temp; // Replace value at indexTwo with the former indexOne value.
        }

        // Completes an amount of swaps equal to the amount of elements present in the set.
        static void shuffle(int[] set) 
        {
            Random rnd = new Random();
            int index;

            for (int i = 0; i < set.Length; i++)
            {
                index = rnd.Next(0, set.Length);
                swap(set, i, index);
            }

        }

        // Completes an amount of swaps equal to the parameter provided.
        static void shuffle(int[] set, int swaps)
        {
            Random rnd = new Random();
            int index;

            for (int i = 0; i < swaps; i++)
            {
                index = rnd.Next(0, set.Length);
                swap(set, i, index);
            }

        }

        // Prints the set to the console, with each element separated by a "|".
        static void printSet(int[] set)  
        {
            for (int i = 0; i < set.Length; i++)
            {
                Console.Write(set[i] + "|");
            }
            Console.WriteLine();
        }

        // Takes in a list of integers and returns an array of integers.
        static int[] intListToArray(List<int> list)
        {
            int[] array = new int[list.Count]; // Creates an array the same size as the list.

            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i]; // Copies each value from the list into the array.
            }
            return array;
        }

        // Takes in an array of integers and returns a list of integers.
        static List<int> intArrayToList(int[] array)
        {
            List<int> list = new List<int>(); // Creates an empty list.

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]); // Copies each value from the array into the list.
            }
            return list;
        }

        //Calculates and returns the largest value in the inputted integer array.
        static int findMax(int[] set)
        {
            int maxValue = 0;

            for(int i = 0; i < set.Length; i++)
            {
                if(set[i] > maxValue) maxValue = set[i];
            }

            return maxValue;
        }

        //Calculates and returns the largest value in the inputted integer list.
        static int findMax(List<int> set) 
        {
            int maxValue = 0;

            for (int i = 0; i < set.Count; i++)
            {
                if (set[i] > maxValue) maxValue = set[i];
            }

            return maxValue;
        }

        //Performs a Counting Sort on the given array and returns the sorted array.
        static int[] doCountingSort(int[] set)
        {
            int[] occurrences = new int[findMax(set) + 1]; //Creates an array the size of the highest value in the set plus 1.

            for (int i = 0; i < occurrences.Length; i++)
            {
                occurrences[i] = 0;  // Initialize all elements in the occurrences array to 0 so that they can be incremented.
            }

            // Counts the occurrences of each number in the array.
            for (int i = 0; i < set.Length; i++)
            {
                occurrences[set[i]]++;
            }

            List<int> sorted = new List<int>(); // Creates an empty list to store the sorted elements.

            // Add elements to the sorted list based on their occurrence count.
            for (int i = 0; i < occurrences.Length; i++)
            {
                for (int j = 0; j < occurrences[i]; j++)
                {
                    sorted.Add(i);
                }
            }

            return intListToArray(sorted); // Convert the sorted list to an array and return it.
        }
        //Returns the nth digit
        static int findDigit(int number, int n, int maxLength)
        {
           string paddedNumber = number.ToString().PadLeft(maxLength, '0');
           return int.Parse(paddedNumber.Substring(n, 1));
            
        }
        static int[] doRadixPass(int[] set, int digit, int maxLength)
        {
            List<int> result = new List<int>(); //List used so it can be added to sequentially.
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < set.Length; j++)
                {
                    if(findDigit(set[j], digit, maxLength) == i)
                    {
                        result.Add(set[j]);
                    }
                }
            }
            return intListToArray(result);
        }

        static int[] doLSDRadixSort(int[] set)
        {
            int max = findMax(set);
            int maxLength = max.ToString().Length;
            for(int i = maxLength-1; i >= 0 ; i--)
            {
                set = doRadixPass(set, i, maxLength);
            }
            return set;
        }
        // Merges two sorted arrays into a single sorted array.
        static int[] merge(int[] left, int[] right)
        {

            int resultLength = right.Length + left.Length; // Calculate the length of the resulting array.
            int[] result = new int[resultLength];  // Create an array of calculated length to store the merged result.
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

            return result; // Return the merged array.
        }

        // Performs a Merge Sort on the given array and returns the sorted array.
        static int[] doMergeSort(int[] set)
        {
            
            if (set.Length <= 1) // Base case: if the array has one or fewer elements, it's already sorted.
                return set;


            int[] left, right;

            int midPoint = set.Length / 2; // Determine the midpoint to split the array by.

            left = new int[midPoint]; // Create the left subarray

            //Create the right subarray starting from the correct index.

            if (set.Length % 2 == 0)
            {
                right = new int[midPoint];
            }
            else
            {
                right = new int[midPoint + 1];
            }
            
            // Fill the left subarray with the first half of the original array.
            for (int i = 0; i < midPoint; i++)
            {
                left[i] = set[i];
            }

            int x = 0;

            // Fill the right subarray with the second half of the original array.
            for (int i = midPoint; i < set.Length; i++)
            {
                right[x] = set[i];
                x++;
            }

            left = doMergeSort(left); // Recursively sort the left subarray.

            right = doMergeSort(right); // Recursively sort the right subarray.

            return merge(left, right); // Merge the sorted subarrays and return the result.
        }

        // Partitions the array around a pivot and returns the pivot's final position.
        static int partition(int[] set, int low, int high)
        {
            int pivot = set[low]; // Choose the first element as the pivot

            int rightBoundary = high; // When a number larger than the pivot is found, this is where it is to be placed.

            //Move elements greater than the pivot to the right
            for (int i = high; i > low; i--)
            {
                if (set[i] > pivot)
                {
                    swap(set, i, rightBoundary); // Move current element if greater than pivot.
                    rightBoundary--; // Move right boundary to the left.
                }
            }
            swap(set, rightBoundary, low); // Place the pivot in its correct position which must be the rightBoundary since all numbers greater than the pivot were move to the right of THIS index.

            return rightBoundary; // Return the final position of the pivot
        }

        // Performs a Quick Sort on the array between the given indices (low to high).
        static void doQuickSort(int[] set, int low, int high)
        {
            if (low < high)
            {
                int index = partition(set, low, high); // Partition the array and get the pivot's position.
                doQuickSort(set, low, index - 1); // Recursively sort the left subarray.
                doQuickSort(set, index + 1, high); // Recursively sort the right subarray.
            }
        }

        // Performs a single pass of Bubble Sort and returns true if a swap was made.
        static bool BubblePass(int[] set, int endPoint) 
        {
            bool swapMade = false;

            // If the current element is greater than the next, swap them.
            for (int i = 0; i < endPoint; i++)
            {
                if (set[i] > set[i + 1])
                {
                    swap(set,i,i + 1);
                    swapMade = true; // Mark that a swap was made.
                }
            }
            return swapMade;
        }

        // Performs a full Bubble Sort on the given array.
        static void doBubbleSort(int[] set)
        {
            for(int i = set.Length-1;i > 0; i--)
            {
                if (!BubblePass(set, i)) return;
            }
        }

        // Performs an Insertion Sort on the given array.
        static void doInsertionSort(int[] set)
        {
            int elementInserting; // The element to be inserted.
            int insertionIndex;

            for (int i = 1; i < set.Length; i++)
            {
                elementInserting = set[i];
                insertionIndex = i - 1;
                // Move up elements one position to the right
                while(insertionIndex > -1 && set[insertionIndex] > elementInserting)
                {
                    set[insertionIndex + 1] = set[insertionIndex];
                    insertionIndex--;
                }
                set[insertionIndex +1] = elementInserting; // Insert the selected element at its correct position. This is insertionIndex + 1 since everything has been moved up one.
            }
        }
        // Stores the result of a sorting operation in two log files, with memory used included.
        static void StoreResult(int sortKey, int range, int[] set, float timeTaken, long memoryUsed)
        {
            string[] sortNames = { "Bubble Sort", "Merge Sort", "Counting Sort", "Quick Sort", "Insertion Sort", "LSD Radix Sort", "MSD Radix Sort", "Heap Sort" };

            // Log the result details to "AlgorithmLog.txt".
            using (StreamWriter writer = new StreamWriter("AlgorithmLog.txt", true))
            {
                writer.WriteLine(sortID + ")" + sortNames[sortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");
                Console.WriteLine(sortID + ")" + sortNames[sortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms with " + memoryUsed + "B total memory");

            }

            // Log the sorted array to "SortedAlgorithms.txt".
            using (StreamWriter writer = new StreamWriter("SortedAlgorithms.txt", true))
            {
                writer.Write(sortID + ")");
                for (int i = 0; i < set.Length; i++)
                {
                    writer.Write(set[i] + "|");
                }
                writer.WriteLine(); writer.WriteLine();
            }

            // Log the time taken to "times.txt".
            using (StreamWriter writer = new StreamWriter("times.txt", true))
            {
                writer.WriteLine(timeTaken);
            }
            sortID++; // Increment the sortID
        }
        //Stores the result of a sorting operation in two log files, without memory used included.
        static void StoreResult(int SortKey, int range, int[] set, float timeTaken)
        {
            string[] sortNames = { "Bubble Sort", "Merge Sort", "Counting Sort", "Quick Sort", "Insertion Sort", "LSD Radix Sort", "MSD Radix Sort", "Heap Sort" };

            // Log the result details to "AlgorithmLog.txt".
            using (StreamWriter writer = new StreamWriter("AlgorithmLog.txt", true))
            {
                writer.WriteLine(sortID + ")" + sortNames[SortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms");
                Console.WriteLine(sortID + ")" + sortNames[SortKey] + " Range 1-" + range + " Size " + set.Length + " Completed in " + timeTaken + "ms");

            }

            // Log the sorted array to "SortedAlgorithms.txt".
            using (StreamWriter writer = new StreamWriter("SortedAlgorithms.txt", true))
            {
                writer.Write(sortID + ")");
                for (int i = 0; i < set.Length; i++)
                {
                    writer.Write(set[i] + "|");
                }
                writer.WriteLine(); writer.WriteLine();
            }

            // Log the time taken to "times.txt".
            using (StreamWriter writer = new StreamWriter("times.txt", true))
            {
                writer.WriteLine(timeTaken);
            }
            sortID++; // Increment the sortID
        }
        static void storeResultAverage(int sortKey, int range, int arrayLength, int averageTime )
        {
            string[] sortNames = { "Bubble Sort", "Merge Sort", "Counting Sort", "Quick Sort", "Insertion Sort", "LSD Radix Sort", "MSD Radix Sort", "Heap Sort" };

            // Log the result details to "AlgorithmAveragesLog.txt".
            using (StreamWriter writer = new StreamWriter("AlgorithmLog.txt", true))
            {
                writer.WriteLine("1000 " + sortNames[sortKey] + "s with numbers in range 1-" + range + " size " + arrayLength + " completed in average " + averageTime + "ms");
            }
        }
        static void runFullTest(int noOfTests = 1000)
        {
            Stopwatch sw = new Stopwatch();
            for(int sortedState = 0; sortedState < 3; sortedState++)
            {
                for (int sortBeingRun = 0; sortBeingRun < 8; sortBeingRun++) //Loop to iterate through all 8 tests for a sort from a randomised set.
                {
                    for (int i = 0; i < 10; i++)//Loop to iterate through all 10 ranges
                    {
                        for (int j = 0; j < 9; j++)//Loop to iterate through all 9 array sizes
                        {
                            long totalTimeTaken = 0;
                            for (int testNo = 0; testNo < noOfTests; testNo++) //Loop to run the required test 1000 times
                            {
                                sw.Reset();
                                int range = (int)Math.Pow(10, i);
                                int arraySize = (int)Math.Pow(10, j);
                                int[] setToBeTested;
                                if (sortedState == 0)
                                {
                                    setToBeTested = makeSet(false, false, false, arraySize, range); //Random set
                                }
                                else if (sortedState == 1)
                                {
                                    setToBeTested = makeSet(true, true, false, arraySize, range); //Pre-sorted set
                                }
                                else
                                {
                                    setToBeTested = makeSet(true, false, true, arraySize, range); //Inverse Pre-Sorted set
                                }


                                switch (sortBeingRun)
                                {
                                    case 0:
                                        sw.Start();
                                        doBubbleSort(setToBeTested);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 1:
                                        sw.Start();
                                        doMergeSort(setToBeTested);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 2:
                                        sw.Start();
                                        doCountingSort(setToBeTested);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 3:
                                        sw.Start();
                                        doQuickSort(setToBeTested, 0, arraySize - 1);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 4:
                                        sw.Start();
                                        doInsertionSort(setToBeTested);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 5:
                                        sw.Start();
                                        doLSDRadixSort(setToBeTested);
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 6:
                                        sw.Start();
                                        //Insert MSD Radix Here
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                    case 7:
                                        sw.Start();
                                        //Insert Heap Sort here.
                                        sw.Stop();
                                        totalTimeTaken += sw.ElapsedMilliseconds;
                                        StoreResult(sortBeingRun, range, setToBeTested, sw.ElapsedMilliseconds);
                                        break;
                                }
                            }
                            int averageTime = (int)(totalTimeTaken / 1000);
                        }  
                        
                    }
                }
            }
            
        }
        static void Main(string[] args)
        {
            int[] set = makeSet(false, false, false, 5, 10000);
            printSet(set);
            Console.ReadKey();
            set = doLSDRadixSort(set);
            printSet(set);
            Console.ReadKey();
        }
    }
}

