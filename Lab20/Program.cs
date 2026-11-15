using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab20
{
    class Program
    {
        // 1. Лінійний пошук O(n)
        static int LinearSearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                iterations++;
                if (arr[i] == target) return i;
            }
            return -1;
        }

        // 2. Бінарний пошук O(log n)
        static int BinarySearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            int low = 0;
            int high = arr.Length - 1;

            while (low <= high)
            {
                iterations++;
                int mid = low + (high - low) / 2; // запобігання переповненню int

                if (arr[mid] == target) return mid;
                if (arr[mid] < target)
                    low = mid + 1;
                else
                    high = mid - 1;
            }

            return -1;
        }

        // Варіант 2: Бінарний пошук у циклічно зсунутому відсортованому масиві O(log n)
        static int SearchRotatedArray(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            int low = 0;
            int high = arr.Length - 1;

            while (low <= high)
            {
                iterations++;
                int mid = low + (high - low) / 2;

                if (arr[mid] == target) return mid;

                // Визначаємо, яка половина масиву залишається строго відсортованою
                if (arr[low] <= arr[mid])
                {
                    // Ліва половина [low..mid] відсортована
                    if (target >= arr[low] && target < arr[mid])
                        high = mid - 1; // Шукане число в лівій частині
                    else
                        low = mid + 1;  // Шукане число в правій частині
                }
                else
                {
                    // Права половина [mid..high] відсортована
                    if (target > arr[mid] && target <= arr[high])
                        low = mid + 1;  // Шукане число в правій частині
                    else
                        high = mid - 1; // Шукане число в лівій частині
                }
            }

            return -1;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("===================================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №20 (ВАРІАНТ 2): LINEAR VS BINARY SEARCH");
            Console.WriteLine("===================================================================================");

            int n = 100_000;
            int[] data = new int[n];
            for (int i = 0; i < n; i++) data[i] = i * 2; // [0, 2, 4, 6 ... 199998]

            int target = 195_432; // шукане число, розташоване біля самого кінця
            Console.WriteLine($"Розмірність масиву: N = {n:N0} елементів");
            Console.WriteLine($"Шукане значення:    {target:N0}\n");

            // Лінійний пошук
            Stopwatch sw = Stopwatch.StartNew();
            int idx1 = LinearSearch(data, target, out int iter1);
            sw.Stop();
            Console.WriteLine("🔍 [Лінійний пошук O(n)]");
            Console.WriteLine($"   Знайдено індекс: {idx1} | Ітерацій: {iter1:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Бінарний пошук
            sw.Restart();
            int idx2 = BinarySearch(data, target, out int iter2);
            sw.Stop();
            Console.WriteLine("\n⚡ [Бінарний пошук O(log n)]");
            Console.WriteLine($"   Знайдено індекс: {idx2} | Ітерацій: {iter2:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            Console.WriteLine($"\n🚀 Прискорення за кількістю кроків: у {iter1 / Math.Max(1, iter2):N0} разів!");

            // --- Варіант 2: Бінарний пошук у циклічно зсунутому масиві ---
            Console.WriteLine("\n--- Варіант 2: Бінарний пошук у циклічно зсунутому масиві (Rotated Sorted Array) ---");
            int[] rotatedArray = { 45, 67, 89, 102, 5, 12, 19, 24, 33 };
            int rotatedTarget = 19;

            Console.WriteLine("Циклічно зсунутий масив: [" + string.Join(", ", rotatedArray) + "]");
            int rotIdx = SearchRotatedArray(rotatedArray, rotatedTarget, out int rotIter);
            Console.WriteLine($"Шукане число: {rotatedTarget} -> Знайдено на індексі: {rotIdx} за {rotIter} ітерацій ✅");
        }
    }
}
