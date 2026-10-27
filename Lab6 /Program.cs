using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab6
{
    public class DoublyNode<T>
    {
        public T Data { get; set; }
        public DoublyNode<T>? Next { get; set; }
        public DoublyNode<T>? Prev { get; set; }

        public DoublyNode(T data)
        {
            Data = data;
        }
    }

    public class DoublyLinkedList<T>
    {
        private DoublyNode<T>? _head;
        private DoublyNode<T>? _tail;
        private int _count;

        public int Count => _count;

        // Вставка в початок - O(1)
        public void AddFirst(T data)
        {
            DoublyNode<T> newNode = new DoublyNode<T>(data);
            if (_head == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Next = _head;
                _head.Prev = newNode;
                _head = newNode;
            }
            _count++;
        }

        // Вставка в кінець - O(1)
        public void AddLast(T data)
        {
            DoublyNode<T> newNode = new DoublyNode<T>(data);
            if (_tail == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Prev = _tail;
                _tail.Next = newNode;
                _tail = newNode;
            }
            _count++;
        }

        // Видалення відомого вузла за O(1) без потреби шукати попередника!
        public void RemoveNode(DoublyNode<T> node)
        {
            if (node.Prev != null)
                node.Prev.Next = node.Next;
            else
                _head = node.Next;

            if (node.Next != null)
                node.Next.Prev = node.Prev;
            else
                _tail = node.Prev;

            _count--;
        }

        // Прямий обхід (від голови до хвоста)
        public void PrintForward()
        {
            Console.Write($"Прямий обхід [Count={_count}]: null <- ");
            DoublyNode<T>? curr = _head;
            while (curr != null)
            {
                Console.Write($"[{curr.Data}] <-> ");
                curr = curr.Next;
            }
            Console.WriteLine("null");
        }

        // Зворотний обхід (від хвоста до голови)
        public void PrintBackward()
        {
            Console.Write($"Зворотний обхід [Count={_count}]: null <- ");
            DoublyNode<T>? curr = _tail;
            while (curr != null)
            {
                Console.Write($"[{curr.Data}] <-> ");
                curr = curr.Prev;
            }
            Console.WriteLine("null");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine(" РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №6: ДВОЗВ'ЯЗНИЙ СПИСОК");
            Console.WriteLine("==========================================================================");

            DoublyLinkedList<int> dList = new DoublyLinkedList<int>();

            dList.AddLast(10);
            dList.AddLast(20);
            dList.AddLast(30);
            dList.AddFirst(5);

            Console.WriteLine("--- Двостороннє виведення списку ---");
            dList.PrintForward();
            dList.PrintBackward();

            Console.WriteLine("--- Тестування двозов'язних посилань завершено! ---");
        }
    }
}
