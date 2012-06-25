using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macros.Helpers;

namespace Shared
{
    public class GeneratorTest
    {
        static System.Globalization.NumberFormatInfo ni = null;

        public static void Main(String[] args)
        {
            System.Globalization.CultureInfo ci =
               System.Globalization.CultureInfo.InstalledUICulture;
            ni = (System.Globalization.NumberFormatInfo)
               ci.NumberFormat.Clone();
            ni.NumberDecimalSeparator = ".";
            float[] output = Generator.Sine((float)Math.Sqrt(2), 10000f, 48000, 512);
            Console.WriteLine("{ ");
            for (int i = 1; i < output.Length-1; i++)
            {
                float o = output[i]/2;
                Console.Write(String.Format("{0:0.#####}", o).Replace(",", ".") + ", ");
                if (i % 10 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine(String.Format("{0:0.#####}", output[output.Length - 1] / 2).Replace(",", "."));
            Console.WriteLine(" };");
        }
    }
}
