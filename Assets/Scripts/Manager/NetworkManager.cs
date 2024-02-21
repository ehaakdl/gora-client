using System.Net.Sockets;
using System;
using System.Net;
using UnityEngine;
using Google.Protobuf;
using System.Collections.Generic;

public class NetworkManager
{
    private static Socket clientTcpSocket = null;
    private UdpClient listenUdp = null;
    private UdpClient connectUdp = null;
    private static readonly Lazy<NetworkManager> instance = new Lazy<NetworkManager>(() => new NetworkManager());

    public static NetworkManager Instance
    {
        get
        {
            return instance.Value;
        }
    }

    // 어떤 곳에서도 비동기 호출 가능 해야한다.
    // 실패시 콜백함수로 알림 받을 수 있어야 한다.
    // TCP, UDP 지원
    public void send(NetworkInfo networkInfo)
    {
        if (networkInfo.protocol == NetworkProtocolType.tcp)
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(tcpIoCompleted);
            byte[] sendPacket = networkInfo.packet.ToByteArray();
            socketAsyncEventArgs.SetBuffer(sendPacket, 0, sendPacket.Length);
            clientTcpSocket.SendAsync(socketAsyncEventArgs);
        }
        else
        {
            byte[] sendPacket = networkInfo.packet.ToByteArray();
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
            Byte[] recvBuffer = new Byte[NetworkUtils.TOTAL_MAX_SIZE];
            // port env change
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Any, 11112);
            recvBuffer = listenUdp.Receive(ref serverIPEndPoint);
            NetworkBufferManager.Instance.AppendByUdp(recvBuffer);
            
            
        }
    }


    public void RecvTcp()
    {
        while (!GameManager.isQuit)
        {
            Byte[] recvBuffer = new Byte[NetworkUtils.TOTAL_MAX_SIZE];
            int recvSize = clientTcpSocket.Receive(recvBuffer, NetworkUtils.TOTAL_MAX_SIZE, SocketFlags.None);
            NetworkBufferManager.Instance.AppendByTcp(recvBuffer);
            Debug.Log(recvBuffer);
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

    public void ListenUdp()
    {
        // 클라이언트 수신용 소켓
        int listenPort = 11112;
        this.listenUdp = new UdpClient(listenPort);
    }
    public void ConnectUdp()
    {
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
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }

    
}
