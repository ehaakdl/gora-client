using Protobuf;
using System;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;
using Google.Protobuf;
using System.Text;
using System.Collections.Generic;

public class NetworkDispatcher
{
    private static readonly Lazy<NetworkDispatcher> instance =
    new Lazy<NetworkDispatcher>(() => new NetworkDispatcher());

    //1. 모든 수신 패킷 모으기
    //2. 데이터 router
    //3. 채팅 패이징

    // 라우팅 테이블
    private Dictionary<NetowrkServiceType.EServiceType, Action<TransportData>> routingTable;



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
            RecvData();
        }
    }

    private NetworkDispatcher()
    {
        // 라우팅 테이블 초기화
        routingTable = new Dictionary<NetowrkServiceType.EServiceType, Action<TransportData>>();
        //routingTable["character_position"] = OnCharacterPositionReceived;
        //routingTable["character_health"] = OnCharacterHealthReceived;
        routingTable[NetowrkServiceType.EServiceType.Test] = OnChatReceived;
    }

    private void OnTcpDataReceived(List<TransportData> data)
    {
        // 데이터를 이용한 작업 수행

        //data.ForEach(transportData =>
        //{
        //    //router(transportData);

        //});
        //Debug.Log($"Received data from TCP: {Encoding.UTF8.GetString(data)}");

        //RouteData(data);
    }


    public void RecvData()
    {
        List<TransportData> data;

        // 큐에서 데이터 가져오기 시도
        if (NetworkBufferManager.Instance.TryDequeueTcpData(out data))
        {
            // 데이터 처리
            //OnTcpDataReceived(data);
            RouteData(data);

        }

    }

    //public void router(TransportData data)
    //{
    //    if (data.type == NetowrkServiceType.EServiceType.Test)
    //    {
    //        Test test = Test.Parser.ParseFrom(data.data);
    //        string recvMsgByServer = Encoding.Default.GetString(test.Msg.ToByteArray());
    //        UnityEngine.Debug.Log($"받은 데이터 {test.Msg.ToStringUtf8()}");
    //        ChatManager.Instance.RecvMsg = recvMsgByServer;
    //    }
    //}


    // 수신한 데이터를 라우팅하는 메서드
    private void RouteData(TransportData data)
    {
        if (routingTable.ContainsKey(data.type))
        {
            routingTable[data.type].Invoke(data);
        }
        else
        {
            Debug.LogError($"No routing rule found for data type: {data.type}");
        }
    }

    // 캐릭터 위치 데이터를 처리하는 메서드
    private void OnCharacterPositionReceived(TransportData data)
    {
        // 캐릭터 위치 데이터 처리
        Debug.Log("Received character position data: " + Encoding.UTF8.GetString(data.data));
    }

    // 캐릭터 체력 데이터를 처리하는 메서드
    private void OnCharacterHealthReceived(TransportData data)
    {
        // 캐릭터 체력 데이터 처리
        Debug.Log("Received character health data: " + Encoding.UTF8.GetString(data.data));
    }

    // 채팅 데이터를 처리하는 메서드
    private void OnChatReceived(TransportData data)
    {
        // 채팅 데이터 처리
        Debug.Log("Received chat data: " + Encoding.UTF8.GetString(data.data));
        Test test = Test.Parser.ParseFrom(data.data);
        string recvMsgByServer = Encoding.Default.GetString(test.Msg.ToByteArray());
        UnityEngine.Debug.Log($"받은 데이터 {test.Msg.ToStringUtf8()}");
        EnqueueChatData(recvMsgByServer);
    }

    // 데이터를 받아 라우팅하는 메서드
    public void RouteData(List<TransportData> dataList)
    {
        foreach (var data in dataList)
        {
            RouteData(data);
        }
    }


    private Queue<string> ChatDataQueue = new Queue<string>();

    private void EnqueueChatData(string data)
    {
        lock (ChatDataQueue)
        {
            ChatDataQueue.Enqueue(data);
        }
    }

    public bool TryDequeueChatData(out string data)
    {
        lock (ChatDataQueue)
        {
            if (ChatDataQueue.Count > 0)
            {
                data = ChatDataQueue.Dequeue();
                return true;
            }
        }

        data = null;
        return false;
    }
}
