clear;
try
    % Load the .NET assemblies
    MacrosAsm = NET.addAssembly('C:\src\instrumentation\Dsp\KISS\KissXi\PCComm\Environment\Macros\bin\Debug\Macros.dll');

    % Instantiate connectors etc.
    macros = Macros.MacroController('DK-XP-7QWWH2J', 1337, 'C:/src/instrumentation/Dsp/KISS/KissXi/6ch-gisp/Debug/6ch-gisp.out', 3, true, true);
    % Open connection
    macros.Open();

    % Get the M_IDA functions object
    m_IDA = macros.M_IDA;

    % Stop the codec on the DSP to avoid unnecessary communication and status
    % messages
    m_IDA.Codec_Stop();
    % Configure the channel
    m_IDA.Config_Service(1, 0, 7, 1, 1);

    echo on
    % Define the frequencies to test and initialize the results array
    freq = [16, 20, 25, 31.5, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 ];
    res =  zeros(size(freq));
    for i=1:length(freq)
    %   Run test and fetch results
        res(i) = m_IDA.SendHarmonicAndReceiveResultGain(1, 0, sqrt(2), freq(i), 48000, 2048, 5);
    end

    % Close the connection to the DSP
    macros.Close();

    % Output a graph with the results of the test
    semilogx(freq, res)
catch ME
    % Try to close the connection to the DSP Server gracefully
    try
        macros.Close();
    catch ME2
    end
    % Make sure the cause of the Exception is printed
    rethrow(ME)
end