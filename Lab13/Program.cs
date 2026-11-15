using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab13
{
    public class PriorityItem<T>
    {
        public T Data { get; set; }
        public int Priority { get; set; } // Чим більше число - тим вищий пріоритет!

        public PriorityItem(T data, int priority)
        {
            Data = data;
            Priority = priority;
        }
    }

    // Варіант 2: Черга з впорядкованою вставкою (Insertion Sort при Enqueue), щоб Dequeue працював за O(1)
    public class MyPriorityQueue<T>
    {
        private List<PriorityItem<T>> _items = new List<PriorityItem<T>>();

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        // Впорядкована вставка - O(n): підтримуємо список відсортованим за зростанням пріоритету
        // Найвищий пріоритет завжди опиняється в кінці списку
        public void Enqueue(T data, int priority)
        {
            var newItem = new PriorityItem<T>(data, priority);
            int index = 0;

            // Шукаємо позицію для вставки (використовуємо <=, щоб зберегти порядок FIFO для однакових пріоритетів)
            while (index < _items.Count && _items[index].Priority <= priority)
            {
                index++;
            }

            _items.Insert(index, newItem);
        }

        // Вилучення елемента з найвищим пріоритетом - O(1), оскільки він завжди останній у списку
        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Черга порожня!");

            int lastIndex = _items.Count - 1;
            T result = _items[lastIndex].Data;
            _items.RemoveAt(lastIndex); // Видалення з кінця List<T> виконується за O(1)
            return result;
        }

        public void Print()
        {
            Console.WriteLine($"[PriorityQueue Count={Count}]:");
            foreach (var item in _items)
            {
                Console.WriteLine($"  🔹 Пріоритет [{item.Priority,2}] -> {item.Data}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №13 (ВАРІАНТ 2): PRIORITY QUEUE");
            Console.WriteLine("=========================================================================");

            MyPriorityQueue<string> hospitalQueue = new MyPriorityQueue<string>();

            // Додаємо пацієнтів у приймальне відділення з різною терміновістю (1-10)
            hospitalQueue.Enqueue("Пацієнт А (Плановий огляд)", priority: 1);
            hospitalQueue.Enqueue("Пацієнт Б (Гострий апендицит)", priority: 9);
            hospitalQueue.Enqueue("Пацієнт В (Застуда)", priority: 2);
            hospitalQueue.Enqueue("Пацієнт Г (Інфаркт міокарда)", priority: 10);
            hospitalQueue.Enqueue("Пацієнт Д (Перелом пальця)", priority: 5);

            // Під час виводу видно, що список вже відсортований за пріоритетом
            hospitalQueue.Print();

            Console.WriteLine("\n--- Черговість прийому лікарями за пріоритетом ---");
            int order = 1;
            while (!hospitalQueue.IsEmpty)
            {
                string patient = hospitalQueue.Dequeue();
                Console.WriteLine($"  🏥 #{order++} На прийом викликається: {patient}");
            }

            Console.WriteLine("\n✅ Усі пацієнти прийняті в строгому порядку пріоритету!");
        }
    }
}
