using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSPConnector.Comm;
using System.Threading;
using Shared.Items;
using Shared.Interfaces;
using DSPConnector;
using ItemHandler;
using Shared.Debug;
using UnitTest;

namespace Shared
{
    public class NewTest2
    {
        public static void Main(String[] args)
        {
            ICommInterface socketClient = new SocketClientWithConsole(GlobalConfiguration.host, GlobalConfiguration.port);
            IDSPProxy dspProxy = new DSPProxy(
                socketClient,
                "C:/src/instrumentation/Dsp/KISS/KissXi/VTS/Debug/VTS.out",
                GlobalConfiguration.loglevel
                );
            IItemInterface item2Dsp = new ItemInterface(dspProxy);
            IDSPController dspController = new DSPController(item2Dsp);

            IItemController itemController = new ItemController(dspController);

            itemController.Open();

            Thread.Sleep(1000);

            itemController.PutItem(new PerformancePrint().ToItem());
            itemController.PutItem(new PerformancePrint().ToItem());
            itemController.PutItem(new PerformancePrint().ToItem());
            itemController.PutItem(new PerformancePrint().ToItem());
            itemController.PutItem(new PerformancePrint().ToItem());
            itemController.PutItem(new PerformanceMode(2).ToItem());
            itemController.PutItem(new PerformancePrint().ToItem());
            PerformancePC ppc = new PerformancePC(itemController.GetItem());
            OutputWrapper.WriteLine(ppc.ToString());
            
            Thread.Sleep(2000);

            itemController.Close();
        }
    }
}