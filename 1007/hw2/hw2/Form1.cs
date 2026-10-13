using static hw2.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace hw2
{
    public partial class Form1 : Form
    {
        private int row = 5, col = 4, sx = 15, sy = 15, ds = 5;
        private Poker pok = new Poker(5, 4);

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(800, 600);

            this.DoubleBuffered = true;
            this.Paint += Form1_Paint;
        }

        public class Poker
        {
            private int row, col;
            private int[] nos;
            public int[] cards;
            public int[] selected;

            public Poker(int row, int col)
            {
                this.row = row;
                this.col = col;
                this.cards = new int[row * col];
                this.selected = new int[row * col];
                this.nos = new int[52];

                for (int k = 0; k < selected.Length; k++)
                {
                    selected[k] = 0;
                }

                shuffle();
            }

            void shuffle()
            {
                Random random = new Random();
                int p;

                for (int k = 0; k < 52; k++)
                {
                    nos[k] = k;
                }

                for (int k = nos.Length - 1; k > 0; k--)
                {
                    p = random.Next(k + 1);
                    (nos[k], nos[p]) = (nos[p], nos[k]);
                }

                for (int k = 0; k < row * col / 2; k++)
                {
                    cards[k] = nos[k];
                    cards[k + row * col / 2] = nos[k];
                }

                for (int k = cards.Length - 1; k > 0; k--)
                {
                    p = random.Next(k + 1);
                    (cards[k], cards[p]) = (cards[p], cards[k]);
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Image image = Properties.Resources.poker;

            int pokerW = image.Width / 13;
            int pokerH = image.Height / 5;
            int x, y, val_x, val_y;

            Graphics g = e.Graphics;

            g.Clear(Color.White);

            for (int k = 0; k < pok.cards.Length; k++)
            {
                x = k % col;
                y = k / col;
                val_x = pok.cards[k] % 13;
                val_y = pok.cards[k] / 13;

                if (pok.selected[k] == 1)
                {
                    Rectangle destRect = new Rectangle(sx + (x * pokerW) + ds, sy + (y * pokerH) + ds, pokerW, pokerH);
                    Rectangle sourceRect = new Rectangle(val_x * pokerW, val_y * pokerH, pokerW, pokerH);

                    g.DrawImage(image, destRect, sourceRect, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle destRect = new Rectangle(sx + (x * pokerW) + ds, sy + (y * pokerH) + ds, pokerW, pokerH);
                    Rectangle sourceRect = new Rectangle(0 * pokerW, 4 * pokerH, pokerW, pokerH);

                    g.DrawImage(image, destRect, sourceRect, GraphicsUnit.Pixel);
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int pokerW = Properties.Resources.poker.Width / 13;
                int pokerH = Properties.Resources.poker.Height / 5;
                int x = (e.X - sx) / pokerW;
                int y = (e.Y - sy) / pokerH;

                if (x >= 0 && x < col && y >= 0 && y < row)
                {
                    int index = y * col + x;
                    pok.selected[index] = 1 - pok.selected[index];
                }

                this.Invalidate();
            }
        }
    }
}
