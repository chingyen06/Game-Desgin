namespace _2
{
    public partial class Form1 : Form
    {
        Image image = Properties.Resources.poker;
        int posx, posy, pokerW, pokerH;
        int n = -1;
        bool isPressed = false;

        public Form1()
        {
            InitializeComponent();

            pokerW = image.Width / 13;
            pokerH = image.Height / 5;

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (!isPressed)
                return;

            Rectangle destImage = new Rectangle(posx - pokerW / 2, posy - pokerH / 2, pokerW, pokerH);
            Rectangle sourceImage = new Rectangle(n * pokerW, 2 * pokerH, pokerW, pokerH);

            g.DrawImage(image, destImage, sourceImage, GraphicsUnit.Pixel);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                posx = e.X;
                posy = e.Y;

                n = (n + 1) % 13;

                this.Invalidate();
                this.Update();
            }
        }
    }
}
