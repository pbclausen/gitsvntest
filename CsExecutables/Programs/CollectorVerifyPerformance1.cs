using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Shared.Interfaces;
using Shared.Items;
using Shared.Debug;
using Shared.Constants;
using ItemHandler;
using DSPConnector.Comm;
using DSPConnector;
using UnitTest;

namespace Programs
{
    class CollectorVerifyPerformance1
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

            Item item = new PerformanceMode(CH.CH_PERFORMANCE, Kind.PERFORMANCE_MODE, 2).ToItem();
            itemController.PutItem(item);
            OutputWrapper.WriteLine("Item 1 sent");

            for (int i = 0; i < 2; i++)
            {
                item = new Item(CH.CH_PERFORMANCE, Kind.PERFORMANCE_PRINT).ToItem();
                itemController.PutItem(item);
                Thread.Sleep(2000);
                OutputWrapper.WriteLine("Sending");
            }
            while (itemController.HasIncoming(Kind.PERFORMANCE_PRINT))
            {
                PerformancePC pItem = new PerformancePC(itemController.GetItem(Kind.PERFORMANCE_PRINT));
                OutputWrapper.WriteLine(pItem.ToString());
            }

            // Request an Item that is not going to come. Timeout test
//            itemController.GetItem();
            itemController.Close();
        }
    }
}
