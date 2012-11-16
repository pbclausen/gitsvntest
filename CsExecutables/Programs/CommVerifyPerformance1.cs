using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DSPConnector;
using Shared.Interfaces;
using Shared.Items;
using Shared.Debug;
using DSPConnector.Comm;
using UnitTest;

namespace Programs
{
    class CommVerifyPerformance1
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

            dspController.Open();

            // Create an Item from a new PerformanceMode item and send it to the DSP
            Item item = new PerformanceMode(2).ToItem();
            while (!dspController.WriteReady()) ;
            dspController.Write(item);
            OutputWrapper.WriteLine("Item 1 sent");

            // Send two print request Items to the DSP
            for (int i = 0; i < 2; i++)
            {
                item = new PerformancePrint().ToItem();
                while (!dspController.WriteReady()) ;
                dspController.Write(item);
                Thread.Sleep(2000);
                OutputWrapper.WriteLine("Sending");
            }
            // Receive one Item and print its contents
            while (!dspController.ReadReady()) ;
            item = dspController.Read();
            OutputWrapper.WriteLine(item.ToString(true));
            // Close the connection to the DSP
            dspController.Close();
        }
    }
}
