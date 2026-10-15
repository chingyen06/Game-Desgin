namespace hw1
{
    public partial class Form1 : Form
    {
        int color = 0;
        Card[] card = new Card[13];

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 13; i++)
            {
                card[i] = new Card(color, i, this.ClientSize, 改變花色ToolStripMenuItem.Height);
                Controls.Add(card[i]);
            }

            //this.DoubleBuffered = true;
        }

        private void 改變花色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            color = (color + 1) % 4;

            for (int i = 0; i < 13; i++)
            {
                card[i].setIndex(color, i);
                card[i].showImage();
            }
        }

    }

    class Card : PictureBox
    {
        private static readonly Random random = new Random();
        Image image = Properties.Resources.poker;
        Size clientSize;
        int r, c, mouseX, mouseY, dx, dy, sy;
        int pokerW, pokerH;
        bool isPressed;
        public Card(int r, int c, Size clientSize, int 改變花色Height)
        {
            this.r = r;
            this.c = c;
            this.clientSize = clientSize;
            this.sy = 改變花色Height;

            isPressed = false;
            pokerW = image.Width / 13;
            pokerH = image.Height / 5;
            SizeMode = PictureBoxSizeMode.AutoSize;

            setPosition();
            showImage();

            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        public void setIndex(int r, int c)
        {
            this.r = r;
            this.c = c;
        }

        private void setPosition()
        {
            int maxX = Math.Max(0, clientSize.Width - pokerW);
            int maxY = Math.Max(sy, clientSize.Height - pokerH);

            Left = random.Next(maxX);
            Top = random.Next(sy, maxY);
        }

        public void showImage()
        {
            Bitmap bitmap = new Bitmap(pokerW, pokerH);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Rectangle destImage = new Rectangle(0, 0, pokerW, pokerH);
                Rectangle sourceImage = new Rectangle(c * pokerW, r * pokerH, pokerW, pokerH);

                g.DrawImage(image, destImage, sourceImage, GraphicsUnit.Pixel);
            }

            Image = bitmap;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            mouseX = e.X;
            mouseY = e.Y;

            BringToFront();
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                dx = e.X - mouseX;
                dy = e.Y - mouseY;

                Left += dx;
                Top += dy;
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }
    }
}
