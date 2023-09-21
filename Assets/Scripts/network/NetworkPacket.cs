using System;
using Newtonsoft.Json;
using System.Text;

[Serializable]
public class NetworkPacket
{
    public int type{ get; set; }
    public object data { get; set; }
    public string key { get; set; }
    //데이터 마지막을 구분짓는다
    public static string EOF = "@@@";

    public static byte[] convertToByteArray(NetworkPacket packet)
    {
        string packetToJson = JsonConvert.SerializeObject(packet) + EOF;
        return Encoding.UTF8.GetBytes(packetToJson);
    }

    public static NetworkPacket convertToNetworkPacket(byte[] bytes)
    {
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<NetworkPacket>(json);
    }
}