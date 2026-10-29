using System.ComponentModel;

namespace DragPictureBoxes
{
    public partial class Form1 : Form
    {
        Bitmap pokers = Properties.Resources.poker;
        Card card;

        Card[] cards = null;

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();

            card = new Card(pokers, 5, 13, 0);

            card.Left = rand.Next(50, Width - 100);
            card.Top  = rand.Next(menuStrip1.Height, Height - 100);

            Controls.Add(card);  // top-left
        }

        private void 改變花色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            card.Index = card.Index + 13;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dispose the original controls first,
            if (cards != null)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    cards[i].Dispose();  // <-- removet the previous ones
                }
            }
            // 
            int n = rand.Next(5, 52);
            cards = new Card[n];

            for (int i = 0; i < n; i++)
            {
                int cardNo = rand.Next(0, 58);  // 52 + 6 = 58
                cards[i] = new Card(pokers, 5, 13, cardNo);  

                cards[i].Left = rand.Next(50, Width - 100);
                cards[i].Top  = rand.Next(menuStrip1.Height, Height - menuStrip1.Height - 100);
                Controls.Add(cards[i]);  // top-left
            }
        }
    }

    //----------------------------------
    // 建立一個繼承自 PictureBox 的 class Card,
    class Card : PictureBox
    {
        Image wholeImage;  // shared image for all objects
        int row, col;
        private int index;
        int cellW, cellH;

        public Card(Image image, int row, int col, int index)
        {
            wholeImage = image;  // Image in PictureBox

            // 使用 PictureBoxSizeMode 列舉的值，而不是類型名稱
            SizeMode = PictureBoxSizeMode.AutoSize;

            this.row = row;
            this.col = col;
            cellW = wholeImage.Width / col;
            cellH = wholeImage.Height / row;

            toIndex(index);  // 確認是合法的索引。

            assignImage();
            // 
            MouseDown += mouse_down;
            MouseMove += mouse_move;
            MouseUp   += mouse_up;
        }

        // 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // 上述用法的說明: https://share.google/aimode/inNouZCr6I8pW685q
        public int Index
        {
            get { return index; }
            set { index = value;  toIndex(index); }
        }

        public void toIndex(int index)
        {
            if (index < 0)
                this.index = 0;
            else if (index > row * col)
                this.index %= row * col;
            else
                this.index = index;

            assignImage();
        }
       
        public void toRC(int r, int c)
        {
            if (r < 0 || r >= row || c < 0 || c >= col)
                index = 0;
            else
                index = r * col + c;

            assignImage();
        }

        public void moveTo(int px, int py)
        {
            Left = px;
            Top  = py;
        }

        void assignImage()
        {
            // 建立指定寬高的 Bitmap 物件:
            Bitmap bmp = new Bitmap(cellW, cellH);

            // 將需要的內容寫入 bmp 的做法:
            using (Graphics g = Graphics.FromImage(bmp))
            {
                int r = index / col;  // int div
                int c = index % col;

                g.DrawImage(wholeImage,
                            new Rectangle(0, 0, cellW, cellH),
                            new Rectangle(c * cellW, r * cellH, cellW, cellH), GraphicsUnit.Pixel);
            }
            // assign the selected part of image to PictureBox.Image,
            Image = bmp;
        }

        //----------------------------------------------------------------------------------------
        // for adding the dragging functions,
        private bool dragging = false;
        int prevX, prevY;

        void mouse_down(object sender, MouseEventArgs e)
        {
            dragging = true;
            prevX = e.X;
            prevY = e.Y;

            BringToFront();  // 能將此物件提升到目前作用視窗的最上層。
        }

        void mouse_move(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                int dx = e.X - prevX;
                int dy = e.Y - prevY;

                Left += dx;
                Top += dy;
            }
        }

        void mouse_up(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        //----------------------------------------------------------------------------------------
    }
}
