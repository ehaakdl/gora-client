using NetowrkServiceType;
using Newtonsoft.Json.Linq;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static UnityEditor.Progress;


public class NetworkBufferManager
{
    
    private static readonly Lazy<NetworkBufferManager> instance = new Lazy<NetworkBufferManager>(() => new NetworkBufferManager());
    private Queue<List<TransportData>> tcpDataQueue = new Queue<List<TransportData>>();

    public static NetworkBufferManager Instance
    {
        get
        {
            return instance.Value;
        }
    }

    private MemoryStream TcpBuffer;
    private MemoryStream UdpBuffer;

    private List<TransportData> AssembleData(List<NetworkPacket> packets)
    {
        List<TransportData> result = new List<TransportData>();
        foreach (var packet in packets)
        {
            byte[] data = packet.Data.ToByteArray();
            EServiceType? serviceType = EServiceTypeExtensions.Convert((int)packet.Type);
            if (serviceType == null)
            {
                throw new Exception();
            }
            uint dataSize = packet.DataSize;
            TransportData transportData;
            if (dataSize < NetworkUtils.DATA_MAX_SIZE)
            {
                transportData = new TransportData
                {
                    data = NetworkUtils.RemovePadding(data, NetworkUtils.DATA_MAX_SIZE - (int)dataSize),
                    type = (EServiceType)serviceType
                };
            }
            else if (dataSize > NetworkUtils.DATA_MAX_SIZE)
            {
                throw new Exception();
            }
            else
            {
                transportData = new TransportData
                {
                    data = data,
                    type = (EServiceType)serviceType
                };
            }
            result.Add(transportData);
        }

        return result;
    }

    public List<TransportData> AssemblePacket(NetworkProtocolType networkType)
    {
        MemoryStream buffer;
        if (networkType == NetworkProtocolType.tcp)
        {
            if (TcpBuffer == null)
            {
                TcpBuffer = new MemoryStream();
            }
            
            buffer = TcpBuffer;
        }
        else
        {
            if (UdpBuffer == null)
            {
                UdpBuffer = new MemoryStream();
            }
            buffer = UdpBuffer;
        }

        List<NetworkPacket> packets = new List<NetworkPacket>();
        

        if (buffer.Length >= NetworkUtils.TOTAL_MAX_SIZE)
        {
            buffer.Seek(0, SeekOrigin.Begin);

            long remainRecvByte = buffer.Length % NetworkUtils.TOTAL_MAX_SIZE;
            long assembleTotalCount = buffer.Length / NetworkUtils.TOTAL_MAX_SIZE;

            // 네트워크 패킷 클래스로 역직렬화
            byte[] convertBytes = new byte[NetworkUtils.TOTAL_MAX_SIZE];

            for (int count = 0; count < assembleTotalCount; count++)
            {
                buffer.Read(convertBytes);
                packets.Add(NetworkPacket.Parser.ParseFrom(convertBytes));
            }

            if (remainRecvByte > 0)
            {
                byte[] remainBytes = new byte[remainRecvByte];
                buffer.Read(remainBytes);
                buffer.Close();
                if (networkType == NetworkProtocolType.tcp)
                {
                    TcpBuffer = new MemoryStream();
                }
                else
                {
                    UdpBuffer = new MemoryStream();
                }
            }
            else
            {
                if (networkType == NetworkProtocolType.tcp)
                {
                    TcpBuffer = new MemoryStream();
                }
                else
                {
                    UdpBuffer = new MemoryStream();
                }
            }
        }

        

        return AssembleData(packets);
    }
    public void AppendByTcp(byte[] buffer)
    {
        if (TcpBuffer == null)
        {
            TcpBuffer = new MemoryStream(buffer);
        }
        else
        {
            TcpBuffer.Write(buffer);
        }
        List<TransportData> transportDatas = AssemblePacket(NetworkProtocolType.tcp);


        EnqueueTcpData(transportDatas);
        //sample code: 서비스 타입 보고 캐스팅만 하면 클래스로 바꿀 수 있게 셋팅해두었습니다.
        //NetworkDispatcher 에서 List<TransportDat> 이 데이터를 큐에 넣어주세요.
        //NetworkDispatcher 에서 while문으로 서비스 타입 체크하고 캐스팅 시키고 각각의 서비스 라우팅 시키면 됩니다.
        /*
        transportDatas.ForEach(transportData => {
            Test test = Test.Parser.ParseFrom(transportData.data);
            string recvMsgByServer = Encoding.Default.GetString(test.Msg.ToByteArray());
            UnityEngine.Debug.Log($"받은 데이터 {test.Msg.ToStringUtf8()}");
        });
        */

    }

    public void AppendByUdp(byte[] buffer)
    {
        if(UdpBuffer== null)
        {
            UdpBuffer = new MemoryStream(buffer);
        }
        else
        {
            UdpBuffer.Write(buffer);
        }

        List<TransportData> transportDatas = AssemblePacket(NetworkProtocolType.udp);

        int a = 1;
    }

    private void EnqueueTcpData(List<TransportData> data)
    {
        lock (tcpDataQueue)
        {
            tcpDataQueue.Enqueue(data);
        }
    }

    public bool TryDequeueTcpData(out List<TransportData> data)
    {
        lock (tcpDataQueue)
        {
            if (tcpDataQueue.Count > 0)
            {
                data = tcpDataQueue.Dequeue();
                return true;
            }
        }

        data = null;
        return false;
    }

}
