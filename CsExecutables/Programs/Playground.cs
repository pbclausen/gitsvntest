using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Items;

namespace Programs
{
    class Playground
    {
        public static void Main(string[] args)
        {
            Item item1 = new PerformanceMode(2);
            Item item2 = new PerformancePrint();
            Item item3 = new PerformanceMode(2);
            LinkedList<Item> list = new LinkedList<Item>();
            list.AddLast(item1);
            Console.WriteLine(list.Contains(item3));
        }
    }
}
