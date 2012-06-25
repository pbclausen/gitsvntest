using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace UnitTest
{
    class Exec
    {
        public static void Main(String[] args)
        {
            Stream outStream = ((StreamWriter)Console.Out).BaseStream;
            TextWriter tw = new StreamWriter(outStream);

            Console.Out.NewLine = "\nCSH: ";
            Console.WriteLine("");
            Console.WriteLine("Hej med dig");
            Console.WriteLine("Hej med dig");
            tw.WriteLine("dig med Hej");
            tw.WriteLine("dig med Hej");
            Console.Write("Hej ");
            Console.WriteLine(" med dig");
            Thread.Sleep(1000);
            Console.WriteLine("Hej med dig");
            Console.WriteLine("Hej med dig");
            Console.Write("Hej ");
            Console.WriteLine(" med dig");
        }
    }
}
