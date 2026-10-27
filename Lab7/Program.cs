using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab7
{
    // Опис процесу для диспетчера ОС
    public class ProcessItem
    {
        public string Name { get; set; }
        public int RemainingTimeMs { get; set; }
        public bool IsHighPriority { get; set; } // ДОДАНО ДЛЯ ВАРІАНТУ 2

        // Конструктор з опціональним параметром пріоритету (за замовчуванням - false)
        public ProcessItem(string name, int remainingTimeMs, bool isHighPriority = false)
        {
            Name = name;
            RemainingTimeMs = remainingTimeMs;
            IsHighPriority = isHighPriority;
        }
    }

    public class CircularNode
    {
        public ProcessItem Process { get; set; }
        public CircularNode Next { get; set; }

        public CircularNode(ProcessItem process)
        {
            Process = process;
            Next = this; // за замовчуванням посилається сам на себе
        }
    }

    public class RoundRobinScheduler
    {
        private CircularNode _tail = null;
        private int _count = 0;

        public int Count
        {
            get { return _count; }
        }

        // Додавання процесу в кільце
        public void AddProcess(ProcessItem proc)
        {
            CircularNode newNode = new CircularNode(proc);
            if (_tail == null)
            {
                _tail = newNode;
                _tail.Next = _tail;
            }
            else
            {
                newNode.Next = _tail.Next;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        // Симуляція планувальника Round-Robin
        public void Run(int timeQuantumMs)
        {
            if (_tail == null) return;

            Console.WriteLine("\n🚀 Запуск диспетчера процесора (Базовий квант часу = " + timeQuantumMs + " мс)");
            CircularNode prev = _tail;
            CircularNode curr = _tail.Next;

            int round = 1;
            while (_count > 0)
            {
                Console.WriteLine("--- Раунд #" + round++ + " ---");

                // РЕАЛІЗАЦІЯ ВАРІАНТУ 2: Процеси з вищим пріоритетом отримують подвійний квант часу
                int currentQuantum = curr.Process.IsHighPriority ? timeQuantumMs * 2 : timeQuantumMs;

                Console.Write("⚙️ Процес [" + curr.Process.Name + "] (залишок: " + curr.Process.RemainingTimeMs + " мс, " + 
                              (curr.Process.IsHighPriority ? "ВИСОКИЙ пріоритет, квант: " + currentQuantum : "нормальний пріоритет") + ") -> ");

                if (curr.Process.RemainingTimeMs <= currentQuantum)
                {
                    Console.WriteLine("ЗАВЕРШЕНО за " + curr.Process.RemainingTimeMs + " мс!");
                    // Видаляємо завершений вузол з кільця
                    if (_count == 1)
                    {
                        _tail = null;
                        _count = 0;
                        break;
                    }
                    else
                    {
                        prev.Next = curr.Next;
                        if (curr == _tail) _tail = prev;
                        curr = curr.Next;
                        _count--;
                    }
                }
                else
                {
                    curr.Process.RemainingTimeMs -= currentQuantum;
                    Console.WriteLine("Квант вичерпано, новий залишок: " + curr.Process.RemainingTimeMs + " мс");
                    prev = curr;
                    curr = curr.Next;
                }
            }

            Console.WriteLine("\n✨ Усі процеси в черзі успішно виконано!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №7: ROUND-ROBIN SCHEDULER");
            Console.WriteLine("==========================================================================");

            RoundRobinScheduler scheduler = new RoundRobinScheduler();

            // Передаємо третім параметром true, якщо процес має високий пріоритет (Варіант 2)
            scheduler.AddProcess(new ProcessItem("IDE Rider", 25, true));       // Високий пріоритет (отримає квант 30 мс замість 15)
            scheduler.AddProcess(new ProcessItem("Browser Chrome", 40));       // Нормальний пріоритет (квант 15 мс)
            scheduler.AddProcess(new ProcessItem("Telegram Bot", 15));         // Нормальний пріоритет (квант 15 мс)
            scheduler.AddProcess(new ProcessItem("Audio Player", 10, true));   // Високий пріоритет (отримає квант 30 мс)

            scheduler.Run(15);
        }
    }
}

