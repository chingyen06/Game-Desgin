namespace Practice2
{
    public partial class Form1 : Form
    {
        Card[] card = new Card[13];

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < card.Length; i++)
            {
                card[i] = new Card(i, this.ClientSize, 改變花色ToolStripMenuItem.Height);
                this.Controls.Add(card[i]);
            }
        }

        private void 改變花色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < card.Length; i++)
            {
                card[i].changeColor();
                card[i].showImage();
            }
        }
    }

    public class Card : PictureBox
    {
        Image image = Properties.Resources.poker;
        private static readonly Random random = new Random();
        int sx, sy, pokerH, pokerW;
        int color, number, posX, posY, dx, dy;
        Size ClientSize;
        bool isPressed;

        public Card(int num, Size ClientSize, int 改變花色Height)
        {
            this.color = 0;
            this.number = num;
            this.isPressed = false;
            this.sy = 改變花色Height;
            this.ClientSize = ClientSize;

            pokerH = image.Height / 5;
            pokerW = image.Width / 13;
            this.Size = new Size(pokerW, pokerH);  // 這一定要記得
            setPosition();
            showImage();

            MouseDown += mouseDown;
            MouseUp += mouseUp;
            MouseMove += mouseMove;
        }

        private void setPosition()
        {
            int maxX = Math.Max(0, ClientSize.Width - pokerW);
            int maxY = Math.Max(sy, ClientSize.Height - pokerH);

            Left = random.Next(maxX);
            Top = random.Next(sy, maxY);
        }

        public void changeColor()
        {
            color = (color + 1) % 4;
        }

        public void showImage()
        {
            Bitmap bitmap = new Bitmap(pokerW, pokerH);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Rectangle destImage = new Rectangle(0, 0, pokerW, pokerH);
                Rectangle sourceImage = new Rectangle(number * pokerW, color * pokerH, pokerW, pokerH);

                g.DrawImage(image, destImage, sourceImage, GraphicsUnit.Pixel);
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
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                dx = e.X - posX;
                dy = e.Y - posY;
                Left += dx;
                Top += dy;
            }
        }
    }
}
