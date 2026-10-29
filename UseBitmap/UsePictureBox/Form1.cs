namespace UsePictureBox
{


    public partial class Form1 : Form
    {
        Bitmap[] bmps = {
            Properties.Resources.alarmclock256,
            Properties.Resources.Conan_4,
            Properties.Resources.CuteBaby,
            Properties.Resources.doll
        };
        int bmpIndex = 0;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = bmps[bmpIndex];
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void changeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bmpIndex = ++bmpIndex % bmps.Length;
            pictureBox1.Image = bmps[bmpIndex];
        }

        //-------------------------------------------------------------------------------------------
        // 實作滑鼠按下事件:
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Color ptColor = Color.Red;

            if (pictureBox1.Image != null)
            {
                if (e.Button == MouseButtons.Left)  // 按滑鼠左鍵，取得顏色。
                {
                    Color color = ((Bitmap)pictureBox1.Image).GetPixel(e.X, e.Y);
                    //Color color = (pictureBox1.Image as Bitmap).GetPixel(e.X, e.Y);

                    label1.Text = $"({e.X}, {e.Y}) => {color.ToString()}";
                }
                else if (e.Button == MouseButtons.Right) // 按滑鼠右鍵，填上ptColor的顏色。
                {
                    (pictureBox1.Image as Bitmap).SetPixel(e.X, e.Y, ptColor);
                    pictureBox1.Invalidate();
                }
            }
        }

        bool dragging = false;
        int prevX, prevY;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            prevX = e.X;
            prevY = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                int dx = e.X - prevX;
                int dy = e.Y - prevY;
                (sender as PictureBox).Left += dx;
                ((PictureBox)sender).Top  += dy;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
