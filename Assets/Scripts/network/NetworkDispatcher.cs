using System;
using System.Threading;

public class NetworkDispatcher
{
    public static NetworkPacket playerCoordinatePacket{ get; set; }
    private static readonly Lazy<NetworkDispatcher> instance =
    new Lazy<NetworkDispatcher>(() => new NetworkDispatcher());
    public static NetworkDispatcher Instance
    {
        get
        {
            return instance.Value;
        }
    }

    public static void Dispatcher()
    {
        while (!GameManager.isQuit)
        {
            if(playerCoordinatePacket == null)
            {
                continue;
            }
            NetworkManager.Instance.send(new NetworkInfo(NetworkProtocolType.udp, playerCoordinatePacket));
            Thread.Sleep(200);
        }
    }
}
