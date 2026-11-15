using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rozumatic_ASD_Lab11
{
    public class ThreadSafeCircularQueue<T>
    {
        private readonly T[] _buffer;
        private int _head = 0;
        private int _tail = 0;
        private int _count = 0;

        // Об'єкт для синхронізації потоків
        private readonly object _lock = new object();

        public int Count
        {
            get { lock (_lock) return _count; }
        }

        public int Capacity => _buffer.Length;

        public bool IsEmpty
        {
            get { lock (_lock) return _count == 0; }
        }

        public bool IsFull
        {
            get { lock (_lock) return _count == _buffer.Length; }
        }

        public ThreadSafeCircularQueue(int capacity = 5)
        {
            if (capacity <= 0)
                throw new ArgumentException("Ємність буфера повинна бути більшою за 0.", nameof(capacity));

            _buffer = new T[capacity];
        }

        // Потокобезпечне додавання в чергу (Producer) - O(1)
        public void Enqueue(T item)
        {
            lock (_lock)
            {
                // Якщо черга заповнена, потік-виробник чекає, поки споживач звільнить місце
                while (_count == _buffer.Length)
                {
                    Console.WriteLine($"[Producer] Буфер повний. Очікування вільного місця для '{item}'...");
                    Monitor.Wait(_lock);
                }

                _buffer[_tail] = item;
                _tail = (_tail + 1) % _buffer.Length; // Циклічний перехід
                _count++;

                Console.WriteLine($"[Producer] Додано: {item} | Стан: [Count={_count}/{Capacity}, Head={_head}, Tail={_tail}]");

                // Сповіщаємо потоки-споживачі, що з'явився новий елемент
                Monitor.PulseAll(_lock);
            }
        }

        // Потокобезпечне вилучення з черги (Consumer) - O(1)
        public T Dequeue()
        {
            lock (_lock)
            {
                // Якщо черга порожня, потік-споживач чекає на нові дані
                while (_count == 0)
                {
                    Console.WriteLine("[Consumer] Буфер порожній. Очікування нових даних...");
                    Monitor.Wait(_lock);
                }

                T item = _buffer[_head];
                _buffer[_head] = default!; // Очищення посилання
                _head = (_head + 1) % _buffer.Length; // Циклічний перехід
                _count--;

                Console.WriteLine($"[Consumer] Оброблено: {item} | Стан: [Count={_count}/{Capacity}, Head={_head}, Tail={_tail}]");

                // Сповіщаємо потоки-виробники, що звільнилося місце
                Monitor.PulseAll(_lock);

                return item;
            }
        }

        // Перегляд першого елемента без вилучення - O(1)
        public T Peek()
        {
            lock (_lock)
            {
                if (_count == 0)
                    throw new InvalidOperationException("Черга порожня!");

                return _buffer[_head];
            }
        }

        // Потокобезпечний вивід стану черги
        public void Print()
        {
            lock (_lock)
            {
                Console.Write($"[Count={_count}/{Capacity}, Head={_head}, Tail={_tail}]: ");
                if (_count == 0)
                {
                    Console.WriteLine("ПОРОЖНЯ");
                    return;
                }

                Console.Write("HEAD -> [ ");
                for (int i = 0; i < _count; i++)
                {
                    int index = (_head + i) % _buffer.Length;
                    Console.Write($"{_buffer[index]} ");
                }
                Console.WriteLine("] -> TAIL");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Коректне відображення кирилиці в онлайн-компіляторах
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("==================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №11: ПОТОКОБЕЗПЕЧНИЙ КІЛЬЦЕВИЙ БУФЕР");
            Console.WriteLine("==================================================================\n");

            // Створюємо буфер невеликої ємності (3), щоб наочно побачити блокування при заповненні
            var queue = new ThreadSafeCircularQueue<string>(capacity: 3);

            string[] documents =
            {
                "Doc1.pdf", "Report.docx", "Photo.png",
                "Invoice.xlsx", "Presentation.pptx", "Certificate.pdf"
            };

            // Потік-виробник (Producer): додає документи в чергу
            Task producer = Task.Run(() =>
            {
                foreach (var doc in documents)
                {
                    queue.Enqueue(doc);
                    Thread.Sleep(200); // Імітація швидкого надходження завдань
                }
                Console.WriteLine("\n>>> [Producer] Усі документи успішно відправлено в чергу!\n");
            });

            // Потік-споживач (Consumer): забирає документи на друк
            Task consumer = Task.Run(() =>
            {
                for (int i = 0; i < documents.Length; i++)
                {
                    Thread.Sleep(450); // Імітація повільнішого друку (буфер встигатиме заповнюватися)
                    queue.Dequeue();
                }
                Console.WriteLine("\n>>> [Consumer] Усі документи успішно надруковано!\n");
            });

            // Очікуємо завершення обох потоків
            Task.WaitAll(producer, consumer);

            Console.Write("Фінальний стан черги: ");
            queue.Print();
            Console.WriteLine("\nРоботу потокобезпечного кільцевого буфера перевірено успішно!");
        }
    }
}
