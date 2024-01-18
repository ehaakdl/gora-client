using Protobuf;
using System;
using System.Collections.Generic;
using System.Threading;

public class NetworkDispatcher
{
    private static readonly Lazy<NetworkDispatcher> instance =
    new Lazy<NetworkDispatcher>(() => new NetworkDispatcher());
    
    public List<NetworkPacket> list = new List<NetworkPacket>();
    
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
