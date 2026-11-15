using System;

namespace Rozumatic_ASD_Lab27
{
    public class AVLNode
    {
        public int Key;
        public int Height;
        public AVLNode? Left;
        public AVLNode? Right;

        public AVLNode(int key)
        {
            Key = key;
            Height = 1;
        }
    }

    public class AVLTree
    {
        public AVLNode? Root;

        public int GetHeight(AVLNode? n) => n?.Height ?? 0;

        int GetBalance(AVLNode? n)
        {
            if (n == null) return 0;
            return GetHeight(n.Left) - GetHeight(n.Right);
        }

        // Малий правий поворот навколо вузла y
        AVLNode RightRotate(AVLNode y)
        {
            AVLNode x = y.Left!;
            AVLNode? T2 = x.Right;

            // Виконуємо поворот
            x.Right = y;
            y.Left = T2;

            // Оновлюємо висоти
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x; // новий корінь піддерева
        }

        // Малий лівий поворот навколо вузла x
        AVLNode LeftRotate(AVLNode x)
        {
            AVLNode y = x.Right!;
            AVLNode? T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public void Insert(int key)
        {
            Root = InsertRec(Root, key);
        }

        private AVLNode InsertRec(AVLNode? node, int key)
        {
            // 1. Звичайна вставка BST
            if (node == null) return new AVLNode(key);

            if (key < node.Key)
                node.Left = InsertRec(node.Left, key);
            else if (key > node.Key)
                node.Right = InsertRec(node.Right, key);
            else
                return node; // дублікати заборонено

            // 2. Оновлення висоти поточного вузла
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // 3. Отримання баланс-фактора (різниця висот лівого і правого піддерев)
            int balance = GetBalance(node);

            // 4. Балансування при відхиленні |balance| > 1:

            // Case 1: Left-Left (Малий правий)
            if (balance > 1 && key < node.Left!.Key)
                return RightRotate(node);

            // Case 2: Right-Right (Малий лівий)
            if (balance < -1 && key > node.Right!.Key)
                return LeftRotate(node);

            // Case 3: Left-Right (Великий лівий: спочатку лівий, потім правий)
            if (balance > 1 && key > node.Left!.Key)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Case 4: Right-Left (Великий правий: спочатку правий, потім лівий)
            if (balance < -1 && key < node.Right!.Key)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        public void PrintPreOrder(AVLNode? node)
        {
            if (node != null)
            {
                Console.Write($"{node.Key}(h={node.Height}, b={GetBalance(node)}) ");
                PrintPreOrder(node.Left);
                PrintPreOrder(node.Right);
            }
        }
    }

    // ВАРІАНТ 2: Допоміжний клас звичайного BST для порівняння глибини
    public class SimpleBSTNode
    {
        public int Key;
        public SimpleBSTNode? Left;
        public SimpleBSTNode? Right;
        public SimpleBSTNode(int key) { Key = key; }
    }

    public class SimpleBST
    {
        public SimpleBSTNode? Root;

        // Ітеративна вставка, щоб уникнути StackOverflow на 1000 відсортованих елементах
        public void Insert(int key)
        {
            if (Root == null)
            {
                Root = new SimpleBSTNode(key);
                return;
            }

            SimpleBSTNode current = Root;
            while (true)
            {
                if (key < current.Key)
                {
                    if (current.Left == null)
                    {
                        current.Left = new SimpleBSTNode(key);
                        return;
                    }
                    current = current.Left;
                }
                else if (key > current.Key)
                {
                    if (current.Right == null)
                    {
                        current.Right = new SimpleBSTNode(key);
                        return;
                    }
                    current = current.Right;
                }
                else return;
            }
        }

        // Обчислення глибини (висоти) без ризику переповнення стеку
        public int GetDepth()
        {
            int depth = 0;
            SimpleBSTNode? current = Root;
            while (current != null)
            {
                depth++;
                current = current.Right ?? current.Left;
            }
            return depth;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==========================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №27: САМОБАЛАНСОВАНЕ AVL-ДЕРЕВО");
            Console.WriteLine("==========================================================");

            AVLTree tree = new AVLTree();

            // Вставляємо строго зростаючу послідовність чисел (яка перетворила б звичайне BST на список)
            int[] sequence = { 10, 20, 30, 40, 50, 25 };
            Console.WriteLine("Послідовна вставка чисел: [ " + string.Join(", ", sequence) + " ]\n");

            foreach (int val in sequence)
            {
                Console.WriteLine($"➕ Вставка {val}...");
                tree.Insert(val);
                Console.Write("   Стан дерева (PreOrder): ");
                tree.PrintPreOrder(tree.Root);
                Console.WriteLine("\n");
            }

            Console.WriteLine($"🎯 Фінальний корінь дерева: {tree.Root?.Key} (Дерево ідеально збалансоване!)");

            // ВАРІАНТ 2: Порівняння глибини звичайного BST та AVL-дерева на 1000 відсортованих числах
            Console.WriteLine("\n==========================================================");
            Console.WriteLine("   ВАРІАНТ 2: ПОРІВНЯННЯ ГЛИБИНИ НА 1000 ВІДСОРТОВАНИХ ЧИСЛАХ");
            Console.WriteLine("==========================================================");

            SimpleBST regularBst = new SimpleBST();
            AVLTree avlTree1000 = new AVLTree();

            for (int i = 1; i <= 1000; i++)
            {
                regularBst.Insert(i);
                avlTree1000.Insert(i);
            }

            Console.WriteLine($"📉 Глибина звичайного BST (1..1000): {regularBst.GetDepth()} (виродилося у зв'язний список, O(n))");
            Console.WriteLine($"📈 Глибина AVL-дерева    (1..1000): {avlTree1000.GetHeight(avlTree1000.Root)} (збалансоване, O(log n))");
        }
    }
}
