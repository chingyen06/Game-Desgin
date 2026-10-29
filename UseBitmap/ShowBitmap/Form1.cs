namespace ShowBitmap
{
    public partial class Form1 : Form
    {
        int sx = 10;
        int sy = 10;

        Bitmap[] bmps = {
            Properties.Resources.alarmclock256,
            Properties.Resources.Conan_4,
            Properties.Resources.doll
        };
        int bmpIndex = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void showBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = bmps[bmpIndex];

            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(BackColor);
                g.DrawImage(bmp, sx, sy + menuStrip1.Height, new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            }

            bmpIndex = ++bmpIndex % bmps.Length;
        }
    }
}
