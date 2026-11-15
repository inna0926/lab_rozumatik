using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab22
{
    class Program
    {
        // 1. Легендарна хеш-функція DJB2 (Ден Бернштейн)
        static uint HashDJB2(string str)
        {
            uint hash = 5381;
            foreach (char c in str)
            {
                // hash * 33 + c
                hash = ((hash << 5) + hash) + (uint)c;
            }
            return hash;
        }

        // 2. Хеш-функція FNV-1a (32-бітний варіант)
        static uint HashFNV1a(string str)
        {
            uint hash = 2166136261;
            const uint FNV_PRIME = 16777619;

            foreach (char c in str)
            {
                hash ^= (uint)c;
                hash *= FNV_PRIME;
            }
            return hash;
        }

        static void AnalyzeHashUniformity(Func<string, uint> hashFunc, string name, List<string> words, int bucketCount)
        {
            int[] buckets = new int[bucketCount];
            HashSet<uint> uniqueHashes = new HashSet<uint>();
            int collisions = 0;

            foreach (string word in words)
            {
                uint h = hashFunc(word);
                if (!uniqueHashes.Add(h))
                {
                    collisions++;
                }

                int bucketIdx = (int)(h % (uint)bucketCount);
                buckets[bucketIdx]++;
            }

            int emptyBuckets = 0;
            int maxInBucket = 0;
            for (int i = 0; i < bucketCount; i++)
            {
                if (buckets[i] == 0) emptyBuckets++;
                if (buckets[i] > maxInBucket) maxInBucket = buckets[i];
            }

            Console.WriteLine($"\n📊 --- Аналіз алгоритму [{name}] для {words.Count:N0} слів ({bucketCount} бакетів) ---");
            Console.WriteLine($"   Унікальних хешів:  {uniqueHashes.Count:N0} із {words.Count:N0}");
            Console.WriteLine($"   Колізій значень:   {collisions} ({((double)collisions / words.Count * 100):F2}%)");
            Console.WriteLine($"   Порожніх бакетів:  {emptyBuckets} ({((double)emptyBuckets / bucketCount * 100):F2}%)");
            Console.WriteLine($"   Максимум в 1 бакеті: {maxInBucket}");
        }

        // Варіант 2: Поліноміальний Rolling Hash та пошук підрядків за алгоритмом Рабіна-Карпа - O(n + m)
        static List<int> RabinKarpSearch(string text, string pattern)
        {
            List<int> foundIndices = new List<int>();
            int n = text.Length;
            int m = pattern.Length;

            if (m == 0 || m > n) return foundIndices;

            const long BASE = 257;           // Проста основа полінома (більша за стандартний алфавіт)
            const long MOD = 1_000_000_007;  // Велике просте число для уникнення переповнення та мінімізації колізій

            long patternHash = 0;
            long windowHash = 0;
            long highestPow = 1; // BASE^(m - 1) % MOD

            // Обчислюємо старший степінь BASE^(m - 1) mod MOD
            for (int i = 0; i < m - 1; i++)
            {
                highestPow = (highestPow * BASE) % MOD;
            }

            // Початкове обчислення хешу шаблону та першого вікна тексту
            for (int i = 0; i < m; i++)
            {
                patternHash = (patternHash * BASE + pattern[i]) % MOD;
                windowHash = (windowHash * BASE + text[i]) % MOD;
            }

            // Ковзаємо вікном по тексту
            for (int i = 0; i <= n - m; i++)
            {
                // Якщо хеші збіглися — перевіряємо посимвольно (захист від хибних колізій)
                if (patternHash == windowHash)
                {
                    bool exactMatch = true;
                    for (int j = 0; j < m; j++)
                    {
                        if (text[i + j] != pattern[j])
                        {
                            exactMatch = false;
                            break;
                        }
                    }

                    if (exactMatch)
                        foundIndices.Add(i);
                }

                // Оновлюємо Rolling Hash за O(1) для наступного зсуву вікна
                if (i < n - m)
                {
                    windowHash = ((windowHash - text[i] * highestPow) * BASE + text[i + m]) % MOD;
                    if (windowHash < 0)
                        windowHash += MOD; // Корекція від'ємного залишку в C#
                }
            }

            return foundIndices;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("===================================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №22 (ВАРІАНТ 2): ДОСЛІДЖЕННЯ ХЕШ-ФУНКЦІЙ");
            Console.WriteLine("===================================================================================");

            // Генеруємо словник тестових слів (ідентифікатори користувачів)
            List<string> dictionary = new List<string>();
            for (int i = 1; i <= 5000; i++)
            {
                dictionary.Add($"student_cadet_{i:D5}");
                dictionary.Add($"sensor_temperature_node_{i}");
            }

            Console.WriteLine($"Згенеровано {dictionary.Count:N0} текстових ключів.");

            AnalyzeHashUniformity(HashDJB2, "DJB2 (hash * 33 + c)", dictionary, bucketCount: 1000);
            AnalyzeHashUniformity(HashFNV1a, "FNV-1a (XOR + Multiply)", dictionary, bucketCount: 1000);

            Console.WriteLine("\n✅ Висновок: Обидві хеш-функції демонструють чудовий рівномірний розподіл!");

            // --- Варіант 2: Пошук підрядка за алгоритмом Рабіна-Карпа (Polynomial Rolling Hash) ---
            Console.WriteLine("\n--- Варіант 2: Поліноміальний Rolling Hash (Алгоритм Рабіна-Карпа) ---");
            string sampleText = "Алгоритм Рабіна-Карпа використовує хеш-функцію для швидкого пошуку. Хеш оновлюється за O(1)!";
            string pattern = "хеш";

            Console.WriteLine($"Текст:   \"{sampleText}\"");
            Console.WriteLine($"Шаблон:  \"{pattern}\"");

            List<int> matches = RabinKarpSearch(sampleText.ToLower(), pattern.ToLower());
            Console.WriteLine($"Знайдено входжень: {matches.Count} (на позиціях: {string.Join(", ", matches)}) ✅");
        }
    }
}
