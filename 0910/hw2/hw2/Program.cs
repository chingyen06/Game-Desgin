using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bingo bingo = new Bingo();
            int num;

            bingo.reset();
            bingo.show();

            while (bingo.countLine() < 5)
            {
                Console.WriteLine("請輸入要標記的數值:");
                num = int.Parse(Console.ReadLine());

                if (bingo.marked(num))
                {
                    Console.WriteLine($"{num} 已標記過了！");
                    Console.WriteLine();
                }
                else
                {
                    bingo.mark(num);
                    Console.Clear();
                    bingo.showMarked('#');
                    //Console.WriteLine($"目前有 {bingo.countLine()} 條線");
                }
            }
        }

        public class Bingo
        {
            private int side = 5;
            private int CellNo;
            int[] nos;
            int[] numbers;
            int[] selected;
            string bar = "";
            int index;

            public Bingo(int side = 5)
            {
                this.side = side;
                CellNo = side * side;

                nos = new int[CellNo];
                numbers = new int[CellNo];
                selected = new int[CellNo];

                for (int k = 0; k < CellNo; k++)
                {
                    nos[k] = k + 1;
                }
                shuffle();

                bar = "+";
                for (int s = 0; s < side; s++)
                {
                    bar += "----+";
                }

                reset();
            }

            public void reset(int[] assigndNumber = null)
            {
                if (assigndNumber != null && assigndNumber.Length == side * side)
                {
                    for (int i = 0; i < side * side; i++)
                    {
                        numbers[i] = assigndNumber[i];
                    }
                }
                else
                {
                    shuffle();
                    for (int i = 0; i < CellNo; i++)
                    {
                        numbers[i] = nos[i];
                        selected[i] = 0;
                    }
                }
            }

            public void mark(int value)
            {
                for (int k = 0; k < selected.Length; k++)
                {
                    if (selected[k] == 0 && numbers[k] == value)
                    {
                        selected[k] = 1;
                        break;
                    }
                }
            }

            public bool marked(int value)
            {
                bool found = false;
                for (int i = 0; i < CellNo; i++)
                {
                    if (selected[i] == 1 && numbers[i] == value)
                    {
                        found = true;
                        break;
                    }
                }
                return found;
            }

            // just show the bingo table,
            public string show()
            {
                string table = bar;
                for (int r = 0; r < side; r++)
                {
                    table += "\n|";
                    for (int c = 0; c < side; c++)
                    {
                        table += $" {numbers[r * side + c],-2} |";
                    }
                    table += $"\n{bar}";
                }
                Console.WriteLine(table);
                return table;
            }

            public void showMarked(char mark = '#')
            {
                Console.WriteLine(bar);
                for (int r = 0; r < side; r++)
                {
                    Console.Write("|");
                    for (int c = 0; c < side; c++)
                    {
                        int index = r * side + c;
                        if (selected[index] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($" {mark,-2} ");
                            Console.ResetColor();
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write($" {numbers[index],-2} |");
                        }
                    }
                    Console.WriteLine($"\n{bar}");
                }
            }
            // show the bingo table with highlight mark,

            void shuffle()
            {
                // Fisher–Yates shuffle (also known as the Knuth shuffle)
                Random rand = new Random();
                int p, val;
                for (int k = nos.Length - 1; k > 0; k--)
                {
                    p = rand.Next(k + 1);
                    //val = nos[k];
                    //nos[k] = nos[p];
                    //nos[p] = val;
                    (nos[k], nos[p]) = (nos[p], nos[k]);  // It works in C#!
                }
            }

            public int countLine()
            {
                int count = 0, temp;

                for (int i = 0; i < side; i++)  // row
                {
                    temp = 1;
                    for (int j = 0; j < side; j++)
                    {
                        if (selected[i * side + j] == 0)
                        {
                            temp = 0;
                            break;
                        }
                    }
                    count += temp;
                }

                for (int i = 0; i < side; i++)  // column
                {
                    temp = 1;
                    for (int j = 0; j < side; j++)
                    {
                        if (selected[i + j * side] == 0)
                        {
                            temp = 0;
                            break;
                        }
                    }
                    count += temp;
                }

                temp = 1;
                for (int i = 0; i < side; i++)  // diagonal
                {
                    if (selected[i * side + i] == 0)
                    {
                        temp = 0;
                        break;
                    }
                }
                count += temp;

                temp = 1;
                for (int i = 0; i < side; i++)  // diagonal
                {
                    if (selected[i * side + (side - 1 - i)] == 0)
                    {
                        temp = 0;
                        break;
                    }
                }
                count += temp;

                return count;
            }
        }
    }
}
