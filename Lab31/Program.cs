using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab31
{
    public class Edge
    {
        public int To;
        public int DistanceKm;     // Відстань у кілометрах
        public int BaseMinutes;    // Базовий час у дорозі без заторів (хв)
        public int TrafficDelay;   // Затримка через затори (хв)

        // ВАРІАНТ 2: Загальна вага ребра — сумарний час у хвилинах з урахуванням заторів
        public int TotalMinutes => BaseMinutes + TrafficDelay;

        public Edge(int to, int distanceKm, int baseMinutes, int trafficDelay = 0)
        {
            To = to;
            DistanceKm = distanceKm;
            BaseMinutes = baseMinutes;
            TrafficDelay = trafficDelay;
        }
    }

    class Program
    {
        static void Dijkstra(int start, int target, Dictionary<int, List<Edge>> graph, string[] cityNames)
        {
            int n = cityNames.Length;
            int[] dist = new int[n]; // Мінімальний час у хвилинах
            int[] prev = new int[n];
            bool[] visited = new bool[n];

            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }

            dist[start] = 0;

            for (int count = 0; count < n; count++)
            {
                // Знаходимо невідвідану вершину з мінімальним часом у дорозі
                int u = -1;
                int minDist = int.MaxValue;

                for (int i = 0; i < n; i++)
                {
                    if (!visited[i] && dist[i] < minDist)
                    {
                        minDist = dist[i];
                        u = i;
                    }
                }

                if (u == -1 || dist[u] == int.MaxValue) break;
                visited[u] = true;

                if (u == target) break; // досягнуто ціль!

                // Релаксація всіх сусідніх ребер за часом (хвилини + затори)
                foreach (var edge in graph[u])
                {
                    int v = edge.To;
                    int weight = edge.TotalMinutes;

                    if (!visited[v] && dist[u] + weight < dist[v])
                    {
                        dist[v] = dist[u] + weight;
                        prev[v] = u;
                    }
                }
            }

            // Виведення результатів
            if (dist[target] == int.MaxValue)
            {
                Console.WriteLine($"Маршруту між {cityNames[start]} та {cityNames[target]} не існує!");
                return;
            }

            int hours = dist[target] / 60;
            int mins = dist[target] % 60;
            Console.WriteLine($"⏱️ Найшвидший час від {cityNames[start]} до {cityNames[target]}: {dist[target]} хв ({hours} год {mins} хв)");

            // Відновлення маршруту
            List<string> route = new List<string>();
            int curr = target;
            while (curr != -1)
            {
                route.Add(cityNames[curr]);
                curr = prev[curr];
            }
            route.Reverse();

            Console.WriteLine("📍 Оптимальний маршрут GPS (з урахуванням заторів):");
            Console.WriteLine("   " + string.Join(" -> ", route));
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №31: АЛГОРИТМ ДЕЙКСТРИ (GPS + ЗАТОРИ)");
            Console.WriteLine("=================================================================");

            string[] cities = { "Житомир", "Київ", "Вінниця", "Рівне", "Львів", "Одеса" };
            // 0: Житомир, 1: Київ, 2: Вінниця, 3: Рівне, 4: Львів, 5: Одеса

            var map = new Dictionary<int, List<Edge>>();
            for (int i = 0; i < cities.Length; i++) map[i] = new List<Edge>();

            // Додаємо дороги: (місто A, місто B, відстань км, базовий час хв, затор хв)
            void AddRoad(int u, int v, int km, int baseMin, int trafficMin)
            {
                map[u].Add(new Edge(v, km, baseMin, trafficMin));
                map[v].Add(new Edge(u, km, baseMin, trafficMin));
            }

            AddRoad(0, 1, 140, 100, 65); // Житомир - Київ (сильний затор на в'їзді: +65 хв)
            AddRoad(0, 2, 128, 105, 10); // Житомир - Вінниця (вільна траса: +10 хв)
            AddRoad(0, 3, 188, 130, 15); // Житомир - Рівне (+15 хв)
            AddRoad(3, 4, 210, 155, 25); // Рівне - Львів (+25 хв)
            AddRoad(2, 5, 425, 310, 20); // Вінниця - Одеса (+20 хв)
            AddRoad(1, 5, 475, 320, 45); // Київ - Одеса (+45 хв)

            Console.WriteLine("Запуск навігатора: Житомир -> Львів...");
            Dijkstra(start: 0, target: 4, map, cities);

            Console.WriteLine("\nЗапуск навігатора: Житомир -> Одеса (порівняння трас через Київ і Вінницю)...");
            Dijkstra(start: 0, target: 5, map, cities);
        }
    }
}
