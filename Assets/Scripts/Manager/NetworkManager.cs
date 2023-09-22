using System.Threading;
using System.Net.Sockets;
using System;
using System.Net;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NetworkManager
{
    private static Socket clientTcpSocket = null;
    private UdpClient listenUdp = null;
    private UdpClient connectUdp = null;
    private static NetworkManager _instance = null;
    private static int RECV_BUF_SIZE = 1024;
    private static StringBuilder tcpRecvAssemble = new StringBuilder();
    private static StringBuilder udpRecvAssemble = new StringBuilder();
    public static NetworkManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (_instance == null)
            {
                _instance = new NetworkManager();
            }

            return _instance;
        }
    }

    // 어떤 곳에서도 비동기 호출 가능 해야한다.
    // 실패시 콜백함수로 알림 받을 수 있어야 한다.
    // TCP, UDP 지원
    public void send(NetworkInfo networkInfo)
    {
        
        if ((int)networkInfo.protocol == (int)NetworkProtocolType.tcp)
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(tcpIoCompleted);
            byte[] sendPacket = NetworkPacket.convertToByteArray(networkInfo.packet);
            socketAsyncEventArgs.SetBuffer(sendPacket, 0, sendPacket.Length);
            clientTcpSocket.SendAsync(socketAsyncEventArgs);
        }
        else
        {
            byte[] sendPacket = NetworkPacket.convertToByteArray(networkInfo.packet);
            this.connectUdp.SendAsync(sendPacket, sendPacket.Length);
        }
    }
    private void sendTcpProcess(SocketAsyncEventArgs socketAsyncEventArgs)
    {
        // 실패 시 알림
        if (socketAsyncEventArgs.SocketError != SocketError.Success)
        {
            closeClientTcpSocket(socketAsyncEventArgs);
        }

    }

    public void RecvUdp()
    {
        while (!GameManager.isQuit)
        {
            Byte[] recvBuffer = new Byte[RECV_BUF_SIZE];
            // port env change
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Any, 11111);
            recvBuffer = listenUdp.Receive(ref serverIPEndPoint);
            string recvToStirng = Encoding.UTF8.GetString(recvBuffer, 0, recvBuffer.Length);
            udpRecvAssemble.Append(recvToStirng);
            int index = udpRecvAssemble.ToString().IndexOf(NetworkPacket.EOF);
            if (index < 0)
            {
                continue;
            }

            string jsonUnit = udpRecvAssemble.ToString(0, index);
            udpRecvAssemble.Remove(0, index);
            udpRecvAssemble.Remove(0, NetworkPacket.EOF.Length);
            NetworkPacket packet = JsonConvert.DeserializeObject<NetworkPacket>(jsonUnit);

            // recv object model routing 
            if ((int)packet.type == (int)ServiceType.test)
            {
                if (packet.data is not JObject)
                {
                    Debug.Log("잘못된 패킷 수신됐습니다");
                    continue;
                }

                JObject jObject = (JObject)packet.data;
                PlayerCoordinate coordinate = JsonConvert.DeserializeObject<PlayerCoordinate>(jObject.ToString());
                Debug.Log(coordinate.x + "," + coordinate.y);
            }
            Thread.Sleep(200);
        }
    }


    public void RecvTcp()
    {
        while (!GameManager.isQuit)
        {
            Byte[] recvBuffer = new Byte[RECV_BUF_SIZE];
            int recvSize = clientTcpSocket.Receive(recvBuffer, RECV_BUF_SIZE, SocketFlags.None);
            string recvToStirng = Encoding.UTF8.GetString(recvBuffer, 0, recvSize);
            tcpRecvAssemble.Append(recvToStirng);
            int index = tcpRecvAssemble.ToString().IndexOf(NetworkPacket.EOF);
            if (index < 0)
            {
                continue;
            }

            string jsonUnit = tcpRecvAssemble.ToString(0, index);
            tcpRecvAssemble.Remove(0, index);
            tcpRecvAssemble.Remove(0, NetworkPacket.EOF.Length);
            NetworkPacket packet = JsonConvert.DeserializeObject<NetworkPacket>(jsonUnit);

            // recv object model routing 
            if ((int)packet.type == (int)ServiceType.test)
            {
                if (packet.data is not JObject)
                {
                    Debug.Log("잘못된 패킷 수신됐습니다");
                    continue;
                }

                JObject jObject = (JObject)packet.data;
                PlayerCoordinate coordinate = JsonConvert.DeserializeObject<PlayerCoordinate>(jObject.ToString());
            }
            Thread.Sleep(200);
        }
    }

    void tcpIoCompleted(object sender, SocketAsyncEventArgs e)
    {
        // determine which type of operation just completed and call the associated handler
        switch (e.LastOperation)
        {
            case SocketAsyncOperation.Send:
                sendTcpProcess(e);
                break;
            default:
                throw new ArgumentException("The last operation completed on the socket was not a receive or send");
        }
    }

    // 크리티컬 섹션 적용 해도 되는지 유니티에 적용해보고 판단하기
    private void closeClientTcpSocket(SocketAsyncEventArgs e)
    {
        lock (clientTcpSocket)
        {
            if (clientTcpSocket == null)
            {
                return;
            }

            try
            {
                clientTcpSocket.Shutdown(SocketShutdown.Send);
            }
            // throws if client process has already closed
            catch (Exception)
            {
                // 에러메시지 남기기
                return;
            }

            clientTcpSocket.Close();
        }
    }

    public void ConnectUdp()
    {
        // 클라이언트 수신용 소켓
        int listenPort = 11112;
        this.listenUdp = new UdpClient(listenPort);

        //서버 송신용 소켓
        this.connectUdp = new UdpClient();
        //string ip = Environment.GetEnvironmentVariable("SERVER_UDP_IP");
        string ip = "127.0.0.1";
        //int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_UDP_PORT"));
        int serverPort = 11111;
        connectUdp.Connect(ip, serverPort);
    }

    public void ConnectTcp()
    {
        clientTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //환경변수 왜안되느지 확인
        //string ip = Environment.GetEnvironmentVariable("SERVER_TCP_IP");

        string ip = "127.0.0.1";
        IPAddress serverIPAdress = IPAddress.Parse(ip);
        //int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_TCP_PORT"));
        int port = 11200;
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, port);

        //서버로 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            clientTcpSocket.Connect(serverEndPoint);

            // send 패킷 이벤트 발생 시 호출하기, 이건 임시로 추가해둠 제,
            NetworkPacket packet = new NetworkPacket {
                data = "data",
                type = 1
            };
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }


}
