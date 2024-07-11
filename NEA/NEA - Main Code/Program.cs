using System; //test
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEA___Main_Code
{
    internal class Program
    {
        public struct item // struct used for algorithms such as quick sort that allwos you to treat values in array as individuals with an ID as opposed to just their value and so compare between items in an array with the same value
        {
            public int value, ID;
        }
        static int SortKey = 1;
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
        static int[] shuffle(int[] set)
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
        static item[] swapItems(item[] items, int IDx, int IDy)
        {
            int indexX = 0, indexY = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].ID == IDx) indexX = i;
                if (items[i].ID == IDy) indexY = i;
            }
            item temp = items[indexX];
            items[indexX] = items[indexY];z
            items[indexY] = temp;
            return items;
        }
        static void doBubbles(int iterations) //sets up a test with multiple variables, ordered, range etc to do 
        {
            int[] set1;
            for (int i = 0; i < iterations; i++)
            {
                set1 = makeSet(false, false, i * 10, 100);
                doBubbleSort(set1, 100);
            }
        }
        static void doCountings(int iterations)
        {
            int[] set1;
            for (int i = 0; i < iterations; i++)
            {
                set1 = makeSet(false, false, i * 10, 100);
                doCountingSort(set1, 100);
            }
        }
        static int[] intListToArray(List<int> set)
        {
            int[] array = new int[set.Count];
            for (int i = 0; i < set.Count; i++)
            {
                array[i] = set[i];
            }
            return array;
        }
        static item[] itemListToArray(List<item> set)
        {
            item[] array = new item[set.Count];
            for (int i = 0; i < set.Count; i++)
            {
                array[i] = set[i];
            }
            return array;
        }
        static void doCountingSort(int[] set, int range)
        {
            long memoryBeforeSort = GC.GetTotalMemory(true);
            Stopwatch sw = new Stopwatch(); sw.Start();

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
            sw.Stop();
            long memoryAfterSort = GC.GetTotalMemory(true);
            StoreResult("Counting Sort", range, intListToArray(sorted), sw.ElapsedMilliseconds, memoryAfterSort - memoryBeforeSort);
            sw.Reset();
        }
        static void doQuicks(int iterations)
        {
            int[] set1;
            item[] itemSet;
            for (int i = 0; i < iterations; i++)
            {
                set1 = makeSet(false, false, i * 10, 100);
                itemSet = new item[set1.Length];
                for (int j = 0; j < iterations; j++)
                {
                    itemSet[j].value = set1[j];
                }
                doQuickSort(itemSet, 100);
            }
        }
        static void doQuickSort(item[] set, int range)
        {
            long memoryBeforeSort = GC.GetTotalMemory(true);
            Stopwatch sw = new Stopwatch(); sw.Start();








            sw.Stop();
            long memoryAfterSort = GC.GetTotalMemory(true);
            StoreResult("Quick Sort", range, intListToArray(sorted), sw.ElapsedMilliseconds, memoryAfterSort - memoryBeforeSort);
            sw.Reset();
        }
        static item[] LeftPartition(item[] original, int centre)
        {
            item[] partitionedSet = new item[centre];
            for (int i = 0; i < centre - 1; i++)
            {
                partitionedSet[i] = original[i];
            }
            return partitionedSet;
        }
        static item[] RightPartition(item[] original, int centre)
        {
            item[] partitionedSet = new item[original.Length - centre];
            for (int i = 0; i < original.Length - centre - 1; i++)
            {
                partitionedSet[i] = original[i + centre];
            }
            return partitionedSet;
        }
        static item[] QuickSortIteration(item[] set, int pivot)
        {
            if (set.Length == 1) return set;
            List<item> newSet = new List<item>();
            for (int i = 0; i < set.Length; i++)
            {
                if (set[i].value < pivot) newSet.Add(set[i]);
            }
            for (int i = 0; i < set.Length; i++)
            {
                if (set[i].value == pivot) newSet.Add(set[i]);
            }
            for (int i = 0; i < set.Length; i++)
            {
                if (set[i].value > pivot) newSet.Add(set[i]);
            }
            return itemListToArray(newSet);
        }
        static void doBubbleSort(int[] set, int range) // carries out the sorting operation
        {
            long memoryBeforeSort = GC.GetTotalMemory(true); // This stores the total memory used so far in the program so that later we can subtract the current value from this to see how much has been used in a sort
            Stopwatch sw = new Stopwatch(); sw.Start();
            while (BubblePass(ref set)) ;
            long memoryAfterSort = GC.GetTotalMemory(true);
            StoreResult("Bubble Sort", range, set, sw.ElapsedMilliseconds, memoryAfterSort - memoryBeforeSort);
            sw.Reset();
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
            doCountings(200);
            doBubbles(200);
            Console.ReadKey();
        }
    }
}

