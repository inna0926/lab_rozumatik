using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab32
{
    public class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; } // Вага в кг
        public int Value { get; set; }  // Цінність у грн

        public Item(string name, int weight, int value)
        {
            Name = name;
            Weight = weight;
            Value = value;
        }
    }

    class Program
    {
        static void Solve01Knapsack(Item[] items, int maxCapacity)
        {
            int n = items.Length;
            int[,] dp = new int[n + 1, maxCapacity + 1];

            // 1. Заповнення таблиці ДП
            for (int i = 1; i <= n; i++)
            {
                int currentWeight = items[i - 1].Weight;
                int currentValue = items[i - 1].Value;

                for (int w = 1; w <= maxCapacity; w++)
                {
                    if (currentWeight <= w)
                    {
                        // Вибір: не брати предмет АБО взяти його
                        dp[i, w] = Math.Max(
                            dp[i - 1, w],
                            dp[i - 1, w - currentWeight] + currentValue
                        );
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w]; // предмет не поміщається
                    }
                }
            }

            int maxValue = dp[n, maxCapacity];
            Console.WriteLine($"💰 Максимальна сумарна цінність вантажу: {maxValue:N0} грн");

            // 2. Відновлення списку обраних предметів
            List<Item> packedItems = new List<Item>();
            int remWeight = maxCapacity;

            for (int i = n; i > 0 && maxValue > 0; i--)
            {
                // Якщо значення відрізняється від верхнього рядка - предмет було взято!
                if (dp[i, remWeight] != dp[i - 1, remWeight])
                {
                    Item taken = items[i - 1];
                    packedItems.Add(taken);
                    maxValue -= taken.Value;
                    remWeight -= taken.Weight;
                }
            }

            Console.WriteLine($"\n🎒 Список предметів у рюкзаку (Вантажопідйомність {maxCapacity} кг):");
            int totalWeight = 0;
            foreach (var item in packedItems)
            {
                Console.WriteLine($"   • {item.Name,-20} | Вага: {item.Weight,2} кг | Цінність: {item.Value} грн");
                totalWeight += item.Weight;
            }
            Console.WriteLine($"\nЗагальна зайнята вага: {totalWeight} / {maxCapacity} кг.");
        }

        // ВАРІАНТ 2: Задача про розмін монет (Coin Change Problem) мінімальною кількістю монет
        static void SolveCoinChange(int[] coins, int amount)
        {
            int max = amount + 1;
            int[] dp = new int[amount + 1];
            int[] lastCoin = new int[amount + 1]; // для відновлення набору монет

            for (int i = 1; i <= amount; i++)
            {
                dp[i] = max;
                lastCoin[i] = -1;
            }
            dp[0] = 0;

            // Заповнення масиву ДП для кожної суми від 1 до amount
            for (int a = 1; a <= amount; a++)
            {
                foreach (int coin in coins)
                {
                    if (coin <= a && dp[a - coin] + 1 < dp[a])
                    {
                        dp[a] = dp[a - coin] + 1;
                        lastCoin[a] = coin;
                    }
                }
            }

            Console.WriteLine($"Доступні номінали монет: [ {string.Join(", ", coins)} ]");
            Console.WriteLine($"Цільова сума для розміну: {amount} грн");

            if (dp[amount] > amount)
            {
                Console.WriteLine("❌ Неможливо розміняти вказану суму наявними монетами!");
                return;
            }

            Console.WriteLine($"🪙 Мінімальна кількість монет: {dp[amount]} шт.");

            // Відновлення використаних монет
            List<int> usedCoins = new List<int>();
            int curr = amount;
            while (curr > 0)
            {
                usedCoins.Add(lastCoin[curr]);
                curr -= lastCoin[curr];
            }
            Console.WriteLine($"   Обрані монети: {string.Join(" + ", usedCoins)} = {amount} грн");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №32: ЗАДАЧА ПРО РЮКЗАК (ДИНАМІЧНЕ ПРОГРАМУВАННЯ)");
            Console.WriteLine("=================================================================");

            // Набір цінних речей для експедиції на Марс (Дюну):
            Item[] items = {
                new Item("Генератор кисню", 4, 3000),
                new Item("Супутниковий зв'язок", 2, 2000),
                new Item("Водний дистилятор", 3, 2500),
                new Item("Запас провізії", 5, 1800),
                new Item("Медичний набір", 1, 1500),
                new Item("Сонячна панель", 3, 2200)
            };

            int dronePayloadLimit = 8; // макс вага 8 кг
            Solve01Knapsack(items, dronePayloadLimit);

            // ВАРІАНТ 2: Демонстрація задачі про розмін монет
            Console.WriteLine("\n=================================================================");
            Console.WriteLine("   ВАРІАНТ 2: ЗАДАЧА ПРО РОЗМІН МОНЕТ (COIN CHANGE PROBLEM)");
            Console.WriteLine("=================================================================");

            int[] coins = { 1, 3, 4, 5 };
            int targetAmount = 7;
            SolveCoinChange(coins, targetAmount);

            Console.WriteLine("\n🎉 ВІТАЄМО! ВИ УСПІШНО ОПАНУВАЛИ ВСІ 32 ТЕМИ АЛГОРИТМІВ ТА СТРУКТУР ДАНИХ!");
        }
    }
}
