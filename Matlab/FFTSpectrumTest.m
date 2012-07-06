clear;
try
    % Load the .NET assemblies
    MacrosAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCComm\Environment\Macros\bin\Debug\Macros.dll');

    % Instantiate connectors etc.
    macros = Macros.MacroController('DK-XP-7QWWH2J', 1337, 'C:/src/instrumentation/Dsp/KISS/KissXi/VTS/Debug/VTS.out', 3, true, true);
    % Open connection
    macros.Open();

    echo on

    % Set parameters
    samples = 8192;
    Fs = 48000;

    macros.RandomAnalysis.SingleChannelConfiguration(0, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL, 1.0, Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_SINGLE, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    macros.RandomAnalysis.AlgorithmTestConfig(Shared.Constants.ALGORITHMTEST.ALGORITHMTEST_FFT);

    %replyItem = macros.RandomAnalysis.SendHarmonic(0, samples, Fs, 10000);
    %dataItem = Shared.Items.FloatData(replyItem);
    signal = macros.RandomAnalysis.SendHarmonic(0, samples, Fs, 10000);

    %amplitude = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(dataItem.Signal);
    amplitude = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(signal);

    % Close the connection to the DSP
    macros.Close();

    indexes = 0:1:(samples-1);
    indexes = indexes * Fs/(2*samples);

    plot(indexes, amplitude);
catch ME
    % Try to close the connection to the DSP Server gracefully
    try
        macros.Close();
    catch ME2
    end
    % Make sure the cause of the Exception is printed
    rethrow(ME)
end