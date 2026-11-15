using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab25
{
    public class TreeNode
    {
        public int Value;
        public TreeNode? Left;
        public TreeNode? Right;

        public TreeNode(int val) { Value = val; }
    }

    public class BinarySearchTree
    {
        public TreeNode? Root;

        // 1. Вставка нового значення в BST - O(log n)
        public void Insert(int val)
        {
            Root = InsertRec(Root, val);
        }

        private TreeNode InsertRec(TreeNode? node, int val)
        {
            if (node == null) return new TreeNode(val);

            if (val < node.Value)
                node.Left = InsertRec(node.Left, val);
            else if (val > node.Value)
                node.Right = InsertRec(node.Right, val);

            return node;
        }

        // 2. Пошук значення в BST - O(log n)
        public bool Search(int val)
        {
            return SearchRec(Root, val);
        }

        private bool SearchRec(TreeNode? node, int val)
        {
            if (node == null) return false;
            if (node.Value == val) return true;

            if (val < node.Value)
                return SearchRec(node.Left, val);
            else
                return SearchRec(node.Right, val);
        }

        // 3. Видалення вузла (обробка 3 випадків)
        public void Delete(int val)
        {
            Root = DeleteRec(Root, val);
        }

        private TreeNode? DeleteRec(TreeNode? node, int val)
        {
            if (node == null) return null;

            if (val < node.Value)
                node.Left = DeleteRec(node.Left, val);
            else if (val > node.Value)
                node.Right = DeleteRec(node.Right, val);
            else
            {
                // Знайдено вузол для видалення!
                // Випадок 1 та 2: вузол з 0 або 1 дитиною
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                // Випадок 3: вузол з 2 нащадками
                // Знаходимо найменший елемент у правому піддереві (In-order Successor)
                node.Value = MinValue(node.Right);

                // Видаляємо дублікат наступника з правого піддерева
                node.Right = DeleteRec(node.Right, node.Value);
            }

            return node;
        }

        private int MinValue(TreeNode node)
        {
            int minv = node.Value;
            while (node.Left != null)
            {
                minv = node.Left.Value;
                node = node.Left;
            }
            return minv;
        }

        // ВАРІАНТ 2: Перевірка, чи є довільне бінарне дерево валідним BST
        public bool IsValidBST(TreeNode? node)
        {
            return ValidateRec(node, long.MinValue, long.MaxValue);
        }

        private bool ValidateRec(TreeNode? node, long minVal, long maxVal)
        {
            if (node == null) return true;

            // Поточний вузол має строго входити в допустимий діапазон (minVal, maxVal)
            if (node.Value <= minVal || node.Value >= maxVal)
                return false;

            return ValidateRec(node.Left, minVal, node.Value) &&
                   ValidateRec(node.Right, node.Value, maxVal);
        }

        // Симетричний обхід для виведення відсортованого списку
        public void PrintInOrder(TreeNode? node)
        {
            if (node != null)
            {
                PrintInOrder(node.Left);
                Console.Write($"{node.Value} ");
                PrintInOrder(node.Right);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==========================================================");
            Console.WriteLine("   РОЗУМАТИК // ЛАБ. №25: БІНАРНЕ ДЕРЕВО ПОШУКУ (BST)");
            Console.WriteLine("==========================================================");

            BinarySearchTree bst = new BinarySearchTree();

            // Вставка елементів
            int[] values = { 50, 30, 70, 20, 40, 60, 80, 65 };
            Console.WriteLine("Вставка значень: [ " + string.Join(", ", values) + " ]");
            foreach (int v in values) bst.Insert(v);

            Console.Write("\nВпорядкований In-Order вивід дерева: ");
            bst.PrintInOrder(bst.Root);
            Console.WriteLine();

            Console.WriteLine("\n--- Пошук елементів ---");
            Console.WriteLine($" 🔍 Пошук 65: {bst.Search(65)} (Очікується True)");
            Console.WriteLine($" 🔍 Пошук 99: {bst.Search(99)} (Очікується False)");

            Console.WriteLine("\n--- Видалення вузла 70 (має 2 дітей: 60 і 80) ---");
            bst.Delete(70);

            Console.Write("In-Order після видалення 70: ");
            bst.PrintInOrder(bst.Root);
            Console.WriteLine("\n\n✅ Вузoл 70 успішно замінено його наступником 80 без порушення властивостей BST!");

            Console.WriteLine("\n--- Перевірка валідності BST (Варіант 2) ---");
            Console.WriteLine($" ✔️ Поточне дерево є валідним BST: {bst.IsValidBST(bst.Root)}");

            // Створюємо некоректне дерево вручну для тестування помилки
            TreeNode invalidTree = new TreeNode(50)
            {
                Left = new TreeNode(30),
                Right = new TreeNode(70)
                {
                    Left = new TreeNode(40) // Порушення: 40 < 50, але знаходиться у правому піддереві кореня!
                }
            };
            Console.WriteLine($" ❌ Некоректне дерево є валідним BST: {bst.IsValidBST(invalidTree)}");
        }
    }
}
