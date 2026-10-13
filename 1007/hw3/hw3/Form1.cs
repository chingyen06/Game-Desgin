namespace hw3
{
    public partial class Form1 : Form
    {
        private Bomb bomb = new Bomb(9, 9, 10);
        private int sx = 15, sy = 80, dx = 1;
        private int row = 9, col = 9, size = 32, second = 0;
        public static int gameStatus, isFacePressed;
        Image[] typeImages, mineImages, faceImages, digitImages;
        private Rectangle faceRect;
        private System.Windows.Forms.Timer gameTimer;

        public Form1()
        {
            InitializeComponent();

            typeImages = new Image[]
            {
                Properties.Resources.type0,
                Properties.Resources.type1,
                Properties.Resources.type2,
                Properties.Resources.type3,
                Properties.Resources.type4,
                Properties.Resources.type5,
                Properties.Resources.type6,
                Properties.Resources.type7,
                Properties.Resources.type8,
                Properties.Resources.closed
            };

            faceImages = new Image[]
            {
                Properties.Resources.face_lose,
                Properties.Resources.face_unpressed,
                Properties.Resources.face_pressed
            };

            digitImages = new Image[]
            {
                Properties.Resources.d0,
                Properties.Resources.d1,
                Properties.Resources.d2,
                Properties.Resources.d3,
                Properties.Resources.d4,
                Properties.Resources.d5,
                Properties.Resources.d6,
                Properties.Resources.d7,
                Properties.Resources.d8,
                Properties.Resources.d9
            };

            int boardWidth = col * size;
            int faceSize = 50;
            faceRect = new Rectangle(sx + (boardWidth - faceSize) / 2, 10, faceSize, faceSize);

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000; // 每 1000 ms (1 秒) 觸發一次
            gameTimer.Tick += GameTimer_Tick;

            gameStatus = 1;
            isFacePressed = 0;
        }

        private void drawDigitalNumber(Graphics g, int num, int startX, int startY)
        {
            int d100 = num / 100;
            int d10 = (num / 10) % 10;
            int d1 = num % 10;

            int digitWidth = 18;
            int digitHeight = 38;

            g.DrawImage(digitImages[d100], new Rectangle(startX, startY, digitWidth, digitHeight));
            g.DrawImage(digitImages[d10], new Rectangle(startX + digitWidth + dx, startY, digitWidth, digitHeight));
            g.DrawImage(digitImages[d1], new Rectangle(startX + (digitWidth + dx) * 2, startY, digitWidth, digitHeight));
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (second < 999)
            {
                second++;
                this.Invalidate(); // 每秒重繪以更新秒數
            }
            else
            {
                second = 0;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // just for test,
            int[,] table = bomb.Mapp;
            int[,] selected = bomb.Selected;
            //----------------------------------------------------------------
            // using sx, sy for moving the table,

            // draw the table first, then draw the numbers in the table
            // Pen pen = new Pen(pen_color, pen_width=1);  // 筆色, 筆的粗細值
            //Pen pen = new Pen(Color.Black, 2);  // 筆色, 粗細值

            //Font font = new Font("consolas", 14);
            //Brush brush = Brushes.Black;

            g.Clear(BackColor);

            int remainFlags = bomb.BombCount - bomb.FlagCount;
            int digitWidth = 18;
            int totalDisplayWidth = digitWidth * 3;

            int leftX = sx;
            int rightX = sx + col * size - totalDisplayWidth;
            int topY = 16;

            drawDigitalNumber(g, remainFlags, leftX, topY);
            drawDigitalNumber(g, second, rightX, topY);

            if (isFacePressed == 1)
            {
                g.DrawImage(faceImages[2], faceRect);
            }
            else
            {
                g.DrawImage(faceImages[gameStatus], faceRect);
            }

            for (int r = 1; r <= row; r++)
            {
                for (int c = 1; c <= col; c++)
                {
                    int val = table[r, c];
                    int x = sx + (c - 1) * size;
                    int y = sy + (r - 1) * size;

                    if (selected[r, c] == 1)
                    {
                        if (val == -1)
                        {
                            Image bombImage = Properties.Resources.mine;

                            g.DrawImage(bombImage, new Rectangle(x, y, size, size));
                        }
                        else if (val == -2)
                        {
                            Image bombRedImage = Properties.Resources.mine_red;
                            g.DrawImage(bombRedImage, new Rectangle(x, y, size, size));
                        }
                        else if (val == -3)
                        {
                            Image flagImage = Properties.Resources.flag;
                            g.DrawImage(flagImage, new Rectangle(x, y, size, size));
                        }
                        else
                        {
                            g.DrawImage(typeImages[val], new Rectangle(x, y, size, size));
                        }
                    }
                    else if (selected[r, c] == -1)
                    {
                        Image flagImage = Properties.Resources.flag;
                        g.DrawImage(flagImage, new Rectangle(x, y, size, size));
                    }
                    else
                    {
                        g.DrawImage(typeImages[9], new Rectangle(x, y, size, size));
                    }
                }
            }
        }

        public void restart()
        {
            gameTimer.Stop();

            gameStatus = 1;
            second = 0;
            bomb.FlagCount = 0;

            bomb.reset();
            bomb.setBomb();
            bomb.countBombs();
        }

        public class Bomb
        {
            private int rows, cols, bombCount, flagCount;
            private int[,] mapp;
            private int[,] selected;

            public int[,] Mapp
            {
                get { return mapp; }
                set { mapp = value; }
            }

            public int[,] Selected
            {
                get { return selected; }
                set { selected = value; }
            }

            public int FlagCount
            {
                get { return flagCount; }
                set { flagCount = value; }
            }

            public int BombCount
            {
                get { return bombCount; }
                set { bombCount = value; }
            }

            public Bomb(int rows = 9, int cols = 9, int bombCount = 10)
            {
                this.rows = rows;
                this.cols = cols;
                this.bombCount = bombCount;
                this.flagCount = 0;
                this.mapp = new int[rows + 2, cols + 2];
                this.selected = new int[rows + 2, cols + 2];

                reset();
                setBomb();
                countBombs();
            }

            public void reset()
            {
                for (int i = 0; i < rows + 2; i++)
                {
                    for (int j = 0; j < cols + 2; j++)
                    {
                        selected[i, j] = 0;
                    }
                }

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

            public void click(int r, int c)
            {
                if (selected[r, c] == -1)
                    return;

                if (mapp[r, c] == -1)
                {
                    for (int i = 1; i <= rows; i++)
                    {
                        for (int j = 1; j <= cols; j++)
                        {
                            if (selected[i, j] == -1)
                                flagCount--;

                            selected[i, j] = 1;
                        }
                    }
                    ;
                    mapp[r, c] = -2;
                    Form1.gameStatus = 0;
                }
                else if (mapp[r, c] == 0)
                {
                    findEmpty(r, c);
                }
                else
                {
                    selected[r, c] = 1;
                }
            }

            public void rightClick(int r, int c)
            {
                if (selected[r, c] == 0 && flagCount < bombCount)
                {
                    selected[r, c] = -1;
                    flagCount++;
                }
                else if (selected[r, c] == -1)
                {
                    selected[r, c] = 0;
                    flagCount--;
                }
            }

            private void findEmpty(int r, int c)
            {
                if (r < 1 || r > rows || c < 1 || c > cols)
                    return;

                if (selected[r, c] == 1)
                    return;

                if (selected[r, c] == -1)
                    flagCount--;

                selected[r, c] = 1;

                if (mapp[r, c] == 0)
                {
                    findEmpty(r - 1, c);
                    findEmpty(r + 1, c);
                    findEmpty(r, c - 1);
                    findEmpty(r, c + 1);
                }
            }
        }

        private void Form1_MouseClick_1(object sender, MouseEventArgs e)
        {
            int r = (e.Y - sy) / size + 1, c = (e.X - sx) / size + 1;

            if (gameStatus == 0)
                return;

            if (r < 1 || r > row || c < 1 || c > col)
                return;

            gameTimer.Start();

            if (e.Button == MouseButtons.Left)
            {
                bomb.click(r, c);
            }
            else if (e.Button == MouseButtons.Right)
            {
                bomb.rightClick(r, c);
            }

            this.Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && faceRect.Contains(e.Location))
            {
                isFacePressed = 1;
                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isFacePressed == 1)
            {
                restart();

                this.Invalidate();

                isFacePressed = 0;
            }
        }
    }
}
