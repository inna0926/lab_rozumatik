using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab18
{
    // Варіант 2: Клас об'єктів Student(Name, Grade) для демонстрації стабільності сортування
    public class Student
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Student(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }

        public override string ToString() => $"{Name} ({Grade})";
    }

    class Program
    {
        // 1. Злиття двох відсортованих частин [left..mid] та [mid+1..right]
        static void Merge(int[] arr, int left, int mid, int right, int[] temp)
        {
            int i = left;      // початок лівої частини
            int j = mid + 1;   // початок правої частини
            int k = left;      // індекс у допоміжному масиві

            while (i <= mid && j <= right)
            {
                if (arr[i] <= arr[j]) // <= забезпечує стабільність сортування!
                {
                    temp[k++] = arr[i++];
                }
                else
                {
                    temp[k++] = arr[j++];
                }
            }

            // Дописуємо залишки лівої частини (якщо є)
            while (i <= mid) temp[k++] = arr[i++];

            // Дописуємо залишки правої частини (якщо є)
            while (j <= right) temp[k++] = arr[j++];

            // Копіюємо відсортовану ділянку назад в оригінальний масив
            for (int idx = left; idx <= right; idx++)
            {
                arr[idx] = temp[idx];
            }
        }

        // 2. Рекурсивне розбиття масиву навпіл
        static void MergeSort(int[] arr, int left, int right, int[] temp)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                MergeSort(arr, left, mid, temp);
                MergeSort(arr, mid + 1, right, temp);

                Merge(arr, left, mid, right, temp);
            }
        }

        // Варіант 2: Стабільне сортування злиттям для списку студентів за оцінкою (Grade)
        static void MergeStudents(List<Student> students, int left, int mid, int right, Student[] temp)
        {
            int i = left;
            int j = mid + 1;
            int k = left;

            while (i <= mid && j <= right)
            {
                // Нестрога нерівність <= гарантує, що при однакових оцінках
                // студент із лівої частини (який був раніше у списку) залишиться першим
                if (students[i].Grade <= students[j].Grade)
                {
                    temp[k++] = students[i++];
                }
                else
                {
                    temp[k++] = students[j++];
                }
            }

            while (i <= mid) temp[k++] = students[i++];
            while (j <= right) temp[k++] = students[j++];

            for (int idx = left; idx <= right; idx++)
            {
                students[idx] = temp[idx];
            }
        }

        static void MergeSortStudents(List<Student> students, int left, int right, Student[] temp)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                MergeSortStudents(students, left, mid, temp);
                MergeSortStudents(students, mid + 1, right, temp);

                MergeStudents(students, left, mid, right, temp);
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №18 (ВАРІАНТ 2): MERGESORT (СТАБІЛЬНЕ)");
            Console.WriteLine("=========================================================================");

            int n = 30_000;
            int[] data = new int[n];
            Random rnd = new Random(42);
            for (int i = 0; i < n; i++) data[i] = rnd.Next(1, 100_000);

            int[] temp = new int[n]; // єдиний буфер для економії виділень пам'яті
            Console.WriteLine($"Вхідний масив: N = {n:N0} елементів");

            Stopwatch sw = Stopwatch.StartNew();
            MergeSort(data, 0, data.Length - 1, temp);
            sw.Stop();

            Console.WriteLine($"⏱ Час сортування MergeSort: {sw.Elapsed.TotalMilliseconds:F3} ms");

            // Перевірка впорядкованості
            bool sorted = true;
            for (int i = 0; i < n - 1; i++)
            {
                if (data[i] > data[i + 1]) sorted = false;
            }
            Console.WriteLine($"✅ Гарантована правильність сортування: {sorted}");

            // --- Варіант 2: Демонстрація стабільності на списку об'єктів Student(Name, Grade) ---
            Console.WriteLine("\n--- Варіант 2: Демонстрація стабільності на списку Student(Name, Grade) ---");
            List<Student> students = new List<Student>
            {
                new Student("Олена (1-ша з 90)", 90),
                new Student("Андрій (1-й з 75)", 75),
                new Student("Максим (2-й з 90)", 90),
                new Student("Ірина (1-ша з 100)", 100),
                new Student("Дмитро (2-й з 75)", 75),
                new Student("Софія (3-тя з 90)", 90)
            };

            Console.WriteLine("До сортування:");
            foreach (var s in students) Console.WriteLine($"  • {s}");

            Student[] studentTemp = new Student[students.Count];
            MergeSortStudents(students, 0, students.Count - 1, studentTemp);

            Console.WriteLine("\nПісля стабільного MergeSort за оцінкою (Grade):");
            foreach (var s in students) Console.WriteLine($"  • {s}");

            Console.WriteLine("\n✅ Висновок: Студенти з однаковими балами (75 та 90) зберегли свій початковий порядок!");
        }
    }
}
