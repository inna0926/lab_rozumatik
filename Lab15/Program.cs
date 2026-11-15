using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab15
{
    // Допоміжний клас для дослідження стабільності сортування (Варіант 2)
    public class RecordItem
    {
        public int Key { get; set; }      // Ключ, за яким відбувається сортування
        public string Tag { get; set; }   // Мітка початкової позиції для перевірки порядку

        public RecordItem(int key, string tag)
        {
            Key = key;
            Tag = tag;
        }

        public override string ToString() => $"[{Key}: {Tag}]";
    }

    class Program
    {
        // 1. Selection Sort: завжди фіксовано O(n^2) порівнянь, але лише O(n) обмінів
        static void SelectionSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0; swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (arr[j] < arr[minIdx])
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    int temp = arr[i];
                    arr[i] = arr[minIdx];
                    arr[minIdx] = temp;
                    swaps++;
                }
            }
        }

        // 2. Insertion Sort: O(n^2) в гіршому, але O(n) в найкращому випадку
        static void InsertionSort(int[] arr, out long comparisons, out long shifts)
        {
            comparisons = 0; shifts = 0;
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0)
                {
                    comparisons++;
                    if (arr[j] > key)
                    {
                        arr[j + 1] = arr[j]; // зсув праворуч
                        shifts++;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                arr[j + 1] = key;
            }
        }

        // Варіант 2: Selection Sort для масиву об'єктів (Нестабільний)
        static void SelectionSortObjects(RecordItem[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j].Key < arr[minIdx].Key)
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    RecordItem temp = arr[i];
                    arr[i] = arr[minIdx];
                    arr[minIdx] = temp;
                }
            }
        }

        // Варіант 2: Insertion Sort для масиву об'єктів (Стабільний завдяки строгому >)
        static void InsertionSortObjects(RecordItem[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                RecordItem current = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j].Key > current.Key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = current;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №15 (ВАРІАНТ 2): SELECTION VS INSERTION");
            Console.WriteLine("=========================================================================");

            int n = 2000;
            Random rnd = new Random(42);
            int[] original = new int[n];
            for (int i = 0; i < n; i++) original[i] = rnd.Next(1, 50000);

            // Майже відсортований масив (лише 2% елементів не на своїх місцях)
            int[] nearlySorted = new int[n];
            for (int i = 0; i < n; i++) nearlySorted[i] = i;
            for (int i = 0; i < n / 50; i++) nearlySorted[rnd.Next(n)] = rnd.Next(n);

            Console.WriteLine($"--- ТЕСТ 1: Випадковий масив (N={n}) ---");
            int[] arr1 = (int[])original.Clone();
            SelectionSort(arr1, out long c1, out long s1);
            Console.WriteLine($"Selection Sort: {c1:N0} порівнянь, {s1:N0} обмінів");

            int[] arr2 = (int[])original.Clone();
            InsertionSort(arr2, out long c2, out long s2);
            Console.WriteLine($"Insertion Sort: {c2:N0} порівнянь, {s2:N0} зсувів");

            Console.WriteLine($"\n--- ТЕСТ 2: Майже відсортований масив (N={n}) ---");
            int[] arr3 = (int[])nearlySorted.Clone();
            SelectionSort(arr3, out long c3, out long s3);
            Console.WriteLine($"Selection Sort: {c3:N0} порівнянь, {s3:N0} обмінів");

            int[] arr4 = (int[])nearlySorted.Clone();
            InsertionSort(arr4, out long c4, out long s4);
            Console.WriteLine($"Insertion Sort: {c4:N0} порівнянь, {s4:N0} зсувів ⚡ (в рази швидше!)");

            // --- ТЕСТ 3 (Варіант 2): Дослідження стабільності на об'єктах ---
            Console.WriteLine("\n--- ТЕСТ 3 (Варіант 2): Дослідження стабільності сортувань ---");
            RecordItem[] sampleObjects = {
                new RecordItem(5, "Перший (5)"),
                new RecordItem(5, "Другий (5)"),
                new RecordItem(2, "Третій (2)")
            };

            Console.WriteLine("Початковий масив: " + string.Join(", ", (object[])sampleObjects));

            RecordItem[] selObj = (RecordItem[])sampleObjects.Clone();
            SelectionSortObjects(selObj);
            Console.WriteLine("Selection Sort:   " + string.Join(", ", (object[])selObj) + " ❌ (Нестабільний: 'Перший (5)' і 'Другий (5)' помінялися місцями)");

            RecordItem[] insObj = (RecordItem[])sampleObjects.Clone();
            InsertionSortObjects(insObj);
            Console.WriteLine("Insertion Sort:   " + string.Join(", ", (object[])insObj) + " ✅ (Стабільний: початковий порядок однакових ключів збережено)");

            Console.WriteLine("\n✅ Висновок: Insertion Sort перемагає на майже відсортованих даних і зберігає стабільність!");
        }
    }
}
