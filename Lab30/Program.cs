using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab30
{
    class Program
    {
        enum Color { White, Gray, Black }

        // 1. Пошук циклів за допомогою триколірного DFS
        static bool HasCycleDFS(int u, Dictionary<int, List<int>> graph, Color[] colors)
        {
            colors[u] = Color.Gray; // у процесі обробки (в поточному стеку викликів)

            foreach (int v in graph[u])
            {
                if (colors[v] == Color.Gray)
                {
                    Console.WriteLine($" ⚠️ Виявлено зворотне ребро {u} -> {v}: ГРАФ МІСТИТЬ ЦИКЛ!");
                    return true;
                }

                if (colors[v] == Color.White && HasCycleDFS(v, graph, colors))
                {
                    return true;
                }
            }

            colors[u] = Color.Black; // повністю оброблено
            return false;
        }

        // 2. Топологічне сортування (додавання у стек на виході з DFS)
        static void TopoSortDFS(int u, Dictionary<int, List<int>> graph, bool[] visited, Stack<int> result)
        {
            visited[u] = true;

            foreach (int v in graph[u])
            {
                if (!visited[v])
                {
                    TopoSortDFS(v, graph, visited, result);
                }
            }

            result.Push(u); // кладемо на вершину стеку
        }

        // ВАРІАНТ 2: Алгоритм Кана (Kahn's Algorithm) через вхідні степені вершин (in-degree)
        static List<int>? TopoSortKahn(int vertices, Dictionary<int, List<int>> graph)
        {
            int[] inDegree = new int[vertices];

            // 1. Обчислюємо вхідні степені (in-degree) для кожної вершини
            for (int u = 0; u < vertices; u++)
            {
                foreach (int v in graph[u])
                {
                    inDegree[v]++;
                }
            }

            // 2. Додаємо у чергу всі вершини, які не мають залежностей (in-degree == 0)
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < vertices; i++)
            {
                if (inDegree[i] == 0)
                {
                    queue.Enqueue(i);
                }
            }

            List<int> topoOrder = new List<int>();

            // 3. Послідовно обробляємо чергу та зменшуємо in-degree сусідів
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                topoOrder.Add(u);

                foreach (int v in graph[u])
                {
                    inDegree[v]--;
                    if (inDegree[v] == 0)
                    {
                        queue.Enqueue(v);
                    }
                }
            }

            // Якщо відсортовано менше вершин, ніж є в графі — у графі існує цикл
            if (topoOrder.Count != vertices)
                return null;

            return topoOrder;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №30: DFS, ПОШУК ЦИКЛІВ ТА ТОПОЛОГІЧНЕ СОРТУВАННЯ");
            Console.WriteLine("=================================================================");

            // Граф залежностей між навчальними предметами:
            // 0: Основи програмування -> 1: ООП
            // 1: ООП -> 2: Алгоритми та структури даних
            // 2: Алгоритми -> 3: Системи штучного інтелекту
            // 0: Основи -> 4: Архітектура комп'ютерів
            int vertices = 5;
            var courseDependencies = new Dictionary<int, List<int>>
            {
                [0] = new List<int> { 1, 4 },
                [1] = new List<int> { 2 },
                [2] = new List<int> { 3 },
                [3] = new List<int> { },
                [4] = new List<int> { }
            };

            Color[] colors = new Color[vertices];
            bool hasCycle = false;
            for (int i = 0; i < vertices; i++)
            {
                if (colors[i] == Color.White)
                {
                    if (HasCycleDFS(i, courseDependencies, colors))
                    {
                        hasCycle = true;
                        break;
                    }
                }
            }

            Console.WriteLine($"Перевірка на цикли: {(hasCycle ? "Цикл знайдено" : "✅ Граф ациклічний (DAG)")}");

            // Топологічне сортування (DFS)
            bool[] visited = new bool[vertices];
            Stack<int> topoOrder = new Stack<int>();

            for (int i = 0; i < vertices; i++)
            {
                if (!visited[i]) TopoSortDFS(i, courseDependencies, visited, topoOrder);
            }

            string[] courseNames = { "Основи прогр.", "ООП", "Алгоритми", "Штучний інтелект", "Архітектура ПК" };
            Console.WriteLine("\n🎓 Рекомендований порядок вивчення дисциплін (Topological Order - DFS):");
            int step = 1;
            while (topoOrder.Count > 0)
            {
                int c = topoOrder.Pop();
                Console.WriteLine($"   Крок {step++}: [{c}] {courseNames[c]}");
            }

            // ВАРІАНТ 2: Перевірка алгоритму Кана
            Console.WriteLine("\n🎓 Топологічне сортування за алгоритмом Кана (Варіант 2, in-degree):");
            var kahnResult = TopoSortKahn(vertices, courseDependencies);
            if (kahnResult != null)
            {
                step = 1;
                foreach (int c in kahnResult)
                {
                    Console.WriteLine($"   Крок {step++}: [{c}] {courseNames[c]}");
                }
            }
            else
            {
                Console.WriteLine("   ⚠️ Сортування неможливе: у графі виявлено цикл!");
            }
        }
    }
}
