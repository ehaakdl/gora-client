using System;
using Newtonsoft.Json;
using System.Text;

[Serializable]  
public class NetworkPacket      
{
    private int type;
    
    private string data;
    private string key;
    
    public NetworkPacket(int type, string key, string data){
        this.type = type;
        this.key = key;
        this.data = data;
    }

    public static byte[] convertToByteArray(NetworkPacket packet)
    {
        string packetToJson = JsonConvert.SerializeObject(packet);
        return Encoding.UTF8.GetBytes(packetToJson);
    }

    public static NetworkPacket convertToNetworkPacket(byte[] bytes)
    {
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<NetworkPacket>(json);
    }

    public object getData(){
        return this.data;
    }

}