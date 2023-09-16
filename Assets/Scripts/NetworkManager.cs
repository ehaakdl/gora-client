using System.Collections.Concurrent;
using Socket;

namespace NetworkManager
{
    public class NetworkManager
    {
        private Socket clientTcpSocket = null;
        private static NetworkManager instance = null;
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkManager();
                }

                return instance;
            }
        }

        protected NetworkManager()
        {
            this.connectTcp();
        }
        // 어떤 곳에서도 비동기 호출 가능 해야한다.
        // 실패시 콜백함수로 알림 받을 수 있어야 한다.
        // TCP, UDP 지원
        public void send(NetworkInfo networkInfo)
        {
            if (networkInfo.type == 1)
            {
                SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
                socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(tcpIoCompleted);
                byte[] sendPacket = NetworkPacket.convertToByteArray(networkInfo.data);
                socketAsyncEventArgs.SetBuffer(sendPacket, 0, sendPacket.Length);
                this.clientSocket.SendAsync(sendPacket, socketAsyncEventArgs);
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
                CloseClientSocket(socketAsyncEventArgs);
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
                this.clientTcpSocket.ReceiveAsync(socketAsyncEventArgs);
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

        private void recvTcpProcess(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                byte[] recvBytes = new byte[1024];
                socketAsyncEventArgs.SetBuffer(recvBytes, e.Offset, e.BytesTransferred);
                // 라우팅 처리
            }
            else
            {
                closeClientTcpSocket(e);
            }
        }
        
        // 크리티컬 섹션 적용 해도 되는지 유니티에 적용해보고 판단하기
        private void closeClientTcpSocket(SocketAsyncEventArgs e)
        {
            lock (this.clientTcpSocket)
            {
                if (this.clientTcpSocket == null)
                {
                    return;
                }

                try
                {
                    this.clientTcpSocket.Shutdown(SocketShutdown.Send);
                }
                // throws if client process has already closed
                catch (Exception)
                {
                    // 에러메시지 남기기
                    return;
                }

                this.clientTcpSocket.Close();
            }
        }
        public void connectTcp()
        {
            //클라이언트에서 사용할 소켓 준비
            this.clientTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //접속할 서버의 통신지점(목적지)
            string ip = Environment.GetEnvironmentVariable("SERVER_TCP_IP");
            IPAddress serverIPAdress = IPAddress.Parse(ip);
            int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_TCP_PORT"));
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, port);

            //서버로 연결 요청
            try
            {
                Debug.Log("Connecting to Server");
                this.clientSocket.Connect(serverEndPoint);
            }
            catch (SocketException e)
            {
                Debug.Log("Connection Failed:" + e.Message);
            }
        }


    }
    public class NetworkInfo
    {
        // tcp==1, udp==2
        private int protocol;
        private NetworkPacket data;

        public NetworkInfo(int protocol, NetworkPacket packet)
        {
            this.protocol = protocol;
            this.packet = packet;
        }
    }
}