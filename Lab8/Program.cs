using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab8
{
    // Обмеження IComparable<T> потрібне для порівняння елементів у Варіанті 2
    public class MinStack<T> where T : IComparable<T>
    {
        private T[] _items;
        private T[] _minItems; // Додатковий масив для зберігання мінімумів на кожному рівні
        private int _top = -1;

        public int Count
        {
            get { return _top + 1; }
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public bool IsEmpty
        {
            get { return _top == -1; }
        }

        public MinStack(int capacity = 5)
        {
            _items = new T[capacity];
            _minItems = new T[capacity]; // Ініціалізуємо масив мінімумів
        }

        // Додавання елемента на вершину стеку - O(1)
        public void Push(T item)
        {
            if (_top == _items.Length - 1)
            {
                throw new StackOverflowException("Стек переповнено! Максимальна місткість: " + _items.Length);
            }

            _top++;
            _items[_top] = item;

            // РЕАЛІЗАЦІЯ ВАРІАНТУ 2: Обчислюємо поточний мінімум за O(1)
            if (_top == 0)
            {
                _minItems[_top] = item;
            }
            else
            {
                T previousMin = _minItems[_top - 1];
                // Якщо новий елемент менший за попередній мінімум, він стає новим мінімумом
                if (item.CompareTo(previousMin) < 0)
                    _minItems[_top] = item;
                else
                    _minItems[_top] = previousMin;
            }
        }

        // Вилучення елемента з вершини стеку - O(1)
        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Неможливо виконати Pop: Стек порожній!");
            }

            T item = _items[_top];
            _items[_top] = default(T);
            _minItems[_top] = default(T); // Очищення для збирача сміття
            _top--;
            return item;
        }

        // Перегляд вершини без вилучення - O(1)
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Стек порожній!");
            return _items[_top];
        }

        // РЕАЛІЗАЦІЯ ВАРІАНТУ 2: Отримання мінімального елемента в стеку за O(1)
        public T GetMin()
        {
            if (IsEmpty) throw new InvalidOperationException("Стек порожній! Мінімальний елемент відсутній.");
            return _minItems[_top];
        }

        public void Print()
        {
            Console.Write("[Count=" + Count + "/" + Capacity + "]: Bottom -> [ ");
            for (int i = 0; i <= _top; i++)
            {
                Console.Write(_items[i] + " ");
            }
            Console.WriteLine("] -> TOP");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №8: СТЕК (LIFO) З ПІДТРИМКОЮ GETMIN()");
            Console.WriteLine("==========================================================================");

            // Оскільки в умові завдання тепер стек підтримує пошук мінімуму, 
            // використаємо цілі числа (int), щоб наочно продемонструвати роботу GetMin()
            MinStack<int> undoHistory = new MinStack<int>(4);

            Console.WriteLine("--- 1. Додавання операцій у стек (Push) ---");
            Console.WriteLine("Додаємо: 30"); undoHistory.Push(30);
            Console.WriteLine("Додаємо: 50"); undoHistory.Push(50);
            Console.WriteLine("Додаємо: 10"); undoHistory.Push(10);
            undoHistory.Print();

            Console.WriteLine("\n🔍 Поточна вершина (Peek): " + undoHistory.Peek());
            Console.WriteLine("📉 Мінімальний елемент у стеку за O(1) (GetMin): " + undoHistory.GetMin());

            Console.WriteLine("\n--- 2. Скасування операцій (Undo / Pop) ---");
            while (!undoHistory.IsEmpty)
            {
                Console.WriteLine("Поточний мінімум перед Pop: " + undoHistory.GetMin());
                int action = undoHistory.Pop();
                Console.WriteLine("🔹 Скасовано (Pop): " + action);
                undoHistory.Print();
            }

            Console.WriteLine("\n--- 3. Перевірка обробки винятків при вилученні з порожнього стеку ---");
            try
            {
                undoHistory.Pop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Перехоплено очікувану помилку: " + ex.Message);
            }

            Console.WriteLine("\n✅ Роботу зі стеком завершено успішно!");
        }
    }
}
