using System.Collections.Concurrent;

namespace NetworkManager
{
    public class NetworkManager
    {
        // private final UdpNetwork UdpNetwork;
        private ConcurrentQueue<NetworkInfo> sendQue = new ConcurrentQueue<NetworkInfo>();
        public void push(NetworkInfo networkInfo)
        {
            this.sendQue.Enqueue(networkInfo);
        }
        public void clear() {
            this.sendQue.Clear();
        }
    }
    public class NetworkInfo
    {
        // tcp, udp
        private int networkType;
        private byte[] data;

        public NetworkInfo(int networkType, byte[] data)
        {
            this.networkType = networkType;
            this.data = data;
        }
    }
}