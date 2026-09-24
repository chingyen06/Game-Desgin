using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int row, col, index;
            Console.Write("Generating rectangular 2d array: \nrow: ");
            row = int.Parse(Console.ReadLine());
            Console.Write("col: ");
            col = int.Parse(Console.ReadLine());

            int[,] array = new int[row, col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    array[i, j] = i * col + (j + 1);
                }
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write($"{array[i, j],2} ");
                }
                Console.WriteLine();
            }

            while (true)
            {
                Console.WriteLine();
                Console.Write("give the index to show: ");
                index = int.Parse(Console.ReadLine());

                if (index >= row * col || index < 0)
                {
                    Console.Write("索引超出範圍！\n\n感謝您使用本程式！");
                    break;
                }

                Console.WriteLine($"在索引順序 {index} 的值: {array[index / col, index % col]}");
            }
        }
    }
}
