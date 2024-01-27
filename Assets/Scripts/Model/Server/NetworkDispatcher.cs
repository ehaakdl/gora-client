using Protobuf;
using System;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;
using Google.Protobuf;
using System.Text;

public class NetworkDispatcher
{
    private static readonly Lazy<NetworkDispatcher> instance =
    new Lazy<NetworkDispatcher>(() => new NetworkDispatcher());

    //1. 모든 수신 패킷 모으기
    //2. 데이터 router
    //3. 채팅 패이징
    public static NetworkDispatcher Instance
    {
        get
        {
            return instance.Value;
        }
    }

    public void Dispatcher()
    {
        
        while (!GameManager.isQuit)
        {
            byte[] data;

            // 큐에서 데이터 가져오기 시도
            if (NetworkManager.Instance.TryDequeueTcpData(out data))
            {
                // 데이터 처리
                OnTcpDataReceived(data);
            }
        }
    }

    private void OnTcpDataReceived(byte[] data)
    {
        // 데이터를 이용한 작업 수행
        Debug.Log($"Received data from TCP: {Encoding.UTF8.GetString(data)}");
    }


    public static void RecvData()
    {

    }

    public void RecvChat()
    {
        
    }
}
