using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Interfaces;
using Shared.Items;
using Shared.Items.Attachments;
using System.Threading;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Debug;
using UnitTest;
using Shared.Constants;

namespace Programs
{
    class Config1
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

            itemController.Open();

            Thread.Sleep(2000);
            itemController.PutItem(new StorageChannelConfig(new SigId(0, 0, 1, 0, 0), 0, new uint[] { 1, 0 }, 512));
            OutputWrapper.WriteLine("1");
            Thread.Sleep(2000);
            itemController.PutItem(new CodecConfig(0, 1));
            OutputWrapper.WriteLine("2");
            Thread.Sleep(2000);
            itemController.PutItem(new ControlParam(CH.CH_CODEC, Kind.M_IDA_CONFIG, 1, CONTROLSTRATEGY.CONTROLSTRATEGY_WEIGHTING));
            OutputWrapper.WriteLine("3");
            Thread.Sleep(2000);
            itemController.PutItem(new FftSetup(CH.CH_RANDANALYSIS, Kind.M_VTS_ANALYSIS_CONFIG, 0, 1, 5, 4, 2, 3, 6, 7, 8));
            OutputWrapper.WriteLine("4");
            Thread.Sleep(2000);
            itemController.PutItem(new InputChannelSetup(CH.CH_CODEC, Kind.M_IDA_CONFIG, 0, 1, CHANNELTYPE.CHANNELTYPE_CONTROL, 1.2f));
            OutputWrapper.WriteLine("5");
            Thread.Sleep(2000);
            itemController.Close();
            OutputWrapper.WriteLine("Closed");
        }
    }
}
