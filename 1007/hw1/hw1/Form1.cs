namespace hw1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw some shapes on the bitmap for testing
            g.Clear(Color.White);

            using (Pen pen = new Pen(Color.Blue, 2))
            {
                float left = 200, top = 20;
                float diameter = 100;
                float radius = diameter / 2;
                float deltaY = (float)(Math.Sqrt(3) * radius);
                float startX, currentX, currentY;

                for (int i = 0; i <= 3; i++)
                {
                    startX = left - (i * radius);
                    currentY = top + (i * deltaY);

                    //g.DrawEllipse(Pens.Blue, left, top, width, height);
                    for (int j = 0; j <= i; j++)
                    {
                        currentX = startX + (j * diameter);
                        g.DrawEllipse(pen, currentX, currentY, diameter, diameter);
                    }
                }
            }
        }

    }
}
