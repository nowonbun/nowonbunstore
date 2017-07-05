using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }
        public Program()
        {
            String express;
            Decimal answer;

            Console.WriteLine("Usable operators : + - * / % ^ ( )");
            Console.WriteLine("Usable Functions : abs exp log ln sqrt\nsin cos tan asin acos atan sinh cosh tanh");
            Console.WriteLine("Usable Math : pi(3.14) e(2.72)");
            Console.WriteLine("Usable statistics operators : C(cooperative)  P(permutation)  !(factorial)");
            Console.WriteLine("If You press Enter key that will be exit.");
            do
            {
                Console.Write("Commnder : ");
                express = Console.ReadLine();
                if (String.Empty.Equals(express))
                {
                    break;
                }
                try
                {
                    answer = Calculator.Calculate.calculate(express);
                    Console.WriteLine("Answer : {0}\n\n", answer);
                }
                catch (Exception)
                {
                    Console.WriteLine("Commnader is wrong");
                }
            } while (true);
        }
    }
}
