
public class NetworkInfo{
    // tcp, udp
    private int networkType;
    private byte[] data;

    public NetworkInfo(int networkType, byte[] data){
        this.networkType = networkType;
        this.data = data;
    }
}