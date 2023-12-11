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
        //while (!GameManager.isQuit)
        //{

        //    NetworkPacket packet = NetworkUtils.GetEmptyData(NetowrkServiceType.EServiceType.Test, NetworkUtils.GetIdentify());
        //    NetworkInfo networkInfo = new NetworkInfo(
        //        NetworkProtocolType.tcp,
        //        packet
        //    );
        //    NetworkManager.Instance.send(networkInfo);
        //    Thread.Sleep(3000);
        //    networkInfo = new NetworkInfo(
        //        NetworkProtocolType.udp,
        //        packet
        //    );
        //    NetworkManager.Instance.send(networkInfo);
        //    Thread.Sleep(3000);
        //}
    }
}
