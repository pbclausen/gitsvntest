using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Shared.Interfaces;
using DSPConnector;
using Shared.Items;
using Shared.Debug;
using DSPConnector.Comm;
using UnitTest;

namespace Programs
{
    class CommStressPerformance1
    {
        static void Main(string[] args)
        {
            ICommInterface commInterface = new SocketClientWithConsole(GlobalConfiguration.host, GlobalConfiguration.port);

            IDSPProxy dspProxy = new DSPProxy(
                commInterface,
                "C:/src/instrumentation/Dsp/KISS/KissXi/PerformanceTest/Debug/PerformanceTest.out",
                GlobalConfiguration.loglevel
                );

            IItemInterface itemInterface = new ItemInterface(dspProxy);
            IDSPController dspController = new DSPController(itemInterface);

            while (true)
            {
                dspController.Open();

                while (dspController.ReadReady())
                {
                    OutputWrapper.WriteLine("Not empty");
                    dspController.Read();
                }
                if (!dspController.ReadReady())
                    OutputWrapper.WriteLine("Empty");

                Item item = new PerformanceMode(2).ToItem();
                while(!dspController.WriteReady());
                dspController.Write(item);
                OutputWrapper.WriteLine("Item 1 sent");

                int i;
                for (i = 0; i < 5; i++)
                {
                    item = new PerformancePrint().ToItem();
                    while (!dspController.WriteReady()) ;
                    dspController.Write(item);
                    OutputWrapper.WriteLine("Sending");
                }

                for (; i > 0; i--)
                {
                    OutputWrapper.WriteLine("" + i);
                    while (!dspController.ReadReady()) ;
                    item = dspController.Read();
                }
                OutputWrapper.WriteLine(item.ToString(true));
                
                dspController.Close();
            }
        }
    }
}
