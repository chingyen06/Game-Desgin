using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hw1
{
    public partial class Form1 : Form
    {
        private int show_count = 0;
        private Bomb bomb = new Bomb(9, 9, 10);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // just for test,
            int[,] table = bomb.Mapp;
            //----------------------------------------------------------------
            // using sx, sy for moving the table,
            int sx = 30;
            int sy = 50;
            int dx = 30; // 每一直行的間距
            int dy = 25; // 每一橫列的間距
            int side = 20, row = 9, col = 9;

            // draw the table first, then draw the numbers in the table
            // Pen pen = new Pen(pen_color, pen_width=1);  // 筆色, 筆的粗細值
            Pen pen = new Pen(Color.Black, 2);  // 筆色, 粗細值

            Font font = new Font("consolas", 14);
            Brush brush = Brushes.Black;

            show_count++;

            if (show_count == 1)
            {
                bomb.reset();
            }
            else if (show_count == 2)
            {
                bomb.setBomb();
            }
            else if (show_count == 3)
            {
                bomb.countBombs();
            }

            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                g.Clear(BackColor);

                int index = 0;

                float digitWidth = g.MeasureString("0", font).Width;
                float minusWidth = g.MeasureString("-", font).Width;

                for (int r = 1; r <= row; r++)
                {
                    for (int c = 1; c <= col; c++)
                    {
                        int val = table[r, c];
                        float centerX = sx + (c - 1) * dx;
                        float y = sy + (r - 1) * dy;

                        if (val == -1)
                        {
                            if (show_count == 3)
                            {
                                float digitX = centerX - digitWidth / 2.0f;
                                g.DrawString("@", font, brush, digitX, y);
                            }
                            else
                            {
                                float digitX = centerX - digitWidth / 2.0f;
                                g.DrawString("1", font, brush, digitX, y);

                                float minusX = digitX - minusWidth + 4;
                                g.DrawString("-", font, brush, minusX, y);
                            }
                        }
                        else
                        {
                            string msg = val.ToString();
                            float digitX = centerX - digitWidth / 2.0f;
                            g.DrawString(msg, font, brush, digitX, y);
                        }
                    }
                }
            }

            if (show_count == 3)
            {
                show_count = 0;
            }
        }

        public class Bomb
        {
            private int rows, cols, bombCount;
            private int[,] mapp;

            public int[,] Mapp
            {
                get { return mapp; }
                set { mapp = value; }
            }

            public Bomb(int rows = 9, int cols = 9, int bombCount = 10)
            {
                this.rows = rows;
                this.cols = cols;
                this.bombCount = bombCount;
                this.mapp = new int[rows + 2, cols + 2];
            }

            public void reset()
            {
                for (int r = 0; r <= rows + 1; r++)
                {
                    for (int c = 0; c <= cols + 1; c++)
                    {
                        mapp[r, c] = 0;
                    }
                }
            }

            public void setBomb()
            {
                Random rand = new Random();

                int nums = 0, r = rand.Next(1, rows + 1), c = rand.Next(1, cols + 1);

                while (nums < bombCount)
                {
                    if (mapp[r, c] != -1)
                    {
                        mapp[r, c] = -1;
                        nums++;
                    }

                    r = rand.Next(1, rows + 1);
                    c = rand.Next(1, cols + 1);
                }
            }

            public void countBombs()
            {
                for (int r = 1; r <= rows; r++)
                {
                    for (int c = 1; c <= cols; c++)
                    {
                        countNeighbors(r, c);
                        if (mapp[r, c] == -1)
                        {
                            mapp[r, c] = -1;
                        };
                    }
                }
            }

            private void countNeighbors(int r, int c)
            {
                int[] dr8 = { -1, -1, -1, 0, 0, 1, 1, 1 };
                int[] dc8 = { -1, 0, 1, -1, 1, -1, 0, 1 };

                if (mapp[r, c] == -1)
                {
                    return;
                }

            for (int i = 0; i < 8; i++)
                {
                    if (mapp[r + dc8[i], c + dr8[i]] == -1)
                    {
                        mapp[r, c]++;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
