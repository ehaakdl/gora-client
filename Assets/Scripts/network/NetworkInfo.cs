public class NetworkInfo
{
    // tcp==1, udp==2
    public NetworkProtocolType protocol { get; }
    public NetworkPacket packet { get; }

    public NetworkInfo(NetworkProtocolType protocol, NetworkPacket packet)
    {
        this.protocol = protocol;
        this.packet = packet;
    }
}