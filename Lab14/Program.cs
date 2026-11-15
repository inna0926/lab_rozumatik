using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab14
{
    class Program
    {
        // Варіант 2: Оптимізований Bubble Sort із запам'ятовуванням індексу останнього обміну
        static void OptimizedBubbleSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;
            int rightBoundary = n - 1; // Межа невідсортованої частини масиву

            while (rightBoundary > 0)
            {
                int lastSwapIndex = 0; // Індекс останньої перестановки на поточному проході

                for (int j = 0; j < rightBoundary; j++)
                {
                    comparisons++;
                    if (arr[j] > arr[j + 1])
                    {
                        // Обмін сусідніх елементів
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;

                        swaps++;
                        lastSwapIndex = j; // Запам'ятовуємо позицію останнього обміну
                    }
                }

                // Усі елементи після lastSwapIndex вже на своїх місцях — відсікаємо їх одразу
                // Якщо обмінів не було (lastSwapIndex == 0), цикл while завершиться автоматично
                rightBoundary = lastSwapIndex;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("===================================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №14 (ВАРІАНТ 2): BUBBLE SORT (LAST SWAP INDEX)");
            Console.WriteLine("===================================================================================");

            int n = 1000;
            int[] randomData = new int[n];
            int[] sortedData = new int[n];
            int[] reversedData = new int[n];

            Random rnd = new Random(42);
            for (int i = 0; i < n; i++)
            {
                randomData[i] = rnd.Next(1, 10000);
                sortedData[i] = i;
                reversedData[i] = n - i;
            }

            // 1. Тест на випадкових даних
            int[] test1 = (int[])randomData.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            OptimizedBubbleSort(test1, out long comp1, out long swap1);
            sw.Stop();
            Console.WriteLine($"📊 1. Випадковий масив (N={n}):");
            Console.WriteLine($"   Порівнянь: {comp1:N0} | Перестановок: {swap1:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // 2. Тест на вже відсортованому масиві (Найкращий випадок O(n))
            int[] test2 = (int[])sortedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test2, out long comp2, out long swap2);
            sw.Stop();
            Console.WriteLine($"\n📊 2. Вже відсортований масив (Найкращий випадок O(n)):");
            Console.WriteLine($"   Порівнянь: {comp2:N0} | Перестановок: {swap2:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // 3. Тест на зворотному масиві (Найгірший випадок O(n^2))
            int[] test3 = (int[])reversedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test3, out long comp3, out long swap3);
            sw.Stop();
            Console.WriteLine($"\n📊 3. Зворотно впорядкований масив (Найгірший випадок O(n^2)):");
            Console.WriteLine($"   Порівнянь: {comp3:N0} | Перестановок: {swap3:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            Console.WriteLine("\n✅ Висновок: запам'ятовування індексу останнього обміну зменшує кількість порівнянь на частково відсортованих даних!");
        }
    }
}
