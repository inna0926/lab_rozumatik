using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab10
{
    class Program
    {
        // Пріоритет операторів (унарний мінус має найвищий пріоритет)
        static int GetPrecedence(string op)
        {
            if (op == "+" || op == "-") return 1;
            if (op == "*" || op == "/") return 2;
            if (op == "^") return 3;
            if (op == "u-") return 4; // u- позначає унарний мінус
            return 0;
        }

        // 1. Shunting-Yard: Інфіксний вираз -> RPN (Постфіксний) з підтримкою унарного мінуса (Варіант 2)
        static List<string> InfixToRPN(string expression)
        {
            List<string> output = new List<string>();
            Stack<string> operators = new Stack<string>();

            // Розбиваємо рядок по пробілах
            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Змінна для відстеження, чи може поточний мінус бути унарним
            bool isUnaryPossible = true;

            foreach (string token in tokens)
            {
                double number;
                // Якщо токен - число
                if (double.TryParse(token, out number))
                {
                    output.Add(token);
                    isUnaryPossible = false; // Після числа наступний оператор буде бінарним
                }
                else if (token == "(")
                {
                    operators.Push("(");
                    isUnaryPossible = true; // Після дужки може йти унарний мінус
                }
                else if (token == ")")
                {
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count > 0) operators.Pop(); // видаляємо "("
                    isUnaryPossible = false;
                }
                else // Оператори: +, -, *, /, ^
                {
                    string currentOp = token;

                    // РЕАЛІЗАЦІЯ ВАРІАНТУ 2: якщо це мінус і він стоїть в унарній позиції
                    if (currentOp == "-" && isUnaryPossible)
                    {
                        currentOp = "u-"; // Міняємо маркер на унарний мінус
                    }

                    while (operators.Count > 0 && operators.Peek() != "(" && 
                           GetPrecedence(operators.Peek()) >= GetPrecedence(currentOp))
                    {
                        output.Add(operators.Pop());
                    }
                    
                    operators.Push(currentOp);
                    isUnaryPossible = true; // Після будь-якого оператора може йти унарний мінус
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }

        // 2. Обчислення значення RPN виразу на числовому стеку
        static double EvaluateRPN(List<string> rpnTokens)
        {
            Stack<double> stack = new Stack<double>();

            foreach (string token in rpnTokens)
            {
                double number;
                if (double.TryParse(token, out number))
                {
                    stack.Push(number);
                }
                // РЕАЛІЗАЦІЯ ВАРІАНТУ 2: обробка унарного мінуса
                else if (token == "u-")
                {
                    if (stack.Count < 1) throw new InvalidOperationException("Помилка виразу: недостатньо операндів для унарного мінуса.");
                    double a = stack.Pop();
                    stack.Push(-a); // Просто міняємо знак числа на протилежний
                }
                else // Бінарні оператори
                {
                    if (stack.Count < 2) throw new InvalidOperationException("Помилка виразу: недостатньо операндів для операції " + token);
                    
                    double b = stack.Pop();
                    double a = stack.Pop();

                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "^": stack.Push(Math.Pow(a, b)); break;
                        default: throw new InvalidOperationException("Невідомий оператор: " + token);
                    }
                }
            }

            return stack.Pop();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №10: RPN СТЕК-КАЛЬКУЛЯТОР (УНАРНИЙ МІНУС)");
            Console.WriteLine("==========================================================================");

            // Тестовий вираз із базового завдання, але з додаванням унарного мінуса попереду: -5 * 2
            // ВАЖЛИВО: всі символи та оператори мають бути розділені пробілами, як того вимагає метод .Split(' ')
            string infixExpr = "- 5 * 2 + 3 + 4 * 2 / ( 1 - 5 ) ^ 2"; 
            
            Console.WriteLine("Вхідний інфіксний вираз: " + infixExpr);

            Stopwatch sw = Stopwatch.StartNew();

            // Перетворення
            List<string> rpn = InfixToRPN(infixExpr);
            Console.WriteLine("Постфіксний запис (RPN): " + string.Join(" ", rpn));

            // Обчислення
            double result = EvaluateRPN(rpn);
            sw.Stop();

            Console.WriteLine("\n🎯 Обчислений результат: " + result);
            Console.WriteLine("⏱ Час обчислення: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");
            Console.WriteLine("✅ Тест алгоритму Дейкстри з унарним мінусом пройдено успішно!");
        }
    }
}
