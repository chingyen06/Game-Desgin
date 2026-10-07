using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ComputerGuessAB computerGuessAB = new ComputerGuessAB();

            computerGuessAB.guessAB();
        }

        class ComputerGuessAB : GuessAB
        {
            // 使用 candidates 來儲存可能的數字:
            public List<string> candidates = new List<string>();
            private string guess, answer = "";

            // 生成所有可能的有效數字:
            public void genCandidates() {
                string no;

                int count = 0;
                for (int k = 0; k < 10000; k++)
                {
                    no = $"{k:d04}";
                    if (validNumber(no))
                    {
                        count++;
                        candidates.Add(no);
                    }
                }
            }

            // 從 candidates 中，隨機取出一個數字:
            public string pickOneCandidate()
            {
                Random random = new Random();
                int index = random.Next(candidates.Count);

                return candidates[index];
            }

            // 移除 candidates 中不符合猜測 AB 值的部分，藉以縮減可能的範圍:
            public void removeNotIncluded(int[] ab) 
            { 
                int[] temp_ab;

                for (int k = 0; k < candidates.Count; k++)
                {
                    temp_ab = new int[2];

                    for (int i = 0; i < numberLen; i++)
                    {
                        for (int j = 0; j < numberLen; j++)
                        {
                            if (candidates[k][i] == guess[j])
                            {
                                if (i == j)
                                    temp_ab[0]++;
                                else
                                    temp_ab[1]++;
                            }
                        }
                    }

                    if (temp_ab[0] != ab[0] || temp_ab[1] != ab[1])
                    {
                        candidates.RemoveAt(k);
                        k--;
                    }
                }
            }

            public void guessAB()
            {
                int[] ab;

                genCandidates();

                Console.WriteLine("Bulls & Cows Game:");

                while (true)
                {
                    guess = pickOneCandidate();
                    ab = getAB(guess);

                    Console.WriteLine($"Computer guess: {guess}");

                    answer = $"{answer}{guess} => {ab[0]}A{ab[1]}B\n";

                    Console.WriteLine(answer);

                    removeNotIncluded(ab);

                    if (ab[0] == numberLen)
                    {
                        Console.WriteLine($"You've got the number, {Number}");
                        return;
                    }
                }
            }
        }

        class GuessAB
        {
            public int numberLen = 4;  // 數字的長度。
            static char[] nos = {'0', '1', '2', '3', '4',
                         '5', '6', '7', '8', '9'};

            private string number = "";  // the number to guess

            Random rand = new Random();

            // simple setter and getter:
            public string Number
            {
                get { return number; }
                set { number = value; }  // => obj.number = value;
            }

            // checking the no is valid or not,
            public bool validNumber(string no)
            {
                int[] num = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                // check the length first,
                if (no.Length != numberLen) return false;

                // repeated number found or not,
                for (int i = 0; i < no.Length; i++)
                {
                    // complete this part!  <-- DIY!
                    if (num[no[i] - '0'] == 1)
                        return false;

                    num[no[i] - '0'] = 1;
                }
                return true;
            }


            public void shuffle()
            {
                // based on Fisher-Yates shuffle,
                int p;
                for (int i = nos.Length - 1; i > 0; i--)
                {
                    p = rand.Next(i + 1);
                    (nos[i], nos[p]) = (nos[p], nos[i]);
                }
            }

            public string genNumber()
            {
                shuffle();

                string numStr = "";
                for (int i = 0; i < numberLen; i++)
                {
                    numStr += nos[i];
                }

                return numStr;
            }

            public int[] getAB(string guess)
            {
                int[] ab = new int[] { 0, 0 };  // save the numbers of A and B found: a -> ab[0]; b -> ab[1]。
                if (validNumber(guess))
                {
                    // complete this part!  <-- DIY!
                    for (int i = 0; i < numberLen; i++)
                    {
                        for (int j = 0; j < numberLen; j++)
                        {
                            if (guess[i] == Number[j])
                            {
                                if (i == j)
                                    ab[0]++;
                                else
                                    ab[1]++;
                            }
                        }
                    }
                }
                return ab;
            }

            public GuessAB()
            {
                Number = genNumber();
            }
        }
    }
}