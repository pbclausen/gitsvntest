clear;
try
    % Load the .NET assemblies
    DSPConnectorAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCTest\PCTest\Macros\bin\Debug\DSPConnector.dll');
    ExtendedConnectorAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCTest\PCTest\Macros\bin\Debug\ItemHandler.dll');
    MacrosAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCTest\PCTest\Macros\bin\Debug\Macros.dll');

    % Instantiate connectors etc.
    macros = Macros.MacroController('DK-W7-66VP72J', 1337, 'C:/src/instrumentation/Dsp/KISS/KissXi/VTS/Debug/VTS.out', 3, true, true);
    % Open connection
    macros.Open();

    macros.Config.StopCodec();
    
    echo on

    % Set parameters
    channel = 0;
    weighting = 1;
    samples = 1024;
    Fs = 1024;
    frequencies = 1:1:samples;
    frequencies = frequencies*Fs/(samples);

    channelType = Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL;

    macros.RandomAnalysis.SingleChannelConfiguration(channel, channelType, weighting, Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_SINGLE, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    macros.RandomAnalysis.AlgorithmTestConfig(Shared.Constants.ALGORITHMTEST.ALGORITHMTEST_FFT);

    nindexes = 0:1:(samples-1);
    indexes = nindexes * Fs/(samples);

    [a,num]=size(frequencies);
    for i = 1:num
        frequencies(i)
        replySignal = macros.RandomAnalysis.SendHarmonic(channel, samples, Fs, frequencies(i));
        spectrum = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(replySignal).double;
        amplitude(i,:) = spectrum;
    end
    
    % Close the connection to the DSP
    macros.Close();

    plot(indexes, amplitude);%, mffta);
catch ME
    % Try to close the connection to the DSP Server gracefully
    try
        macros.Close();
    catch ME2
    end
    % Make sure the cause of the Exception is printed
    rethrow(ME)
end
