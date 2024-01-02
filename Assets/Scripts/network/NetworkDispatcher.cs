using Protobuf;
using System;
using System.Threading;

public class NetworkDispatcher
{
    private static readonly Lazy<NetworkDispatcher> instance =
    new Lazy<NetworkDispatcher>(() => new NetworkDispatcher());
    public static NetworkDispatcher Instance
    {
        get
        {
            return instance.Value;
        }
    }

    public void Dispatcher()
    {
        
    }
}
