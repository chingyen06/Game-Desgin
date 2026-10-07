namespace hw3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = getValidNumber4();
        }

        List<string> getValidNumber4()
        {
            SimpleGuessAB guessAB = new SimpleGuessAB();
            string no;
            List<string> result = new List<string>();

            int count = 0;
            for (int k = 0; k < 10000; k++)
            {
                no = $"{k:d04}";
                if (guessAB.validNumber(no))
                {
                    count++;
                    result.Add(no);
                }
            }

            return result;
        }
    }

    class SimpleGuessAB
    {
        int numberLen = 4;
        public bool validNumber(string no)
        {
            // check the length first,
            if (no.Length != numberLen) return false;

            // repeated number found or not,
            for (int i = 0; i < no.Length - 1; i++)
            {
                for (int j = i + 1; j < no.Length; j++)
                {
                    if (no[i] == no[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int[] getAB(string guess, string number)
        {
            int[] ab = new int[] { 0, 0 };
            if (validNumber(guess) && validNumber(number))
            {
                for (int p = 0; p < numberLen; p++)
                {
                    for (int q = 0; q < numberLen; q++)
                    {
                        if (guess[p] == number[q])
                        {
                            if (p == q)
                                ab[0]++;
                            else
                                ab[1]++;

                            break;
                        }
                    }
                }
            }
            return ab;
        }
    }
}
