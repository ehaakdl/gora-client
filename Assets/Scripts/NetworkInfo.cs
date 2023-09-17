public class NetworkInfo
{
    // tcp==1, udp==2
    private int protocol;
    private NetworkPacket packet;

    public NetworkInfo(int protocol, NetworkPacket packet)
    {
        this.protocol = protocol;
        this.packet = packet;
    }

    public int getProtocol()
    {
        return this.protocol;
    }
    public NetworkPacket getPacket()
    {
        return this.packet;
    }
}