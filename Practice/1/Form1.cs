namespace _1
{
    public partial class Form1 : Form
    {
        static int size = 7;
        int[,] sol = new int[size+2, size+2];

        public Form1()
        {
            InitializeComponent();

            count_num();

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }

        private void count_num()
        {
            int total = size * size, n = 1;
            int pos = 0, y = 1, x = 1;
            int[,] dir = { {0, 1}, {1, 0}, {0, -1}, {-1, 0} };

            for (int i = 0; i < size+2; i++)
            {
                for (int j = 0; j< size+2; j++)
                {
                    if (i == 0 || j == 0 || i == size +1 || j == size + 1)
                    {
                        sol[i, j] = -1;
                    }
                    else
                    {
                        sol[i, j] = 0;
                    }
                }
            }

            while (n <= total)
            {
                sol[y, x] = n;

                if (sol[y + dir[pos, 0], x + dir[pos, 1]] != 0)
                {
                    pos = (pos + 1) % 4;
                }

                y = y + dir[pos, 0];
                x = x + dir[pos, 1];

                n++;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            string s;
            int posx, posy, sx = 15, sy = 15;
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            Brush brush = Brushes.Black;
            // Brush brush = new SolidBrush(Color.Black);
            Font font = new Font("Consolas", 12);

            for (int i = 1; i <= size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    s = $"{sol[i, j],2}";

                    g.DrawString(s, font, brush, sx + (j-1) * 25, sy + (i-1) * 25);
                }
            }
        }
    }
}
