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
        Test test = Test.Parser.ParseFrom(transportDatas[0].data);
        string recvMsgByServer= Encoding.Default.GetString(test.Msg.ToByteArray());
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
    }

}
