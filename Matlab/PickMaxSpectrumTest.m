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
    channels = [0, 1, 2, 3, 4, 5];
    %weightings = [0.8, 0.9, 1.0, 1.1, 1.2, 1.3];
    weightings = [1, 1, 1, 1, 1, 1];
    frequencies = [1002, 1998, 3000, 4002, 4998, 6000];
    samples = 8192;
    Fs = 48000;

    channelTypes = NET.createArray('Shared.Constants.CHANNELTYPE', length(channels));
    for i = 1:length(channels)
        channelTypes.Set(i-1, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL);
    end

    macros.RandomAnalysis.MultiChannelConfiguration(channels, channelTypes, weightings, Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_MAXIMUM, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    macros.RandomAnalysis.AlgorithmTestConfig(Shared.Constants.ALGORITHMTEST.ALGORITHMTEST_PICKMAX);

    for i = 1:length(channels)
        macros.RandomAnalysis.SendHarmonic(channels(i), samples, Fs, frequencies(i));
    end;
    replyItem = macros.ItemController.GetItem();
    dataItem = Shared.Items.FloatData(replyItem);

    %amplitude = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(dataItem.Signal);
    amplitude = dataItem.Signal;

    % Close the connection to the DSP
    macros.Close();
    
    %indexes = 0:1:(samples/4-1);
    indexes = 0:1:(samples/2-1);
    indexes = indexes * Fs/(samples);

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