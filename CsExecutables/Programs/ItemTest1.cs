using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Interfaces;
using Macros;
using Shared.Debug;
using System.Threading;
using Shared.Items;
using Macros.Helpers;
using Shared.Constants;

namespace Programs
{
    class ItemTest1
    {
        public static void Main(String[] args)
        {
            Item item = new Item();
            item.Type = Kind.PERFORMANCE_MODE;
            if (item.Type != Kind.PERFORMANCE_MODE)
                throw new Exception("Kind set");
            Console.WriteLine("Set Kind OK");

            item.ReplyBit = true;
            if (item.Type != Kind.PERFORMANCE_MODE || item.ReplyBit != true)
                throw new Exception("Reply set");
            Console.WriteLine("Set Reply OK");

            if (item.Type != Kind.PERFORMANCE_MODE)
                throw new Exception("Kind altered");
            Console.WriteLine("Check Kind OK");

            item.Type = Kind.M_IDA_DATA;
            if (item.Type != Kind.M_IDA_DATA || item.ReplyBit != true)
                throw new Exception("Kind reset");
            Console.WriteLine("Reset Kind OK");

            if (item.Type != Kind.M_IDA_DATA)
                throw new Exception("Kind altered");
            Console.WriteLine("Check Kind OK");

            item.ReplyBit = false;
            if (item.Type != Kind.M_IDA_DATA)
                throw new Exception("Reply reset");
            Console.WriteLine("Reset Reply OK");

            item.Status = 0x80;
            if (item.Type != Kind.M_IDA_DATA || item.ReplyBit != false)
                throw new Exception("Status set");
            Console.WriteLine("Set Status OK");

            if (item.Type != Kind.M_IDA_DATA)
                throw new Exception("Kind altered");
            Console.WriteLine("Check Kind OK");
        }
    }
}
