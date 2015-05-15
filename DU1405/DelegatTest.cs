using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU1405
{

    public delegate int BinaryOp(int x, int y);

    class DelegateTest
    {
        static int Main(string[] args)
        {
            BinaryOp bo = DelegateTest.sucet;

            Console.WriteLine(bo(2,4));
            bo = DelegateTest.rozdiel;
            Console.WriteLine(bo(2,4));

            Console.Read();

            return 0;
        }

        public static int sucet(int x, int y)
        {
            Console.WriteLine("sucet");
            return x + y;
        }

        public static int rozdiel(int x, int y)
        {
            Console.WriteLine("rozdiel");
            return x - y;
        }
    }
}
