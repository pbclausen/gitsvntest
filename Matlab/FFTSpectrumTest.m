clear;
% Load the .NET assemblies
MacrosAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCTest\PCTest\Macros\bin\Debug\Macros.dll');
    
% Instantiate connectors etc.
macros = Macros.MacroController('DK-W7-66VP72J', 1337, 'C:/src/instrumentation/Dsp/KISS/KissXi/VTS/Debug/VTS.out', 3, true, true);
% Open connection
macros.Open();

echo on

% Set parameters
samples = 8192;
Fs = 48000;

macros.RandomAnalysis.SingleChannelConfiguration(0, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL, 1.0, Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_SINGLE, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
macros.RandomAnalysis.AlgorithmTestConfig(Shared.Constants.ALGORITHMTEST.ALGORITHMTEST_FFT);

replyItem = macros.RandomAnalysis.SendHarmonic(0, samples, Fs, 10000);
dataItem = Shared.Items.FloatData(replyItem);

amplitude = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(dataItem.Signal);

% Close the connection to the DSP
macros.Close();

indexes = 0:1:(samples-1);
indexes = indexes * Fs/(2*samples);

plot(indexes, amplitude);
