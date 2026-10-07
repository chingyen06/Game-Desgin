using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int num = 0, total = 0, temp_t;

            while (num < 10000)
            {
                int[] check = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                temp_t = 0;

                string ans = $"{num:d04}";

                for (int i = 0; i < ans.Length; i++)
                {
                    {
                        if (check[ans[i] - '0'] == 0)
                        {
                            temp_t++;
                            check[ans[i] - '0']++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (temp_t == 4)
                    total++;

                num++;
            }

            Console.WriteLine($"Total: {total}");
        }
    }
}
