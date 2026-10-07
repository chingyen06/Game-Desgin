using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hw3
{
    public partial class Form1 : Form
    {
        private Bingo bingo = new Bingo();
        int sx = 15;
        int sy = 50;
        int side = 40, row = 5, col = 5;
        int showBingo = 0;

        public Form1()
        {
            InitializeComponent();

            this.MouseClick += Form1_MouseClick;
            this.Paint += Form1_Paint;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bingo.reset();

            showBingo = 1;

            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (showBingo == 0) return;
            Graphics g = e.Graphics;

            // just for test,
            int[] table = bingo.Numbers;
            int[] select = bingo.Selected;
            //----------------------------------------------------------------
            // using sx, sy for moving the table,

            // draw the table first, then draw the numbers in the table
            // Pen pen = new Pen(pen_color, pen_width=1);  // 筆色, 筆的粗細值

            using (Pen pen = new Pen(Color.Black, 2))  // 筆色, 粗細值
            {
                g.Clear(BackColor);   // 清除 g 所指向的整個畫面。

                //g.FillRectangle(Brushes.LightGray, sx, sy, side * col, side * row);  // 填滿矩形區域。

                // horizontal lines:
                for (int r = 0; r <= row; r++)
                {
                    // g.DrawLine(pen, p1_x, p1_y, p2_x, p2_y);
                    g.DrawLine(pen, sx, sy + r * side, sx + side * col, sy + r * side);
                }

                // vertical lines:
                for (int c = 0; c <= col; c++)
                {
                    g.DrawLine(pen, sx + c * side, sy, sx + c * side, sy + row * side);
                }
            }

            string msg;

            using (Font font = new Font("consolas", 14))
            {
                Brush brush = Brushes.Brown;

                int index = 0;
                for (int r = 0; r < row; r++)
                {
                    for (int c = 0; c < col; c++)
                    {
                        if (select[index] == 1)
                        {
                            msg = "#";
                            index++;
                        }
                        else
                        {
                            msg = $"{table[index++]}";  // 取得訊息字串。
                        }
                        SizeF size = g.MeasureString(msg, font);  // 計算即將繪出的字串的寬與高。
                        float width = size.Width;   // 字串寬度。
                        float height = size.Height; // 字串高度。

                        // 置中設定:
                        g.DrawString(msg, font, brush,
                                     sx + c * side + (side - width) / 2,
                                     sy + r * side + (side - height) / 2);
                    }
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (showBingo == 0) return;

            if (e.X < sx || e.Y < sy) return;

            int c = (e.X - sx) / side;
            int r = (e.Y - sy) / side;

            if (c >= 0 && c < col && r >= 0 && r < row)
            {
                int index = r * col + c;
                bingo.Selected[index] = 1;

                this.Invalidate();
            }
        }

        public class Bingo
        {
            private int side = 5;
            private int CellNo;
            int[] nos;
            int[] numbers;
            int[] selected;
            int[] num_count;
            string bar = "";
            int index;

            public int[] Numbers
            {
                get { return numbers; }
                set { numbers = value; }  // => obj.numbers = value;
            }

            public int[] Selected
            {
                get { return selected; }
                set { selected = value; }  // => obj.selected = value;
            }

            public Bingo(int side = 5)
            {
                this.side = side;
                CellNo = side * side;

                nos = new int[CellNo];
                numbers = new int[CellNo];
                selected = new int[CellNo];
                num_count = new int[CellNo];

                for (int k = 0; k < CellNo; k++)
                {
                    nos[k] = k + 1;
                }
                shuffle();

                //bar = "+";
                //for (int s = 0; s < side; s++)
                //{
                //    bar += "----+";
                //}

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
            //public string show()
            //{
            //    string table = bar;
            //    for (int r = 0; r < side; r++)
            //    {
            //        table += "\n|";
            //        for (int c = 0; c < side; c++)
            //        {
            //            table += $" {numbers[r * side + c],-2} |";
            //        }
            //        table += $"\n{bar}";
            //    }
            //    Console.WriteLine(table);
            //    Console.WriteLine("From Player:");
            //    Console.WriteLine();
            //    return table;
            //}

            //public void showMarked(char mark = '#')
            //{
            //    int computer = -1;

            //    if (countLine() < 3)
            //    {
            //        computer = compute();
            //        countLine();
            //    }

            //    Console.WriteLine($"{bar}    {bar}");
            //    for (int r = 0; r < side; r++)
            //    {
            //        Console.Write("|");
            //        for (int c = 0; c < side; c++)
            //        {
            //            int index = r * side + c;
            //            if (selected[index] == 1)
            //            {
            //                Console.ForegroundColor = ConsoleColor.Yellow;
            //                Console.Write($" {mark,-2} ");
            //                Console.ResetColor();
            //                Console.Write("|");
            //            }
            //            else
            //            {
            //                Console.Write($" {numbers[index],-2} |");
            //            }
            //        }

            //        Console.Write("    |");
            //        for (int c = 0; c < side; c++)
            //        {
            //            int index = r * side + c;

            //            Console.Write($" {num_count[index],-2} |");
            //        }
            //        Console.WriteLine($"\n{bar}    {bar}");
            //    }

            //    if (computer == -1 && countLine() >= 3)
            //    {
            //        Console.WriteLine("You win!");
            //        Console.WriteLine($"lines: {countLine()}");
            //        return;
            //    }

            //    if (countLine() >= 3)
            //    {
            //        Console.WriteLine("Computer wins!");
            //        Console.WriteLine($"Computer selected {computer}.");
            //        Console.WriteLine($"lines: {countLine()}");
            //        return;
            //    }

            //    Console.WriteLine($"Computer selected {computer}.");
            //    Console.WriteLine($"lines: {countLine()}");
            //    Console.WriteLine("From Player:");
            //    Console.WriteLine();
            //}
            //// show the bingo table with highlight mark,

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

            //public int countLine()
            //{
            //    int count = 0, temp;

            //    for (int i = 0; i < CellNo; i++)
            //    {
            //        num_count[i] = 0;
            //    }

            //    for (int i = 0; i < side; i++)  // row
            //    {
            //        temp = 0;

            //        for (int j = 0; j < side; j++)
            //        {
            //            if (selected[i * side + j] == 1)
            //            {
            //                temp++;
            //            }
            //        }

            //        for (int j = 0; j < side; j++)
            //        {
            //            if (selected[i * side + j] == 1)
            //            {
            //                num_count[i * side + j] = 0;
            //            }
            //            else
            //            {
            //                num_count[i * side + j] += temp;
            //            }
            //        }

            //        if (temp == side)
            //        {
            //            count++;
            //        }
            //    }

            //    for (int i = 0; i < side; i++)  // column
            //    {
            //        temp = 0;

            //        for (int j = 0; j < side; j++)
            //        {
            //            if (selected[i + j * side] == 1)
            //            {
            //                temp++;
            //            }
            //        }

            //        for (int j = 0; j < side; j++)
            //        {
            //            if (selected[i + j * side] == 1)
            //            {
            //                num_count[i + j * side] = 0;
            //            }
            //            else
            //            {
            //                num_count[i + j * side] += temp;
            //            }
            //        }

            //        if (temp == side)
            //        {
            //            count++;
            //        }
            //    }

            //    temp = 0;
            //    for (int i = 0; i < side; i++)  // diagonal
            //    {
            //        if (selected[i * side + i] == 1)
            //        {
            //            temp++;
            //        }
            //    }
            //    for (int i = 0; i < side; i++)
            //    {
            //        if (selected[i * side + i] == 1)
            //        {
            //            num_count[i * side + i] = 0;
            //        }
            //        else
            //        {
            //            num_count[i * side + i] += temp;
            //        }
            //    }
            //    if (temp == side)
            //    {
            //        count++;
            //    }

            //    temp = 0;
            //    for (int i = 0; i < side; i++)  // diagonal
            //    {
            //        if (selected[i * side + (side - 1 - i)] == 1)
            //        {
            //            temp++;
            //        }
            //    }
            //    for (int i = 0; i < side; i++)
            //    {
            //        if (selected[i * side + (side - 1 - i)] == 1)
            //        {
            //            num_count[i * side + (side - 1 - i)] = 0;
            //        }
            //        else
            //        {
            //            num_count[i * side + (side - 1 - i)] += temp;
            //        }
            //    }
            //    if (temp == side)
            //    {
            //        count++;
            //    }

            //    return count;
            //}

            //private int compute()
            //{
            //    int max_index = -1;

            //    for (int i = 0; i < CellNo; i++)
            //    {
            //        if (selected[i] == 1)
            //            continue;

            //        if (max_index == -1 || num_count[i] > num_count[max_index])
            //        {
            //            max_index = i;
            //        }
            //    }

            //    selected[max_index] = 1;

            //    return numbers[max_index];
            //}
        }
    }
}
