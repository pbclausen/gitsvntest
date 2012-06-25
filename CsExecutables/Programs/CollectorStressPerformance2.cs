using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Shared.Interfaces;
using Shared.Items;
using Shared.Debug;
using Shared.Constants;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using UnitTest;

namespace Programs
{
    class CollectorStressPerformance2
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

            IItemController itemController = new ItemController(dspController);

            itemController.Open();

            Item item = new PerformanceMode(2).ToItem();
            itemController.PutItem(item);
            OutputWrapper.WriteLine("Item 1 sent");
            while (true)
            {
                for (int i = 0; i < 30; i++)
                {
                    item = new PerformancePrint().ToItem();
                    itemController.PutItem(item);
                    OutputWrapper.Write("#");
                }
                OutputWrapper.Write("-\n");
                for (int i = 0; i < 30; i++)
                {
                    while (!itemController.HasIncoming(Kind.PERFORMANCE_PRINT)) ;

                    OutputWrapper.Write("¤");
                    PerformancePC pItem = new PerformancePC(itemController.GetItem(Kind.PERFORMANCE_PRINT));
                }
                OutputWrapper.Write("-\n");
            }
            itemController.Close();
        }
    }
}
