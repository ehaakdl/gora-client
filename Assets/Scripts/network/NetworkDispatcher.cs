using System.Threading;

public class NetworkDispatcher
{
    public static NetworkPacket playerCoordinatePacket { get; set; }
    private static NetworkDispatcher _instance = null;

    public static NetworkDispatcher Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (_instance == null)
            {
                _instance = new NetworkDispatcher();
            }

            return _instance;
        }
    }

    public static void Run()
    {
        while (true)
        {
            NetworkManager.Instance.send(new NetworkInfo(NetworkProtocolType.udp, playerCoordinatePacket));
            Thread.Sleep(200);
        }
    }
}
