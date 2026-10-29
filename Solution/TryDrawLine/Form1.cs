namespace TryDrawLine
{
    public partial class Form1 : Form
    {
        Bitmap bitmap = Properties.Resources.sunset_with_snoopy;
        Bitmap bgBmp;

        public Form1()
        {
            InitializeComponent();

            bgBmp = new Bitmap(panel1.Width, panel1.Height);

            resetPanel();
        }

        void resetPanel()
        {
            Random rand = new Random();
            int side = 100;

            using (Graphics g = Graphics.FromImage(bgBmp))
            {
                g.Clear(panel1.BackColor);

                for (int k = 0; k < 10; k++)
                {
                    int px = rand.Next(10, panel1.Width - side);
                    int py = rand.Next(menuStrip1.Height + 10, panel1.Height - side);
                    if (k % 2 == 0)
                    {
                        g.DrawEllipse(Pens.Black, px, py, side, side);
                    }
                    else
                    {
                        g.DrawRectangle(Pens.Black, px, py, side, side);
                    }
                }
            }
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x0 = 0;
            int y0 = 0;
            int x1 = 300;
            int y1 = 300;

            for (int x = 0; x < 100; x++)
            {
                /* usage 1: with color,
                drawLine(this, Color.Red, x0 + x, y0, x1 + x, y1);
                drawLine(panel1, Color.Blue, x0 + x, y0, x1 + x, y1);
                //*/

                //* usage 2: with bitmap,
                drawLine(this, bitmap, x0 + x, y0, x1 + x, y1);
                drawLine(panel1, bitmap, x0 + x, y0, x1 + x, y1);
                //*/
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetPanel();

            using (Graphics g = panel1.CreateGraphics())
            {
                g.DrawImage(bgBmp, 0, 0);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //FloodFillStackQueue(Bitmap bmp, Point startPoint, Color fillColor, Color targetColor)
            FloodFillStackQueue(bgBmp, new Point(e.X, e.Y), Color.Red, Color.Black);
            using (Graphics g = panel1.CreateGraphics())
            {
                g.DrawImage(bgBmp, 0, 0);
            }
        }

        //--------------------------------------------------------------------------------
        /// 使用 Bresenham 演算法計算兩點之間的直線像素座標
        /// 起點 X 座標
        /// 起點 Y 座標
        /// 終點 X 座標
        /// 終點 Y 座標
        /// 組成直線的所有座標點清單

        // draw a line with a specified color.
        public void drawLine(Control control, Color color, int x0, int y0, int x1, int y1)
        {
            List<Point> points = new List<Point>();

            Brush brush = new SolidBrush(color);

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            // 決定前進的方向（正向或負向）
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                // 記錄當前點
                points.Add(new Point(x0, y0));

                // 如果已經到達終點，則結束
                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            // draw the line:
            using (Graphics g = control.CreateGraphics())
            {
                foreach (Point p in points)
                {
                    g.FillRectangle(brush, p.X, p.Y, 1, 1);
                }
            }
        }

        // draw a line using a bitmap content.
        public void drawLine(Control control, Bitmap bmp, int x0, int y0, int x1, int y1)
        {
            List<Point> points = new List<Point>();

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            // 決定前進的方向（正向或負向）
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                // 記錄當前點
                points.Add(new Point(x0, y0));

                // 如果已經到達終點，則結束
                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            // draw the line:
            using (Graphics g = control.CreateGraphics())
            {
                foreach (Point pt in points)
                {
                    int bx = pt.X % bmp.Width;
                    int by = pt.Y % bmp.Height;

                    g.FillRectangle(new SolidBrush(bmp.GetPixel(bx, by)), pt.X, pt.Y, 1, 1);
                }
            }
        }

        // 實現在 bitmap 上，對以 targetColor 圍住區域進行填色的函式:
        public void FloodFillStackQueue(Bitmap bmp, Point startPoint, Color fillColor, Color targetColor)
        {
            // 1. 取得起始點的原始顏色
            //Color targetColor = bmp.GetPixel(startPoint.X, startPoint.Y);

            // 2. 如果新舊顏色相同，則不需要重複填色，直接結束
            if (targetColor.ToArgb() == fillColor.ToArgb()) return;

            // 3. 建立一個明確的 Stack 來儲存待處理的像素點
            Queue<Point> pixels = new Queue<Point>();
            pixels.Enqueue(startPoint);

            // 4. 開始迴圈直到 Stack 裡面沒有像素為止
            while (pixels.Count > 0)
            {
                Point current = pixels.Dequeue();
                int x = current.X;
                int y = current.Y;

                // 5. 檢查當前像素是否超出圖形邊界
                if (x < 0 || x >= bmp.Width || y < 0 || y >= bmp.Height) continue;

                // 6. 檢查當前像素顏色是否與目標原始顏色一致
                if (bmp.GetPixel(x, y).ToArgb() != fillColor.ToArgb() && bmp.GetPixel(x, y).ToArgb() != targetColor.ToArgb())
                {
                    // 7. 變更當前像素的顏色
                    bmp.SetPixel(x, y, fillColor);

                    // 8. 將相鄰的四個方向（4-連通）推入 Queue 中待處理
                    pixels.Enqueue(new Point(x + 1, y));     // 右
                    pixels.Enqueue(new Point(x - 1, y));     // 左
                    pixels.Enqueue(new Point(x, y + 1));     // 下
                    pixels.Enqueue(new Point(x, y - 1));     // 上
                }
            }
        }
    }
}
