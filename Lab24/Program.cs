using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab24
{
    public class MyHashTableDoubleHashing<K, V>
    {
        private enum CellState { Empty, Occupied, Deleted }

        private struct Cell
        {
            public K Key;
            public V Value;
            public CellState State;
        }

        private Cell[] _cells;
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _cells.Length;
        public double LoadFactor => (double)_count / _cells.Length;

        public MyHashTableDoubleHashing(int capacity = 8)
        {
            _cells = new Cell[capacity];
            for (int i = 0; i < capacity; i++) _cells[i].State = CellState.Empty;
        }

        // Перша хеш-функція (початковий індекс)
        private int Hash1(K key)
        {
            return (key!.GetHashCode() & 0x7FFFFFFF) % _cells.Length;
        }

        // ВАРІАНТ 2: Друга хеш-функція (крок зсуву для подвійного хешування)
        // Повертає непарне число >= 1, щоб крок завжди був взаємно простим із розміром масиву (степенем двійки)
        private int Hash2(K key)
        {
            int h = key!.GetHashCode() & 0x7FFFFFFF;
            return ((h % (_cells.Length - 1)) | 1);
        }

        // Вставка з подвійним хешуванням (Double Hashing)
        public void Put(K key, V value)
        {
            if (LoadFactor >= 0.65) Resize(_cells.Length * 2);

            int startIdx = Hash1(key);
            int step = Hash2(key);
            int firstDeletedIdx = -1;

            for (int i = 0; i < _cells.Length; i++)
            {
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Occupied)
                {
                    if (Equals(_cells[idx].Key, key))
                    {
                        _cells[idx].Value = value; // Оновлення значення
                        return;
                    }
                }
                else if (_cells[idx].State == CellState.Deleted)
                {
                    if (firstDeletedIdx == -1) firstDeletedIdx = idx;
                }
                else if (_cells[idx].State == CellState.Empty)
                {
                    // Якщо раніше зустріли комірку Deleted - вставляємо туди для економії
                    int insertIdx = firstDeletedIdx != -1 ? firstDeletedIdx : idx;
                    _cells[insertIdx].Key = key;
                    _cells[insertIdx].Value = value;
                    _cells[insertIdx].State = CellState.Occupied;
                    _count++;
                    return;
                }
            }
        }

        // Пошук з пропуском комірок Deleted
        public bool TryGet(K key, out V value)
        {
            int startIdx = Hash1(key);
            int step = Hash2(key);

            for (int i = 0; i < _cells.Length; i++)
            {
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Empty)
                {
                    break; // якщо зустріли Empty - ключа точно немає
                }

                if (_cells[idx].State == CellState.Occupied && Equals(_cells[idx].Key, key))
                {
                    value = _cells[idx].Value;
                    return true;
                }
            }

            value = default!;
            return false;
        }

        // Видалення із встановленням маркера Deleted (Tombstone)
        public bool Remove(K key)
        {
            int startIdx = Hash1(key);
            int step = Hash2(key);

            for (int i = 0; i < _cells.Length; i++)
            {
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Empty) break;

                if (_cells[idx].State == CellState.Occupied && Equals(_cells[idx].Key, key))
                {
                    _cells[idx].State = CellState.Deleted; // Позначаємо видаленим!
                    _cells[idx].Key = default!;
                    _cells[idx].Value = default!;
                    _count--;
                    return true;
                }
            }
            return false;
        }

        private void Resize(int newCapacity)
        {
            Console.WriteLine($" ⚡ [RESIZE] Збільшення місткості до {newCapacity}");
            Cell[] oldCells = _cells;
            _cells = new Cell[newCapacity];
            for (int i = 0; i < newCapacity; i++) _cells[i].State = CellState.Empty;
            _count = 0;

            foreach (var c in oldCells)
            {
                if (c.State == CellState.Occupied) Put(c.Key, c.Value);
            }
        }

        public void Print()
        {
            Console.WriteLine($"--- Стан комірок (Count={_count}/{Capacity}) ---");
            for (int i = 0; i < _cells.Length; i++)
            {
                string stateStr = _cells[i].State switch
                {
                    CellState.Occupied => $"[{_cells[i].Key} : {_cells[i].Value}]",
                    CellState.Deleted => "💀 [DELETED]",
                    _ => "⚪ [EMPTY]"
                };
                Console.WriteLine($"  [{i,2}]: {stateStr}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №24: ПОДВІЙНЕ ХЕШУВАННЯ (DOUBLE HASHING)");
            Console.WriteLine("=================================================================");

            MyHashTableDoubleHashing<int, string> table = new MyHashTableDoubleHashing<int, string>();

            table.Put(10, "Зошит");
            table.Put(18, "Ручка (Колізія з 10)");
            table.Put(26, "Олівець (Колізія з 10)");
            table.Put(7,  "Книга");

            table.Print();

            Console.WriteLine("\n--- Видаляємо ключ 18 (Комірка стане DELETED) ---");
            table.Remove(18);
            table.Print();

            Console.WriteLine("\n--- Перевірка пошуку ключа 26 (повинен знайти через DELETED) ---");
            if (table.TryGet(26, out string val))
            {
                Console.WriteLine($" 🎯 Успішно знайдено: {val}");
            }

            Console.WriteLine("\n✅ Маркер DELETED зберіг цілісність ланцюжка подвійного хешування!");
        }
    }
}
