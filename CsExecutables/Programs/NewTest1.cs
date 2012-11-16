using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSPConnector.Comm;
using System.Threading;
using Shared.Items;
using Shared.Interfaces;
using DSPConnector;
using UnitTest;

namespace Shared
{
    public class NewTest1
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
            dspController.Open();

            Thread.Sleep(1000);

            while (!dspController.WriteReady()) ;
            dspController.Write(new PerformancePrint().ToItem());
            while (!dspController.WriteReady()) ;
            dspController.Write(new PerformancePrint().ToItem());
            while (!dspController.WriteReady()) ;
            dspController.Write(new PerformancePrint().ToItem());
            while (!dspController.WriteReady()) ;
            dspController.Write(new PerformancePrint().ToItem());
            while (!dspController.WriteReady()) ;
            dspController.Write(new PerformancePrint().ToItem());

            Thread.Sleep(2000);

            dspController.Close();
        }
    }
}