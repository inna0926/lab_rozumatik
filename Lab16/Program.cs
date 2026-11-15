using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab16
{
    class Program
    {
        // 1. Наївний рекурсивний Фібоначчі - Експоненційний час O(2^n)
        static long NaiveFibonacci(int n, ref long callsCount)
        {
            callsCount++;
            if (n <= 1) return n; // Базовий випадок
            return NaiveFibonacci(n - 1, ref callsCount) + NaiveFibonacci(n - 2, ref callsCount);
        }

        // 2. Оптимізований Фібоначчі з мемоїзацією (кешуванням) - Лінійний час O(n)
        static long MemoizedFibonacci(int n, Dictionary<int, long> memo, ref long callsCount)
        {
            callsCount++;
            if (n <= 1) return n;
            if (memo.ContainsKey(n)) return memo[n]; // миттєвий результат з кешу

            long result = MemoizedFibonacci(n - 1, memo, ref callsCount) + MemoizedFibonacci(n - 2, memo, ref callsCount);
            memo[n] = result;
            return result;
        }

        // 3. Демонстрація глибини рекурсії та переповнення стеку
        static void DemonstrateRecursionDepth(int currentDepth, int targetDepth)
        {
            if (currentDepth >= targetDepth)
            {
                Console.WriteLine($"  🎯 Досягнуто безпечну максимальну глибину стеку: {currentDepth}");
                return;
            }
            DemonstrateRecursionDepth(currentDepth + 1, targetDepth);
        }

        // Варіант 2: Рекурсивний алгоритм Евкліда для знаходження НСД (GCD) - O(log(min(a, b)))
        static int RecursiveGCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (b == 0) return a; // Базовий випадок: НСД(a, 0) = a
            return RecursiveGCD(b, a % b);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №16 (ВАРІАНТ 2): РЕКУРСІЯ ТА МЕМОЇЗАЦІЯ");
            Console.WriteLine("=========================================================================");

            int n = 35;
            Console.WriteLine($"Обчислюємо {n}-е число Фібоначчі:\n");

            // Тест 1: Наївна рекурсія
            long calls1 = 0;
            Stopwatch sw = Stopwatch.StartNew();
            long fib1 = NaiveFibonacci(n, ref calls1);
            sw.Stop();
            Console.WriteLine("1. Наївна рекурсія:");
            Console.WriteLine($"  Результат: {fib1:N0} | Викликів функцій: {calls1:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Тест 2: Мемоїзація
            long calls2 = 0;
            var memo = new Dictionary<int, long>();
            sw.Restart();
            long fib2 = MemoizedFibonacci(n, memo, ref calls2);
            sw.Stop();
            Console.WriteLine("\n2. Мемоїзація (Кеш):");
            Console.WriteLine($"  Результат: {fib2:N0} | Викликів функцій: {calls2:N0} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // Тест 3: Перевірка глибини рекурсивних викликів
            Console.WriteLine("\n3. Перевірка глибини рекурсивних викликів:");
            DemonstrateRecursionDepth(1, 1000);

            // Тест 4 (Варіант 2): Найбільший спільний дільник за алгоритмом Евкліда
            Console.WriteLine("\n4. Варіант 2 — Обчислення НСД за рекурсивним алгоритмом Евкліда:");
            int num1 = 252, num2 = 105;
            int gcd = RecursiveGCD(num1, num2);
            Console.WriteLine($"  НСД({num1}, {num2}) = {gcd}");

            Console.WriteLine("\n✅ Висновок: Мемоїзація зменшила кількість викликів із мільйонів до лінійної кількості!");
        }
    }
}
