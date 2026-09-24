using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayGuessAB playGuessAB = new PlayGuessAB();

            playGuessAB.guessAB();
        }

        class PlayGuessAB : GuessAB
        {
            public void guessAB()
            {
                string guess, answer = "";
                int[] ab;
                Console.WriteLine("Bulls & Cows Game:");

                while (true)
                {
                    Console.Write("Your guess: ");
                    guess = Console.ReadLine();

                    if (validNumber(guess))
                    {
                        ab = getAB(guess);

                        answer = answer + $"{guess} => {ab[0]}A{ab[1]}B\n";

                        Console.WriteLine(answer);
                        // Console.WriteLine(Number);

                        if (ab[0] == numberLen)
                        {
                            Console.WriteLine($"You've got the number, {Number}");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("數字格式不符合");
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