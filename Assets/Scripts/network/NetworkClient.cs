using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


public class Client : SingleTonMonobehaviour<Client>
{
    private Socket socket = null;

    private void connect()
    {
        //클라이언트에서 사용할 소켓 준비
        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //접속할 서버의 통신지점(목적지)
        string ip = Environment.GetEnvironmentVariable("SERVER_TCP_IP");
        IPAddress serverIPAdress = IPAddress.Parse(ip);
        int port = Int32.Parse(Environment.GetEnvironmentVariable("SERVER_TCP_PORT"));
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, port);

        //서버로 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            this.socket.Connect(serverEndPoint);
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        connect();
    }

    private void OnApplicationQuit()
    {
        if (this.socket != null)
        {
            this.socket.Close();
            this.socket = null;
        }
    }

    public static void Send(NetworkPacket packet)
    {
        if (Client.Instance.socket == null)
        {
            return;
        }
        byte[] sendData = NetworkPacket.convertToByteArray(packet);
        byte[] prefSize = new byte[1];
        prefSize[0] = (byte)sendData.Length;    //버퍼의 가장 앞부분에 이 버퍼의 길이에 대한 정보가 있는데 이것을 
        Client.Instance.socket.Send(prefSize);    //먼저 보낸다.
        Client.Instance.socket.Send(sendData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
