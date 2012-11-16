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
using Shared.Items;
using Macros.Helpers;
using Shared.Constants;
using Macros.Collections;
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
            uint sendBufSize = 2048;
            uint freq = 10000;

            float[] result = null;

            IMacros macros = new MacroController(
                GlobalConfiguration.host,
                GlobalConfiguration.port,
                "C:/src/instrumentation/Dsp/KISS/KissXi/6ch-gisp/Debug/6ch-gisp.out",
                GlobalConfiguration.loglevel,
                GlobalConfiguration.transferConsole,
                GlobalConfiguration.transferDebugOutput
                );
            IItemController itemController = macros.ItemController;

            // Set program to run on the DSP as well as other parameters, and configure the connection
            macros.Open();

            IM_IDA m_IDA = macros.M_IDA;
            m_IDA.Codec_Stop();

            // Configure the channel
            m_IDA.Config_Service(new M_IDA_Config_ChannelConfig[] { new M_IDA_Config_ChannelConfig(1, ch, 7, 1, filt) });
                
            // Set input amplitude
            float a_in = (float)Math.Sqrt(2);
            // Send harmonic to the DSP and receive the results

            int cnt;
            int numberOfRuns = 5;
            // Generate and send Items of the generated Sine (number depends on the repeat variable)
            for (cnt = 1; cnt < numberOfRuns + 1; cnt++)
            {
                float[] send = Generator.Sine(a_in, freq, Fs, sendBufSize);

                OutputWrapper.WriteLine("Sine max: " + send.Max(), 4);
                FloatData floatData = new FloatData(CH.CH_SP, Kind.M_IDA_DATA, new DataDescriptor((uint)0, (uint)0, 0.0f, (uint)0, 0, (uint)BitConverter.ToInt32(BitConverter.GetBytes(9.87654321), 0), new SigId(0, func, ch, 0, 0)), send);
                floatData.Info = 1;
                itemController.PutItem(floatData);
            }

            // Receive the responses from the DSP and save the last one
            for (cnt--; cnt > 0; cnt--)
            {
                Item item;

                item = itemController.GetItem(Kind.M_IDA_DATA);

                result = new FloatData(item).Signal;
            }

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
