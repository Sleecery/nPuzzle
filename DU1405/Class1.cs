using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DU1405
{
    class Class1
    {
        public static int Main(string[] args)
        {
            int[] pole = { 1, 2, 3, 4, 5 };

            foreach (var item in pole)
            {
                Console.WriteLine(item);
            }

            Transform tr = Negate;
            forEachWrite(pole, tr);

            forEachWrite(pole, delegate(int x) { return -x; });

            forEachWrite(pole, x => { return -x; });

            forEachWrite(pole, x => -x);

                    
            Console.ReadLine();
            return 0;
        }
        delegate int Transform(int x);

        static void forEachWrite(int[] pole, Transform transform)
        {
            foreach (int item in pole)
            {
                Console.WriteLine(transform(item));
            }
        }

        static int Negate(int x)
        {
            return -x;
        }
    }
}
