using System;
using System.Collections.Generic;
using System.Linq;

namespace Redox_Code_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            for (int i = 1; i < 100; i++)
            {
                list.Add(i);
            }

            printEven(list);
            printThreeOrFive(list);
        }

        static void printEven(List<int> list) 
        {
            IEnumerable<int> isEven = from item in list where item % 2 == 0 select item;
            String result = "";
            foreach (int item in isEven)
            {
                result+=item + ", ";
            }
            Console.WriteLine(result.Substring(0, result.Length - 2));

        }

        static void printThreeOrFive(List<int> list) 
        {
            String result = "";
            foreach (int item in list) 
            {
                if(((item%3==0)&&(item%5!=0))||((item%3!=0)&&(item%5==0))) result+=item+", ";
            }
            Console.WriteLine(result.Substring(0,result.Length-2));
        }
    }
}
