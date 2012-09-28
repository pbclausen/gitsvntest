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
    samples = 256;
    Fs = 48000;

    %macros.RandomAnalysis.SingleChannelConfiguration(0, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL, 1.0, Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_SINGLE, 1024, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    channelTypes = NET.createArray('Shared.Constants.CHANNELTYPE', 3);
    channelTypes.Set(0, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL);
    channelTypes.Set(1, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL);
    channelTypes.Set(2, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL);
%    channelTypes.Set(3, Shared.Constants.CHANNELTYPE.CHANNELTYPE_CONTROL);

    %macros.RandomAnalysis.MultiChannelConfiguration([5, 1, 3], channelTypes, [0.7, 1.2, 0.5], Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_WEIGHTING, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    %macros.RandomAnalysis.MultiChannelConfiguration([2, 3, 5, 4], channelTypes, [0.9, 1.0, 1.5, 1.2], Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_WEIGHTING, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    macros.RandomAnalysis.MultiChannelConfiguration([0, 3, 5], channelTypes, [1.2, 0.9, 1.5], Shared.Constants.CONTROLSTRATEGY.CONTROLSTRATEGY_WEIGHTING, samples, 0, Shared.Constants.WINDOWS.WINDOWSHANNING);
    macros.RandomAnalysis.AlgorithmTestConfig(Shared.Constants.ALGORITHMTEST.ALGORITHMTEST_WEIGHTACCUMULATE);

    %macros.RandomAnalysis.SendHarmonic(1, samples, Fs, 2000);%10000);
    %%macros.RandomAnalysis.SendHarmonic(2, samples, Fs, 2000);%10000);
    %macros.RandomAnalysis.SendHarmonic(5, samples, Fs, 5000);
    %%macros.RandomAnalysis.SendHarmonic(3, samples, Fs, 3000);
    %macros.RandomAnalysis.SendHarmonic(3, samples, Fs, 3000);%7000);
    %%macros.RandomAnalysis.SendHarmonic(5, samples, Fs, 6000);%7000);
    %%macros.RandomAnalysis.SendHarmonic(4, samples, Fs, 3000);%7000);
    macros.RandomAnalysis.SendHarmonic(0, samples, Fs, 3000);
    macros.RandomAnalysis.SendHarmonic(3, samples, Fs, 6000);
    macros.RandomAnalysis.SendHarmonic(5, samples, Fs, 9000);
    replyItem = macros.ItemController.GetItem();
    dataItem = Shared.Items.FloatData(replyItem);

    amplitude = macros.RandomAnalysis.ConvertComplexToAmplitudeSpectrum(dataItem.Signal);

    % Close the connection to the DSP
    macros.Close();

    indexes = 0:1:(samples/4-1);
    indexes = indexes * Fs/samples;

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