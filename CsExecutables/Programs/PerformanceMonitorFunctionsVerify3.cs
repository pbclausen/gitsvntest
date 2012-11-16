using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macros;
using Shared.Interfaces;
using System.Threading;
using Shared.Items.Attachments;
using Shared.Debug;
using Macros.Helpers;
using Shared.Constants;
using System.Diagnostics;
using Shared.Items;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Interfaces.Macros;
using UnitTest;

namespace Programs
{
    class PerformanceMonitorFunctionsVerify3
    {
        public static void Main(string[] args)
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

            // Create a FunctionsController object
            IMacros functions = new MacroController(itemController);
            // Set program to run on the DSP as well as other parameters, and configure the connection
            functions.Open();

            Thread.Sleep(1000);

            // Get the performance monitor collection from the Macros controller
            OutputWrapper.WriteLine("Get PerformanceMonitor object");
            IPerformanceMonitor performanceMonitor = functions.PerformanceMonitor;
            // Set performance monitor period to 1000 (1s)
            OutputWrapper.WriteLine("Setting Performance Monitor readout period to 1000 ms");
            performanceMonitor.SetPeriod(1000);
            // Set performance monitor mode to 2 (write results to PC)
            OutputWrapper.WriteLine("Setting Performance Monitor mode to 3 (continuous run + readout to PC)");
            performanceMonitor.SetMode(3);//2&1);
            OutputWrapper.WriteLine("Emptying incoming queue");
            while (!functions.ItemController.HasIncoming(Kind.PERFORMANCE_PRINT)) ;
            while (functions.ItemController.HasIncoming(Kind.PERFORMANCE_PRINT))
                functions.ItemController.GetItem(Kind.PERFORMANCE_PRINT);

            List<Attachment_PerformancePC> performanceItems = new List<Attachment_PerformancePC>();

            OutputWrapper.WriteLine("Get 5 measurements");
            for (int i = 0; i < 50; i++)
            {
                OutputWrapper.WriteLine("Start stopwatch");
                Stopwatch sw = Stopwatch.StartNew();
                OutputWrapper.WriteLine("Wait for item to be ready in queue");
                while (!functions.ItemController.HasIncoming(Kind.PERFORMANCE_PRINT)) ;
                OutputWrapper.WriteLine("Stop stopwatch");
                sw.Stop();
                OutputWrapper.WriteLine("Get measurements and output them");
                PerformancePC performancePC = new PerformancePC(functions.ItemController.GetItem(Kind.PERFORMANCE_PRINT));
                Console.WriteLine(performancePC.ToString());
                OutputWrapper.WriteLine("Output stopwatch display");
                Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
                performanceItems.Add(performancePC.Data);
            }
            FileOutput.writePerformanceCSV("id2.csv", performanceItems, 2);

            OutputWrapper.WriteLine("Setting Performance Monitor readout period to 5000 ms");
            performanceMonitor.SetPeriod(5000);

            OutputWrapper.WriteLine("Get 5 measurements");
            for (int i = 0; i < 5; i++)
            {
                OutputWrapper.WriteLine("Start stopwatch");
                Stopwatch sw = Stopwatch.StartNew();
                OutputWrapper.WriteLine("Wait for item to be ready in queue");
                while (!functions.ItemController.HasIncoming(Kind.PERFORMANCE_PRINT)) ;
                OutputWrapper.WriteLine("Stop stopwatch");
                sw.Stop();
                OutputWrapper.WriteLine("Get measurements and output them");
                Console.WriteLine(new PerformancePC(functions.ItemController.GetItem(Kind.PERFORMANCE_PRINT)).ToString());
                OutputWrapper.WriteLine("Output stopwatch display");
                Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
            }

            // Write performance data to a .csv file
//            FileOutput.writePerformanceCSV("PerformanceOutput.csv", ap);
            // Close connection to the DSP
            OutputWrapper.WriteLine("All done, shutting down");
            functions.Close();
        }
    }
}
