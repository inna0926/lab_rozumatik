using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab26
{
    public class TreeNode
    {
        public string Name;
        public TreeNode? Left;
        public TreeNode? Right;

        public TreeNode(string name) { Name = name; }
    }

    class Program
    {
        // 1. Pre-Order (V-L-R)
        static void PreOrder(TreeNode? node)
        {
            if (node == null) return;
            Console.Write($"{node.Name} ");
            PreOrder(node.Left);
            PreOrder(node.Right);
        }

        // 2. In-Order (L-V-R) — рекурсивний
        static void InOrder(TreeNode? node)
        {
            if (node == null) return;
            InOrder(node.Left);
            Console.Write($"{node.Name} ");
            InOrder(node.Right);
        }

        // ВАРІАНТ 2: Ітеративний In-Order (L-V-R) через явний Stack<TreeNode> без рекурсії
        static void InOrderIterative(TreeNode? root)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNode? current = root;

            while (current != null || stack.Count > 0)
            {
                // Йдемо максимально вліво, зберігаючи вузли у стек
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                // Дістаємо вузол зі стеку, виводимо та переходимо в праве піддерево
                current = stack.Pop();
                Console.Write($"{current.Name} ");
                current = current.Right;
            }
        }

        // 3. Post-Order (L-R-V)
        static void PostOrder(TreeNode? node)
        {
            if (node == null) return;
            PostOrder(node.Left);
            PostOrder(node.Right);
            Console.Write($"{node.Name} ");
        }

        // 4. Level-Order (BFS) через чергу
        static void LevelOrder(TreeNode? root)
        {
            if (root == null) return;

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                TreeNode current = queue.Dequeue();
                Console.Write($"{current.Name} ");

                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==========================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №26: ОБХОДИ БІНАРНИХ ДЕРЕВ (DFS / BFS)");
            Console.WriteLine("==========================================================");

            // Побудова ієрархічного дерева:
            //       A
            //      / \
            //     B   C
            //    / \ / \
            //   D  E F  G

            TreeNode root = new TreeNode("A")
            {
                Left = new TreeNode("B") { Left = new TreeNode("D"), Right = new TreeNode("E") },
                Right = new TreeNode("C") { Left = new TreeNode("F"), Right = new TreeNode("G") }
            };

            Console.WriteLine("Дерево побудовано з коренем 'A'. Результати обходів:\n");

            Console.Write("1. 🌲 Pre-order  (Прямий, V-L-R):      "); PreOrder(root); Console.WriteLine();
            Console.Write("2. 🌲 In-order   (Симетричний, L-V-R): "); InOrder(root); Console.WriteLine();
            Console.Write("3. 🌲 Post-order (Зворотний, L-R-V):   "); PostOrder(root); Console.WriteLine();
            Console.Write("4. 🌊 Level-order (Ширина, BFS черга): "); LevelOrder(root); Console.WriteLine();

            Console.Write("\n5. 📚 In-order ітеративний (Варіант 2, Stack): ");
            InOrderIterative(root);
            Console.WriteLine();

            Console.WriteLine("\n✅ Усі 4 класичні обходи та ітеративний In-order виконано коректно!");
        }
    }
}
