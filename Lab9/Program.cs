using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab9
{
    class Program
    {
        // Алгоритм перевірки збалансованості дужок з АВТО-ВИПРАВЛЕННЯМ (Варіант 2)
        static bool CheckBalancedParentheses(string expression, out string errorDetails)
        {
            Stack<int> openingIndices = new Stack<int>();
            Stack<char> bracketsStack = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                // 1. Якщо відкриваюча дужка - кладемо в стек
                if (ch == '(' || ch == '[' || ch == '{')
                {
                    bracketsStack.Push(ch);
                    openingIndices.Push(i);
                }
                // 2. Якщо закриваюча дужка - перевіряємо вершину стеку
                else if (ch == ')' || ch == ']' || ch == '}')
                {
                    if (bracketsStack.Count == 0)
                    {
                        errorDetails = "❌ Зайва закриваюча дужка '" + ch + "' на позиції " + i;
                        return false;
                    }

                    char top = bracketsStack.Pop();
                    int openPos = openingIndices.Pop();

                    if (!IsMatchingPair(top, ch))
                    {
                        errorDetails = "❌ Невідповідність типів: відкрита '" + top + "' на позиції " + openPos + ", але закрита '" + ch + "' на позиції " + i;
                        return false;
                    }
                }
            }

            // 3. РЕАЛІЗАЦІЯ ВАРІАНТУ 2: Автовиправлення не закритих в кінці дужок
            if (bracketsStack.Count > 0)
            {
                string suggestedFix = "";
                
                // Дістаємо всі незакриті дужки і формуємо для них правильні закриваючі в зворотному порядку
                while (bracketsStack.Count > 0)
                {
                    char unclosed = bracketsStack.Pop();
                    suggestedFix += GetMatchingCloseBracket(unclosed);
                }

                errorDetails = "🔧 Не вистачає дужок у кінці! Автовиправлено. Додано: \"" + suggestedFix + "\" -> Результат: \"" + expression + suggestedFix + "\"";
                return true; // Повертаємо true, бо функція успішно виправила рядок
            }

            errorDetails = "✅ Дужки ідеально збалансовані!";
            return true;
        }

        static bool IsMatchingPair(char open, char close)
        {
            if (open == '(' && close == ')') return true;
            if (open == '[' && close == ']') return true;
            if (open == '{' && close == '}') return true;
            return false;
        }

        // Допоміжний метод для Варіанту 2: повертає парну закриваючу дужку
        static char GetMatchingCloseBracket(char open)
        {
            if (open == '(') return ')';
            if (open == '[') return ']';
            if (open == '{') return '}';
            return ' ';
        }

        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №9: ВАЛІДАТОР ДУЖОК З АВТОВИПРАВЛЕННЯМ");
            Console.WriteLine("==========================================================================");

            string[] testCases = {
                "{ int a = (b + c) * [d - 1]; }",         // Вірно
                "if (a[i] > 0) { list.Add((a[i]); }",     // Помилка: зайва чи невідповідна дужка всередині (не кінець рядка)
                "function test() { return [1, 2, (3 + 4]; }", // Помилка: невідповідність типів
                "())(",                                   // Помилка: зайва закриваюча
                "(((1 + 2) * 3)"                          // ВАРІАНТ 2: Не закрито 2 дужки в кінці (Буде автовиправлено!)
            };

            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < testCases.Length; i++)
            {
                string expr = testCases[i];
                Console.WriteLine("\nТест #" + (i + 1) + ": \"" + expr + "\"");
                
                string details;
                bool isValid = CheckBalancedParentheses(expr, out details);
                
                Console.WriteLine("Результат: " + details);
            }

            sw.Stop();
            Console.WriteLine("\n⏱ Загальний час аналізу: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " мс");
            Console.WriteLine("\n✅ Роботу з валідатором дужок завершено успішно!");
        }
    }
}


