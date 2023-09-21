using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
public class UdpUtil
{

    public void Recv()
    {
        try
        {

            //string ip = Environment.GetEnvironmentVariable("SERVER_UDP_IP");
            string ip = "127.0.0.1";
            //int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_UDP_PORT"));
            int serverPort = 11111;
            UdpClient server= new UdpClient(serverPort);
            server.Connect(ip, serverPort);

            // Sends a message to the host to which you have connected.
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            server.Send(sendBytes, sendBytes.Length);

            // Sends a message to a different host using optional hostname and port parameters.
            
            UdpClient client= new UdpClient(listenPort);
            
            client.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", listenPort);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = server.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Console.WriteLine("This is the message you received " +
                                         returnData.ToString());
            Console.WriteLine("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());

            server.Close();
            udpClientB.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}