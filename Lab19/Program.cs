using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab19
{
    class Program
    {
        // 1. Counting Sort для невід'ємних чисел у відомому діапазоні [0..maxVal] - O(n + k)
        static void CountingSort(int[] arr, int maxVal)
        {
            int[] counts = new int[maxVal + 1];

            // Підрахунок кількості кожного значення
            for (int i = 0; i < arr.Length; i++) counts[arr[i]]++;

            // Відновлення впорядкованого масиву
            int idx = 0;
            for (int val = 0; val <= maxVal; val++)
            {
                while (counts[val] > 0)
                {
                    arr[idx++] = val;
                    counts[val]--;
                }
            }
        }

        // 2. Допоміжна функція сортування за конкретним розрядом exp (1, 10, 100...)
        static void CountSortByDigit(int[] arr, int exp)
        {
            int n = arr.Length;
            int[] output = new int[n];
            int[] count = new int[10];

            for (int i = 0; i < n; i++)
            {
                int digit = (arr[i] / exp) % 10;
                count[digit]++;
            }

            for (int i = 1; i < 10; i++) count[i] += count[i - 1];

            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (arr[i] / exp) % 10;
                output[count[digit] - 1] = arr[i];
                count[digit]--;
            }

            for (int i = 0; i < n; i++) arr[i] = output[i];
        }

        // 3. Порозрядне сортування Radix Sort - O(d * (n + b))
        static void RadixSort(int[] arr)
        {
            int max = arr[0];
            for (int i = 1; i < arr.Length; i++) if (arr[i] > max) max = arr[i];

            // Проходимо по всіх розрядах: одиниці, десятки, сотні...
            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountSortByDigit(arr, exp);
            }
        }

        // Варіант 2: Допоміжний стабільний підрахунок за конкретною позицією символу (charIndex)
        static void CountSortByChar(string[] arr, int charIndex)
        {
            int n = arr.Length;
            string[] output = new string[n];
            int[] count = new int[65536]; // Підтримка всіх символів UTF-16 (включно з кирилицею та латиницею)

            for (int i = 0; i < n; i++)
            {
                char ch = char.ToLowerInvariant(arr[i][charIndex]);
                count[ch]++;
            }

            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            // Йдемо з кінця масиву для збереження стабільності сортування
            for (int i = n - 1; i >= 0; i--)
            {
                char ch = char.ToLowerInvariant(arr[i][charIndex]);
                output[count[ch] - 1] = arr[i];
                count[ch]--;
            }

            for (int i = 0; i < n; i++)
            {
                arr[i] = output[i];
            }
        }

        // Варіант 2: Рядковий LSD Radix Sort для слів однакової довжини
        static void StringRadixSort(string[] words)
        {
            if (words.Length == 0) return;

            int wordLength = words[0].Length;
            foreach (string w in words)
            {
                if (w.Length != wordLength)
                    throw new ArgumentException("Усі слова в масиві повинні мати однакову довжину!");
            }

            // Сортуємо від останнього символу (молодшого розряду) до першого (старшого)
            for (int pos = wordLength - 1; pos >= 0; pos--)
            {
                CountSortByChar(words, pos);
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("============================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №19 (ВАРІАНТ 2): COUNTING ТА RADIX SORT");
            Console.WriteLine("============================================================================");

            // --- Частина 1: Тест числового Radix Sort ---
            int n = 50_000;
            int[] testData = new int[n];
            Random rnd = new Random(42);
            for (int i = 0; i < n; i++) testData[i] = rnd.Next(1, 100_000);

            int[] arrRadix = (int[])testData.Clone();

            Stopwatch sw = Stopwatch.StartNew();
            RadixSort(arrRadix);
            sw.Stop();

            Console.WriteLine($"📊 Radix Sort для N={n:N0} випадкових 5-значних чисел:");
            Console.WriteLine($"   Час виконання: {sw.Elapsed.TotalMilliseconds:F3} ms ⚡ (Лінійна складність!)");

            bool ok = true;
            for (int i = 0; i < n - 1; i++) if (arrRadix[i] > arrRadix[i + 1]) ok = false;
            Console.WriteLine($"   Результат: Масив відсортовано без помилок: {ok}");

            // --- Частина 2 (Варіант 2): Тест рядкового Radix Sort для слів однакової довжини ---
            Console.WriteLine("\n--- Варіант 2: Рядковий Radix Sort для слів однакової довжини (4 літери) ---");
            string[] words = { "кіт!", "літо", "море", "зіма", "вода", "небо", "поле", "гора" };

            Console.WriteLine("До сортування:    " + string.Join(", ", words));
            StringRadixSort(words);
            Console.WriteLine("Після сортування: " + string.Join(", ", words));
        }
    }
}
