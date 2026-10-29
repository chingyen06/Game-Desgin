using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace hw2
{
    
    public partial class Form1 : Form
    {
        public static readonly int sx = 15, sy = 15;
        public static readonly System.Drawing.Image image = Properties.Resources.doll;
        Puzzle[] puzzles = new Puzzle[16];

        public Form1()
        {
            InitializeComponent();

            this.Paint += Form1_Paint;


            for (int i = 0; i < 16; i++)
            {
                puzzles[i] = new Puzzle(i / 4, i % 4, this.ClientSize);
                Controls.Add(puzzles[i]);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int w = image.Width / 4;
            int h = image.Height / 4;

            int totalWidth = w * 4;
            int totalHeight = h * 4;

            using (Pen pen = new Pen(Color.Green, 1))
            {
                for (int i = 0; i <= 4; i++)
                {
                    int y = sy + i * h;
                    g.DrawLine(pen, sx, y, sx + totalWidth, y);
                }

                for (int i = 0; i <= 4; i++)
                {
                    int x = sx + i * w;
                    g.DrawLine(pen, x, sy, x, sy + totalHeight);
                }
            }
        }
    }

    public class Puzzle : PictureBox
    {
        private static readonly Random random = new Random();
        public bool isPressed, isDone;
        int row, col, posX, posY, imageW, imageH;
        int dx, dy, closedX, closedY, howClosed;
        Size clientSize;

        public Puzzle(int r, int c, Size clientSize, int howClosed = 15)
        {
            this.row = r;
            this.col = c;
            this.clientSize = clientSize;
            this.howClosed = 15;

            this.imageW = Form1.image.Width / 4;
            this.imageH = Form1.image.Height / 4;
            this.isPressed = false;
            this.closedX = Form1.sx + this.col * Form1.image.Width / 4;
            this.closedY = Form1.sy + this.row * Form1.image.Height / 4;
            this.isDone = false;
            SizeMode = PictureBoxSizeMode.AutoSize;

            shuffle();
            showImage();

            MouseDown += mouseDown;
            MouseUp += mouseUp;
            MouseMove += mouseMove;
        }

        private void shuffle()
        {
            Left = random.Next(0, clientSize.Width - imageW);
            Top = random.Next(0, clientSize.Height - imageH);
        }

        private void checkClosed()
        {
            int realX, realY;

            realX = Form1.sx + col * imageW;
            realY = Form1.sy + row * imageH;

            if (Math.Abs(Left - realX) <= howClosed && Math.Abs(Top - realY) <= howClosed)
            {
                Left = realX;
                Top = realY;

                isDone = true;
            }
        }

        public void showImage()
        {
            Bitmap bitmap = new Bitmap(imageW, imageH);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Rectangle destImage = new Rectangle(0, 0, imageW, imageH);
                Rectangle sourceImage = new Rectangle(col * imageW, row * imageH, imageW, imageH);

                g.DrawImage(Form1.image, destImage, sourceImage, GraphicsUnit.Pixel);
            }

            Image = bitmap;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                posX = e.X;
                posY = e.Y;

                BringToFront();
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;

            checkClosed();
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed && !isDone)
            {
                dx = e.X - posX;
                dy = e.Y - posY;

                Left += dx;
                Top += dy;
            }
        }
    }
}
