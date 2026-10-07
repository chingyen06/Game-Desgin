namespace hw2
{
    public partial class Form1 : Form
    {
        private int show_count = 0;
        private Bomb bomb = new Bomb(9, 9, 10);

        public Form1()
        {
            InitializeComponent();
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
                        }
                        ;
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
                    if (mapp[r + dr8[i], c + dc8[i]] == -1)
                    {
                        mapp[r, c]++;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // just for test,
            int[,] table = bomb.Mapp;
            //----------------------------------------------------------------
            // using sx, sy for moving the table,
            int sx = 15;
            int sy = 50;
            int dx = 32; // 每一直行的間距
            int dy = 32; // 每一橫列的間距
            int row = 9, col = 9, size = 32;

            // draw the table first, then draw the numbers in the table
            // Pen pen = new Pen(pen_color, pen_width=1);  // 筆色, 筆的粗細值
            //Pen pen = new Pen(Color.Black, 2);  // 筆色, 粗細值

            //Font font = new Font("consolas", 14);
            //Brush brush = Brushes.Black;


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

            Image[] typeImages =
            {
                Properties.Resources.type0,
                Properties.Resources.type1,
                Properties.Resources.type2,
                Properties.Resources.type3,
                Properties.Resources.type4,
                Properties.Resources.type5,
                Properties.Resources.type6,
                Properties.Resources.type7,
                Properties.Resources.type8
            };

            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(BackColor);

                for (int r = 1; r <= row; r++)
                {
                    for (int c = 1; c <= col; c++)
                    {
                        int val = table[r, c];
                        int x = sx + (c - 1) * dx;
                        int y = sy + (r - 1) * dy;

                        if (val == -1)
                        {
                            Image bombImage = Properties.Resources.mine;

                            g.DrawImage(bombImage, new Rectangle(x, y, size, size));
                        }
                        else
                        {
                            g.DrawImage(typeImages[val], new Rectangle(x, y, size, size));
                        }
                    }
                }
            }

            if (show_count == 3)
            {
                show_count = 0;
            }
        }
    }
}
