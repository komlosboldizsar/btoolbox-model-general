using System.IO.Pipes;
using System.Security.Principal;

namespace BToolbox.OneInstance;

public class OneInstanceGuard
{

    private const string KEYWORD_ONEINSTANCE = "ONEINSTANCE";

    private static string serverId;

    public static void Set(string appName, string instanceId = null)
    {
        serverId = $"{KEYWORD_ONEINSTANCE}_{appName}";
        if (instanceId != null)
            serverId += $"_{instanceId}";
    }

    public static bool Init(bool signalOtherInstance = true)
    {
        if (serverId == null)
            throw new Exception($"Illegal state: one instance data not set with {nameof(OneInstanceGuard)}.{nameof(Set)}.");
        Mutex oneInstanceMutex = new(true, serverId, out bool mutexResult);
        if (mutexResult)
            Task.Run(OneInstanceServerTask);
        else
            SignalOtherInstanceToShow();
        return mutexResult;
    }

    private static async Task OneInstanceServerTask()
    {
        while (true)
        {
            NamedPipeServerStream pipeServer = new(serverId, PipeDirection.In);
            await pipeServer.WaitForConnectionAsync();
            using StreamReader pipeReader = new(pipeServer);
            string pipeMessage = await pipeReader.ReadLineAsync();
            if (pipeMessage == MESSAGE_SHOW)
                ShowMessageReceived?.Invoke();
            pipeServer.Close();
        }
    }

    public static void SignalOtherInstanceToShow()
    {
        using NamedPipeClientStream pipeClient = new(".", serverId, PipeDirection.Out, PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation);
        pipeClient.Connect(1000);
        StreamWriter pipeWriter = new(pipeClient) { AutoFlush = true };
        pipeWriter.WriteLine(MESSAGE_SHOW);
    }

    private const string MESSAGE_SHOW = "show";

    public delegate void ShowMessageReceivedHandler();
    public static event ShowMessageReceivedHandler ShowMessageReceived;

}