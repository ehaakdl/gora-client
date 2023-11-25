
using System.Net.Sockets;
using System.Text;
using Google.Protobuf;
using NetowrkServiceType;
using Protobuf;

StringBuilder builder = new StringBuilder();

string uuid = Guid.NewGuid().ToString();
for (int j = 0; j < 50; j++)
{
    builder.Append(uuid);
}




byte[] byteArray = Encoding.UTF8.GetBytes(builder.ToString());
Test test = new Test
{
    Msg = ByteString.CopyFrom(byteArray)
};

List<NetworkPacket> packets = NetworkUtils.getSegment(test.ToByteArray(), EServiceType.Test, NetworkUtils.GetIdentify());

TcpClient client = new TcpClient("127.0.0.1", 11200);
NetworkStream stream = client.GetStream();
foreach (NetworkPacket packet in packets)
{
    Console.WriteLine($"바이트 배열 크기: {packet.ToByteArray().Length}");
    stream.Write(packet.ToByteArray());
}



client.Close();