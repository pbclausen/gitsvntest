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
using Shared.Items.Attachments;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Interfaces.Macros;
using UnitTest;

namespace Programs
{
    class Storage1
    {
        public static void Main(String[] args)
        {
            ICommInterface commInterface = new SocketClientWithConsole(GlobalConfiguration.host, GlobalConfiguration.port);

            IDSPProxy dspProxy = new DSPProxy(
                commInterface,
                "C:/src/instrumentation/Dsp/KISS/KissXi/VTS/Debug/VTS.out",
                GlobalConfiguration.loglevel
                );

            IItemInterface itemInterface = new ItemInterface(dspProxy);
            IDSPController dspController = new DSPController(itemInterface);

            IItemController itemController = new ItemController(dspController);

            // Create a FunctionsController object
            IMacros macros = new MacroController(itemController);
            // Set program to run on the DSP as well as other parameters, and configure the connection
            macros.Open();

//            Thread.Sleep(1000);

            IItemController controller = macros.ItemController;

            IConfig config = macros.Config;
            IStorage storage = macros.Storage;
            IM_VTS m_VTS = macros.M_VTS;

            uint moduleId = 1;
            uint funcId = 1;
            uint channel = 0;
            uint sequenceType = 8;
            uint dataType = 8;
            uint nOutputs = 1;
            uint[] outputIds = new uint[] { 0, 1 };
            uint bufSize = 4 * 65536;

            OutputWrapper.WriteLine("Running");

//            config.StopCodec();

            for (; channel < 6; channel++)
            {
                storage.ConfigChannel(moduleId, funcId, channel, sequenceType, dataType, nOutputs, outputIds, bufSize);

            }

            for (; channel < 7; channel++)
                storage.ConfigChannel(moduleId, funcId, channel, sequenceType, dataType, 2, outputIds, bufSize);
            OutputWrapper.WriteLine("Configs sent");

            storage.SetUpTrigger(0, 0, true);
//            config.SetUpStorageTrigger(0, 0, true);
//            config.SetUpStorageTrigger(200, 200, false);
//            config.SetUpStorageTrigger(0, 0, false);
            OutputWrapper.WriteLine("Set up done");

            channel = 1;
            controller.PutItem(new FloatData(CH.CH_SP, Kind.M_IDA_DATA, new DataDescriptor(0, 0, 48000, 0, 1, 1, new SigId(moduleId, funcId, channel, sequenceType, dataType)), Generator.Sine((float)Math.Sqrt(2), 100f, (uint)48000, (uint)512)));
            OutputWrapper.WriteLine("Data sent");

            while (!controller.HasIncoming(Kind.M_IDA_DATA)) ;

            OutputWrapper.WriteLine("Data ready");
            bool done = false;
            while (!done)
            {
                if (controller.HasIncoming())
                //if (collector.HasIncoming(Kind.M_IDA_DATA, CH.CH_SP))
                {
                    OutputWrapper.WriteLine(controller.GetItem().ToString());//, (ushort)CH.CH_STORAGE).ToString());
                    //OutputWrapper.WriteLine(collector.GetItem(Kind.M_IDA_DATA).ToString());//, (ushort)CH.CH_STORAGE).ToString());
                }
//                else
//                    done = true;
            }
            OutputWrapper.WriteLine("Everything done. Waiting");
            Thread.Sleep(5000);
            OutputWrapper.WriteLine("Shutting down");
            macros.Close();
        }
    }
}
