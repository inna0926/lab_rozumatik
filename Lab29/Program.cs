using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab29
{
    class Program
    {
        // Пошук у ширину BFS з відновленням найкоротшого шляху
        static List<int>? ShortestPathBFS(Dictionary<int, List<int>> graph, int start, int target)
        {
            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();

            queue.Enqueue(start);
            visited.Add(start);

            bool found = false;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == target)
                {
                    found = true;
                    break;
                }

                foreach (int neighbor in graph[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parent[neighbor] = current; // Запам'ятовуємо, звідки прийшли
                        queue.Enqueue(neighbor);
                    }
                }
            }

            if (!found) return null; // шляху немає

            // Відновлюємо шлях від фінішу до старту за масивом батьків
            List<int> path = new List<int>();
            int curr = target;
            while (curr != start)
            {
                path.Add(curr);
                curr = parent[curr];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        // ВАРІАНТ 2: Обчислення найкоротших відстаней від стартової вершини до ВСІХ інших вершин графа
        static Dictionary<int, int> GetAllDistancesBFS(Dictionary<int, List<int>> graph, int start)
        {
            Dictionary<int, int> distances = new Dictionary<int, int>();
            Queue<int> queue = new Queue<int>();

            // Ініціалізуємо відстані значенням -1 (означає, що вершина ще не відвідана / недосяжна)
            foreach (var vertex in graph.Keys)
            {
                distances[vertex] = -1;
            }

            distances[start] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                foreach (int neighbor in graph[current])
                {
                    if (distances[neighbor] == -1)
                    {
                        distances[neighbor] = distances[current] + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return distances;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №29: ПОШУК У ШИРИНУ (BFS) ТА НАЙКОРОТШИЙ ШЛЯХ");
            Console.WriteLine("=================================================================");

            // Створюємо граф комп'ютерної мережі (0: Сервер, ..., 6: Клієнт)
            var network = new Dictionary<int, List<int>>
            {
                [0] = new List<int> { 1, 2 },
                [1] = new List<int> { 0, 3, 4 },
                [2] = new List<int> { 0, 5 },
                [3] = new List<int> { 1, 6 },
                [4] = new List<int> { 1, 5 },
                [5] = new List<int> { 2, 4, 6 },
                [6] = new List<int> { 3, 5 }
            };

            int startNode = 0;
            int targetNode = 6;

            Console.WriteLine($"Шукаємо найкоротший шлях від Вузла [{startNode}] до Вузла [{targetNode}]...");

            List<int>? path = ShortestPathBFS(network, startNode, targetNode);

            if (path != null)
            {
                Console.WriteLine($"\n🎯 Найкоротший шлях знайдено ({path.Count - 1} кроків):");
                Console.WriteLine("   " + string.Join(" -> ", path));
            }
            else
            {
                Console.WriteLine("❌ Шляху між вузлами не існує!");
            }

            Console.WriteLine("\n--- Відстані від стартової вершини до ВСІХ вершин (Варіант 2) ---");
            var allDistances = GetAllDistancesBFS(network, startNode);
            foreach (var kvp in allDistances)
            {
                string distText = kvp.Value == -1 ? "недосяжна" : $"{kvp.Value} крок(ів)";
                Console.WriteLine($" 📍 Відстань від [{startNode}] до [{kvp.Key}]: {distText}");
            }

            Console.WriteLine("\n✅ Алгоритм BFS гарантує знаходження мінімальної кількості ребер!");
        }
    }
}
