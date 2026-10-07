namespace hw4
{
    public partial class Form1 : Form
    {
        private Image pokerSheet = Properties.Resources.poker;
        private string studentID = "113820032";
        private int[] suitRows;

        public Form1()
        {
            InitializeComponent();

            Random random = new Random();
            suitRows = new int[studentID.Length];
            for (int i = 0; i < studentID.Length; i++)
            {
                suitRows[i] = random.Next(0, 4);
            }

            // 註冊 Paint 事件
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int ds = 10;
            int sx = ds;
            int sy = label1.Location.Y + label1.Height + ds;
            int pokerW = pokerSheet.Width / 13;
            int pokerH = pokerSheet.Height / 5;  // 包含 撲克牌的背面圖案，所以總共是 5 列。

            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(BackColor);
                // Draw the whole image

                // Draw a part of the image
                for (int i = 0; i < studentID.Length; i++)
                {
                    int c = studentID[i] - '1';
                    int r = suitRows[i];

                    if (studentID[i] == '0')
                    {
                        c = 9;
                    }
                    else if (studentID[i] == 'A')
                    {
                        c = 10;
                    }
                    else if (studentID[i] == 'B')
                    {
                        c = 11;
                    }

                    Rectangle destRect = new Rectangle(sx + (i * pokerW) + ds, sy, pokerW, pokerH); // Where to draw it
                    Rectangle sourceRect = new Rectangle(c * pokerW, r * pokerH, pokerW, pokerH); // The part of the image to draw

                    g.DrawImage(pokerSheet, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }
        }
    }
}
