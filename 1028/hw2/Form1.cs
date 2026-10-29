namespace hw2
{
    public partial class Form1 : Form
    {
        private static readonly Random random = new Random();
        int n = 20, length = 60, width = 80, w, h;
        int[,] pos = new int[20, 2];
        bool isPressed;

        public Form1()
        {
            InitializeComponent();

            w = 800;
            h = 600;
            isPressed = false;
            this.ClientSize = new Size(w, h);

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!isPressed)
                return;

            Graphics g = e.Graphics;

            g.Clear(BackColor);

            using (Pen pen = new Pen(Color.Black))
            {
                for (int i = 0; i < n; i++)
                {
                    g.DrawRectangle(pen, pos[i, 0], pos[i, 1], width, length);
                }
            }
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool ok;
            int i = 0, posX, posY;

            while (i < n)
            {
                posX = random.Next(0, w - 2 * width);
                posY = random.Next(rectangleToolStripMenuItem.Height + 10, h - 2 * length);

                ok = true;

                for (int j = i - 1; j >= 0; j--)
                {
                    if (!((posX + width) < pos[j, 0] || posX > (pos[j, 0] + width) || (posY + length) < pos[j, 1] || posY > (pos[j, 1] + length)))
                    {
                        ok = false;
                    }
                }

                if (ok)
                {
                    pos[i, 0] = posX;
                    pos[i, 1] = posY;

                    i++;
                }
            }

            isPressed = true;
            this.Invalidate();
        }
    }
}
