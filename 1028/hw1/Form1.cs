namespace hw1
{
    public partial class Form1 : Form
    {
        private static readonly Random random = new Random();
        int n = 20, r = 50, w, h;
        int[,] pos = new int[20, 4];
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

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool ok;
            int i = 0, posX, posY, oX, oY;

            while (i < n) {
                posX = random.Next(0, w - 2 * r);
                posY = random.Next(checkToolStripMenuItem.Height + 10, h - 2 * r);
                oX = posX + r;
                oY = posY + r;

                ok = true;

                for (int j = i - 1; j >= 0; j--)
                {
                    if ((pos[j,2]-oX) * (pos[j,2]-oX) + (pos[j,3]-oY) * (pos[j,3]-oY) < (2*r) * (2*r))
                    {
                        ok = false;
                    }
                }

                if (ok)
                {
                    pos[i, 0] = posX;
                    pos[i, 1] = posY;
                    pos[i, 2] = oX;
                    pos[i, 3] = oY;

                    i++;
                }
            }

            isPressed = true;
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!isPressed)
                return;

            Graphics g = e.Graphics;

            g.Clear(BackColor);

            using (Pen pen = new Pen(Color.Black))
            {
                for (int i = 0; i < n; i++) {
                    g.DrawEllipse(pen, pos[i,0], pos[i, 1], 2 * r, 2 * r);
                }
            }
        }
    }
}
