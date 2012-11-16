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
    class M_IDAFunctionsStresss2
    {
        public static void Main(string[] args)
        {
            // Define filter parameters etc.
            uint ch = 0;
            uint filt = 1;
            uint func = 1;
            uint Fs = 48000;
            uint sendBufSize = 512;

            // Prepare lists for frequencies tested and the results
            List<uint> freqs = new List<uint>();
            List<float> gain = new List<float>();

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

                // Run the test for a set of frequencies separated by an octave
                for (uint freq = 10; freq < 50000; freq += freq)
                {
                    // Set input amplitude
                    float a_in = (float)Math.Sqrt(2);
                    // Use the complete function to generate a harmonic, send it to the DSP, receive the results and calculate the gain
                    float res = m_IDA.SendHarmonicAndReceiveResultGain(func, ch, a_in, freq, Fs, sendBufSize);
                    // Add the frequency and gain to the result lists, and print them
                    freqs.Add(freq);
                    gain.Add(res);
                    OutputWrapper.WriteLine("F: " + freq + "\t\t - G: " + res + "dB");
                }
                // Close the connection to the DSP and other services
                macros.Close();
            }
        }
    }
}
