using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macros;
using Macros.Data;
using Shared.Interfaces;
using Shared.Items.Attachments;
using Shared.Debug;
using System.Threading;
using DSPConnector;
using DSPConnector.Comm;
using ItemHandler;
using Shared.Interfaces.Macros;
using UnitTest;

namespace Programs
{
    class M_IDAFunctionsStresss1
    {
        public static void Main(string[] args)
        {
            // Define filter parameters etc.
            uint ch = 0;
            uint filt = 1;
            uint func = 1;
            uint Fs = 48000;
            uint sendBufSize = 512;
            uint freq = 10000;

            // Run in an infinite loop
            while (true)
            {
                ICommInterface commInterface = new SocketClientWithConsole(GlobalConfiguration.host, GlobalConfiguration.port);

                IDSPProxy dspProxy = new DSPProxy(
                    commInterface,
                    "C:/src/instrumentation/Dsp/KISS/KissXi/6ch-gisp/Debug/6ch-gisp.out",
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

                // Get the M_IDA macros collection from the macros controller
                IM_IDA m_IDA = macros.M_IDA;

                // Stop codec to avoid unnecessary babble
                m_IDA.Codec_Stop();
                
                // Configure the channel
                m_IDA.Config_Service(new M_IDA_Config_ChannelConfig[] { new M_IDA_Config_ChannelConfig(1, ch, 7, 1, filt) });
                
                // Set input amplitude
                float a_in = (float)Math.Sqrt(2);
                // Send harmonic to the DSP and receive the results
                float[] result = m_IDA.SendHarmonicAndReceiveResult(func, ch, a_in, freq, Fs, sendBufSize);
                // Find amplitude of resulting waveform, and calculate the resulting gain
                float a_out = result.Max();
                float res = 20 * (float)Math.Log10(a_out / a_in);
                OutputWrapper.WriteLine("Result: " + res + "dB");
                // Close the connection to the DSP and other services
                Thread.Sleep(1000);
                macros.Close();
            }
        }
    }
}
