using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab12
{
    // Варіант 2: Реалізація Deque на кільцевому масиві T[] для гарантованого амортизованого O(1)
    public class MyDeque<T>
    {
        private T[] _buffer;
        private int _head;
        private int _tail;
        private int _count;

        public MyDeque(int capacity = 8)
        {
            _buffer = new T[capacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        // Автоматичне збільшення масиву вдвічі при заповненні
        private void EnsureCapacity()
        {
            if (_count == _buffer.Length)
            {
                int newCapacity = _buffer.Length * 2;
                T[] newBuffer = new T[newCapacity];

                for (int i = 0; i < _count; i++)
                {
                    newBuffer[i] = _buffer[(_head + i) % _buffer.Length];
                }

                _buffer = newBuffer;
                _head = 0;
                _tail = _count;
            }
        }

        public void AddFirst(T item)
        {
            EnsureCapacity();
            // Рухаємо початок назад по колу
            _head = (_head - 1 + _buffer.Length) % _buffer.Length;
            _buffer[_head] = item;
            _count++;
        }

        public void AddLast(T item)
        {
            EnsureCapacity();
            _buffer[_tail] = item;
            // Рухаємо кінець вперед по колу
            _tail = (_tail + 1) % _buffer.Length;
            _count++;
        }

        public T RemoveFirst()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            
            T val = _buffer[_head];
            _buffer[_head] = default!; // Очищення посилання для збирача сміття
            _head = (_head + 1) % _buffer.Length;
            _count--;
            return val;
        }

        public T RemoveLast()
        {
            if (IsEmpty) throw new InvalidOperationException("Deque порожній!");
            
            _tail = (_tail - 1 + _buffer.Length) % _buffer.Length;
            T val = _buffer[_tail];
            _buffer[_tail] = default!;
            _count--;
            return val;
        }
    }

    class Program
    {
        // Алгоритм перевірки паліндрома за допомогою Deque
        static bool IsPalindrome(string text)
        {
            MyDeque<char> deque = new MyDeque<char>();

            // Очищаємо текст від пробілів, розділових знаків та зводимо до нижнього регістру
            foreach (char c in text.ToLower())
            {
                if (char.IsLetterOrDigit(c))
                {
                    deque.AddLast(c);
                }
            }

            // Порівнюємо символи з обох кінців черги
            while (deque.Count > 1)
            {
                char first = deque.RemoveFirst();
                char last = deque.RemoveLast();
                if (first != last) return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("============================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №12 (ВАРІАНТ 2): DEQUE НА КІЛЬЦЕВОМУ МАСИВІ");
            Console.WriteLine("============================================================================");

            string[] samples = {
                "А роза упала на лапу Азора",
                "Я несу гусеня",
                "Козак з казок",
                "Алгоритми та структури даних",
                "12321",
                "123321"
            };

            Stopwatch sw = Stopwatch.StartNew();

            foreach (string s in samples)
            {
                bool result = IsPalindrome(s);
                string status = result ? "✅ ПАЛІНДРОМ" : "❌ НЕ паліндром";
                Console.WriteLine($"  Рядок: \"{s}\" -> {status}");
            }

            sw.Stop();
            Console.WriteLine($"\n⏱ Час перевірки: {sw.Elapsed.TotalMilliseconds:F4} ms");
        }
    }
}
