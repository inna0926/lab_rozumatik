using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab17
{
    class Program
    {
        // Варіант 2: Вибір медіани з трьох (перший, середній, останній) та переміщення її в кінець як pivot
        static void SetMedianOfThreePivot(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            int mid = low + (high - low) / 2;

            // Впорядковуємо три елементи: arr[low], arr[mid], arr[high]
            comparisons++;
            if (arr[low] > arr[mid])
                Swap(arr, low, mid, ref swaps);

            comparisons++;
            if (arr[low] > arr[high])
                Swap(arr, low, high, ref swaps);

            comparisons++;
            if (arr[mid] > arr[high])
                Swap(arr, mid, high, ref swaps);

            // Тепер arr[mid] є медіаною. Ставимо її на позицію high, щоб схема Ломуто використала її як pivot
            Swap(arr, mid, high, ref swaps);
        }

        // 1. Схема розбиття Ломуто з медіаною з трьох
        static int LomutoPartition(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            // Обираємо оптимальний опорний елемент перед розбиттям
            SetMedianOfThreePivot(arr, low, high, ref comparisons, ref swaps);

            int pivot = arr[high];
            int i = low - 1; // індекс менших елементів

            for (int j = low; j < high; j++)
            {
                comparisons++;
                if (arr[j] <= pivot)
                {
                    i++;
                    Swap(arr, i, j, ref swaps);
                }
            }

            // Ставимо pivot на його правильне фінальне місце (i + 1)
            Swap(arr, i + 1, high, ref swaps);
            return i + 1;
        }

        // 2. Рекурсивне сортування QuickSort
        static void QuickSort(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            if (low < high)
            {
                // pi - індекс опорного елемента після розбиття
                int pi = LomutoPartition(arr, low, high, ref comparisons, ref swaps);

                // Рекурсивно сортуємо ліву та праву частини
                QuickSort(arr, low, pi - 1, ref comparisons, ref swaps);
                QuickSort(arr, pi + 1, high, ref comparisons, ref swaps);
            }
        }

        static void Swap(int[] arr, int a, int b, ref long swaps)
        {
            if (a == b) return;
            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
            swaps++;
        }

        static bool IsArraySorted(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] > arr[i + 1]) return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("===================================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №17 (ВАРІАНТ 2): QUICKSORT (MEDIAN OF THREE)");
            Console.WriteLine("===================================================================================");

            int n = 20_000;
            int[] randomData = new int[n];
            Random rand = new Random(42);
            for (int i = 0; i < n; i++) randomData[i] = rand.Next(1, 100_000);

            // --- Тест 1: Випадковий масив ---
            Console.WriteLine($"📊 1. Сортування випадкового масиву з N = {n:N0} чисел:");
            long comp1 = 0, swaps1 = 0;
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(randomData, 0, randomData.Length - 1, ref comp1, ref swaps1);
            sw.Stop();

            Console.WriteLine($"  ⏱ Час сортування:     {sw.Elapsed.TotalMilliseconds:F3} ms");
            Console.WriteLine($"  🔍 Кількість порівнянь: {comp1:N0}");
            Console.WriteLine($"  🔄 Кількість обмінів:   {swaps1:N0}");
            Console.WriteLine($"  ✅ Масив відсортовано коректно: {IsArraySorted(randomData)}");

            // --- Тест 2: Вже відсортований масив (демонстрація захисту від O(n^2)) ---
            int[] sortedData = new int[n];
            for (int i = 0; i < n; i++) sortedData[i] = i;

            Console.WriteLine($"\n📊 2. Сортування вже відсортованого масиву з N = {n:N0} чисел:");
            long comp2 = 0, swaps2 = 0;
            sw.Restart();
            QuickSort(sortedData, 0, sortedData.Length - 1, ref comp2, ref swaps2);
            sw.Stop();

            Console.WriteLine($"  ⏱ Час сортування:     {sw.Elapsed.TotalMilliseconds:F3} ms");
            Console.WriteLine($"  🔍 Кількість порівнянь: {comp2:N0}");
            Console.WriteLine($"  🔄 Кількість обмінів:   {swaps2:N0}");
            Console.WriteLine($"  ✅ Масив відсортовано коректно: {IsArraySorted(sortedData)}");
        }
    }
}
