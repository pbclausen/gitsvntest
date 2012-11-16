using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Shared.Interfaces;
using Macros;
using Shared.Items.Attachments;
using Shared.Debug;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Interfaces.Macros;
using UnitTest;

namespace Programs
{
    class PerformanceMonitorFunctionsStress1
    {
        public static void Main(string[] args)
        {
            // Run the test infinitely
            while (true)
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

                // Create Macros controller object
                IMacros macros = new MacroController(itemController);
                // Specify parameters and program to run on DSP, and open the connection
                macros.Open();

                // Get the performance monitor collection from the Macros controller
                IPerformanceMonitor performanceMonitor = macros.PerformanceMonitor;
                // Set performance monitor mode to 2 (write results to PC)
                performanceMonitor.SetMode(2);

                // Get several performance measurements in quick succession
                uint[][] ap = null;
                for (int i = 0; i < 20; i++)
                {
                    ap = performanceMonitor.getPerformanceMeasurements();
                }

                // Output the last results
                for (int i = 0; i < ap.Length; i++)
                {
                    OutputWrapper.Write(" | ");
                    for (int j = 0; j < ap[i].Length; j++)
                    {
                        OutputWrapper.Write(ap[i][j] + " | ");
                    }
                    OutputWrapper.WriteLine("");
                }

                // Close connection to the DSP
                macros.Close();
            }
        }
    }
}
