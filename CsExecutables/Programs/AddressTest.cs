using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSPConnector;
using Shared.Interfaces;
using DSPConnector.Comm;
using Shared.Debug;
using System.Threading;
using UnitTest;

namespace Shared
{
    class AddressTest
    {
        public static void Main(String[] args)
        {
            ICommInterface commInterface = new SocketClientWithConsole(GlobalConfiguration.host, GlobalConfiguration.port);

            IDSPProxy dspProxy = new DSPProxy(
                commInterface,
                "C:/src/instrumentation/Dsp/KISS/KissXi/PerformanceTest/Debug/PerformanceTest.out",
                GlobalConfiguration.loglevel
                );

            dspProxy.Open();

            long newInput = dspProxy.GetAddress("newInput");
            long inputHeadAddr = dspProxy.GetAddress("inputHeadAddr");
            long newOutput = dspProxy.GetAddress("newOutput");
            long outputHeadAddr = dspProxy.GetAddress("outputHeadAddr");
            OutputWrapper.WriteLine("newInput: 0x" + newInput.ToString("X"));
            OutputWrapper.WriteLine("inputHeadAddr: 0x" + inputHeadAddr.ToString("X"));
            OutputWrapper.WriteLine("newOutput: 0x" + newOutput.ToString("X"));
            OutputWrapper.WriteLine("outputHeadAddr: 0x" + outputHeadAddr.ToString("X"));

            long inItemHeader;
            long inItemData;

            long outItemHeader;
            long outItemData;

            long writeReady;
//            long readReady;

            short dest;
            short reply;
            int kind;
            int size;
            int info;

            long[] data;

            // Send PerformancePrint item
            do
            {
                writeReady = dspProxy.ReadWord(0, newInput);
                OutputWrapper.Write("#");
            }
            while (writeReady != 0);
            OutputWrapper.Write("\n");

            inItemHeader = dspProxy.ReadData(0, inputHeadAddr, 32);
            dspProxy.WriteData(0, inItemHeader +  8, 0x050, 16);
            dspProxy.WriteData(0, inItemHeader + 12, 0x031, 16);
            dspProxy.WriteData(0, inItemHeader + 24, 0x1B1, 32);
            dspProxy.WriteData(0, inItemHeader + 28, 0x000, 32);
            dspProxy.WriteData(0, inItemHeader + 36, 0x000, 32);

            inItemData = dspProxy.ReadData(0, inItemHeader + 60, 32);
            OutputWrapper.WriteLine("InItemData: 0x" + inItemData.ToString("X"));

            dspProxy.WriteWord(0, newInput, 1);

//            Thread.Sleep(2000);

            // Send PerformanceMode item
            do
            {
                writeReady = dspProxy.ReadWord(0, newInput);
                OutputWrapper.Write("#");
            }
            while (writeReady != 0);
            OutputWrapper.Write("\n");

            inItemHeader = dspProxy.ReadData(0, inputHeadAddr, 32);
            dspProxy.WriteData(0, inItemHeader + 8, 0x050, 16);
            dspProxy.WriteData(0, inItemHeader + 12, 0x031, 16);
            dspProxy.WriteData(0, inItemHeader + 24, 0x1B0, 32);
            dspProxy.WriteData(0, inItemHeader + 28, 0x000, 32);
            dspProxy.WriteData(0, inItemHeader + 36, 0x001, 32);

            inItemData = dspProxy.ReadData(0, inItemHeader + 60, 32);
            OutputWrapper.WriteLine("InItemData: 0x" + inItemData.ToString("X"));

            data = new long[] { 3 };
            dspProxy.WriteData(0, inItemData, data, 32);

            dspProxy.WriteWord(0, newInput, 1);

//            Thread.Sleep(2000);

            // Send PerformancePrint item
            do
            {
                writeReady = dspProxy.ReadWord(0, newInput);
                OutputWrapper.Write("#");
            }
            while (writeReady != 0);
            OutputWrapper.Write("\n");

            inItemHeader = dspProxy.ReadData(0, inputHeadAddr, 32);
            dspProxy.WriteData(0, inItemHeader + 8, 0x050, 16);
            dspProxy.WriteData(0, inItemHeader + 12, 0x031, 16);
            dspProxy.WriteData(0, inItemHeader + 24, 0x1B1, 32);
            dspProxy.WriteData(0, inItemHeader + 28, 0x000, 32);
            dspProxy.WriteData(0, inItemHeader + 36, 0x000, 32);

            inItemData = dspProxy.ReadData(0, inItemHeader + 60, 32);
            OutputWrapper.WriteLine("InItemData: 0x" + inItemData.ToString("X"));

            dspProxy.WriteWord(0, newInput, 1);

//            Thread.Sleep(2000);

            // Send PerformanceMode item
            do
            {
                writeReady = dspProxy.ReadWord(0, newInput);
                OutputWrapper.Write("#");
            }
            while (writeReady != 0);
            OutputWrapper.Write("\n");

            inItemHeader = dspProxy.ReadData(0, inputHeadAddr, 32);
            dspProxy.WriteData(0, inItemHeader + 8, 0x050, 16);
            dspProxy.WriteData(0, inItemHeader + 12, 0x031, 16);
            dspProxy.WriteData(0, inItemHeader + 24, 0x1B0, 32);
            dspProxy.WriteData(0, inItemHeader + 28, 0x000, 32);
            dspProxy.WriteData(0, inItemHeader + 36, 0x001, 32);

            inItemData = dspProxy.ReadData(0, inItemHeader + 60, 32);
            OutputWrapper.WriteLine("InItemData: 0x" + inItemData.ToString("X"));

            data = new long[] { 2 };
            dspProxy.WriteData(0, inItemData, data, 32);

            dspProxy.WriteWord(0, newInput, 1);

//            Thread.Sleep(2000);

            // Receive items
            //readReady = dspProxy.ReadWord(0, newOutput);
            //while (readReady != 0)
            while (dspProxy.ReadWord(0, newOutput) != 0)
            {
                outItemHeader = dspProxy.ReadData(0, outputHeadAddr, 32);
                dest = (short)dspProxy.ReadData(0, outItemHeader + 8, 16);
                OutputWrapper.WriteLine("Dest: 0x" + dest.ToString("X"));
                reply = (short)dspProxy.ReadData(0, outItemHeader + 12, 16);
                OutputWrapper.WriteLine("Reply: 0x" + reply.ToString("X"));
                kind = (int)dspProxy.ReadData(0, outItemHeader + 24, 32);
                OutputWrapper.WriteLine("Kind: 0x" + kind.ToString("X"));
                size = (int)dspProxy.ReadData(0, outItemHeader + 28, 32);
                OutputWrapper.WriteLine("Size: 0x" + size.ToString("X"));
                info = (int)dspProxy.ReadData(0, outItemHeader + 36, 32);
                OutputWrapper.WriteLine("Info: 0x" + info.ToString("X"));

                outItemData = dspProxy.ReadData(0, outItemHeader + 60, 32);
                OutputWrapper.WriteLine("OutItemData: 0x" + outItemData.ToString("X"));

                if (size > 0)
                {
                    data = dspProxy.ReadData(0, outItemData, 32, (uint)size);

                    foreach (long d in data)
                    {
                        OutputWrapper.WriteLine("" + d);
                    }
                }

                dspProxy.WriteWord(0, newOutput, 0);

                //Thread.Sleep(2000);
            }

            dspProxy.Close();
        }
    }
}
