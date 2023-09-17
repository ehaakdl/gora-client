using System.Collections.Concurrent;
using System.Net.Sockets;
using System;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private static Socket clientTcpSocket = null;
    private static NetworkManager _instance = null;
    public static NetworkManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
                _instance.connectTcp();

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    // 어떤 곳에서도 비동기 호출 가능 해야한다.
    // 실패시 콜백함수로 알림 받을 수 있어야 한다.
    // TCP, UDP 지원
    public void send(NetworkInfo networkInfo)
    {
        if (networkInfo.getProtocol() == 1)
        {
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(tcpIoCompleted);
            byte[] sendPacket = NetworkPacket.convertToByteArray(networkInfo.getPacket());
            socketAsyncEventArgs.SetBuffer(sendPacket, 0, sendPacket.Length);
            clientTcpSocket.SendAsync(socketAsyncEventArgs);
        }
        else
        {
            // udp
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

    public NetworkPacket startRecv()
    {
        while (true)
        {
            // udp Receive

            // tcp Receive
            SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(tcpIoCompleted);
            clientTcpSocket.ReceiveAsync(socketAsyncEventArgs);
        }
    }

    void tcpIoCompleted(object sender, SocketAsyncEventArgs e)
    {
        // determine which type of operation just completed and call the associated handler
        switch (e.LastOperation)
        {
            case SocketAsyncOperation.Receive:
                recvTcpProcess(e);
                break;
            case SocketAsyncOperation.Send:
                sendTcpProcess(e);
                break;
            default:
                throw new ArgumentException("The last operation completed on the socket was not a receive or send");
        }
    }

    private void recvTcpProcess(SocketAsyncEventArgs socketAsyncEventArgs)
    {
        if (socketAsyncEventArgs.BytesTransferred > 0 && socketAsyncEventArgs.SocketError == SocketError.Success)
        {
            byte[] recvBytes = new byte[1024];
            socketAsyncEventArgs.SetBuffer(recvBytes, socketAsyncEventArgs.Offset, socketAsyncEventArgs.BytesTransferred);
            // 라우팅 처리
        }
        else
        {
            closeClientTcpSocket(socketAsyncEventArgs);
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
    public void connectTcp()
    {
        //클라이언트에서 사용할 소켓 준비
        clientTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //접속할 서버의 통신지점(목적지)
        string ip = Environment.GetEnvironmentVariable("SERVER_TCP_IP");
        IPAddress serverIPAdress = IPAddress.Parse(ip);
        int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_TCP_PORT"));
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
