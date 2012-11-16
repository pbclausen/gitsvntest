using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macros;
using Shared.Interfaces;
using Shared.Items.Attachments;
using Shared.Debug;
using System.Threading;
using Macros.Helpers;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Interfaces.Macros;
using UnitTest;

namespace Programs
{
    class PerformanceMonitorFunctionsVerify2
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
            IMacros macros = new MacroController(itemController);
            // Set program to run on the DSP as well as other parameters, and configure the connection
            macros.Open();

            Thread.Sleep(1000);

            // Get the performance monitor collection from the Macros controller
            IPerformanceMonitor performanceMonitor = macros.PerformanceMonitor;
            // Set performance monitor mode to 2 (write results to PC)
            performanceMonitor.SetMode(2);
            // Wait for the performance monitor to gather some data
            Thread.Sleep(5000);
            // Retrieve data from the DSP
            uint[][] ap = performanceMonitor.getPerformanceMeasurements();
            // Write out the performance data received
            for (int i = 0; i < ap.Length; i++)
            {
                OutputWrapper.Write(" | ");
                for (int j = 0; j < ap[i].Length; j++)
                {
                    OutputWrapper.Write(ap[i][j] + " | ");
                }
                OutputWrapper.WriteLine("");
            }
            // Write performance data to a .csv file
            FileOutput.writePerformanceCSV("PerformanceOutput.csv", ap);
            // Close connection to the DSP
            macros.Close();
        }
    }
}
