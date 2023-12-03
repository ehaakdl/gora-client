using NetowrkServiceType;
using Newtonsoft.Json.Linq;
using Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

    private byte[] TcpBuffer;
    private int TcpBufferIndex;
    private byte[] UdpBuffer;
    private int UdpBufferIndex;

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
                    data = data,
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
            Test tt = Test.Parser.ParseFrom(data);

            result.Add(transportData);
        }

        return result;
    }

    public List<TransportData> AssemblePacket(NetworkProtocolType networkType)
    {
        byte[] buffer;
        if (networkType == NetworkProtocolType.tcp)
        {
            if (TcpBuffer == null)
            {
                TcpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                TcpBufferIndex = 0;
            }
            
            buffer = TcpBuffer;
        }
        else
        {
            if (UdpBuffer == null)
            {
                UdpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                UdpBufferIndex = 0;
            }
            buffer = UdpBuffer;
        }

        List<NetworkPacket> packets = new List<NetworkPacket>();
        

        if (buffer.Length >= NetworkUtils.TOTAL_MAX_SIZE)
        {
            int remainRecvByte = buffer.Length % NetworkUtils.TOTAL_MAX_SIZE;
            int assembleTotalCount = buffer.Length / NetworkUtils.TOTAL_MAX_SIZE;

            // 네트워크 패킷 클래스로 역직렬화
            byte[] convertBytes = new byte[NetworkUtils.TOTAL_MAX_SIZE];
            int to;

            for (int count = 0; count < assembleTotalCount; count++)
            {
                int from = count * NetworkUtils.TOTAL_MAX_SIZE;
                Array.Copy(buffer, from, convertBytes, 0, convertBytes.Length);
                packets.Add(NetworkPacket.Parser.ParseFrom(convertBytes));
            }

            if (remainRecvByte > 0)
            {
                int endPos = buffer.Length - remainRecvByte;
                byte[] remainBytes = new byte[remainRecvByte];
                Array.Copy(buffer, endPos, remainBytes, 0, remainBytes.Length);
                if (networkType == NetworkProtocolType.tcp)
                {
                    TcpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                    buffer = TcpBuffer;
                }
                else
                {
                    UdpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                    buffer = UdpBuffer;
                }
                
                remainBytes.CopyTo(buffer, 0);
            }
            else
            {
                if (networkType == NetworkProtocolType.tcp)
                {
                    TcpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                }
                else
                {
                    UdpBuffer = new byte[NetworkUtils.TOTAL_MAX_SIZE];
                }
            }
        }

        

        return null;
    }
    public void AppendByTcp(byte[] buffer)
    {
        if (TcpBuffer == null)
        {
            TcpBuffer = buffer;
            TcpBufferIndex = 0;
        }
        else
        {
            if(buffer.Length > TcpBuffer.Length)
            {
                // 새로할당
            }
            else
            {
                if(TcpBufferIndex + 1 + buffer.Length > TcpBuffer.Length)
                {
                    // 새로할당
                    byte[] newBuffer = new byte[TcpBufferIndex + 1 + buffer.Length];
                    TcpBuffer.CopyTo(newBuffer, 0);
                    buffer.CopyTo(newBuffer, TcpBuffer.Length);
                    TcpBuffer = newBuffer;
                }
                else
                {
                    // 기존버퍼 append
                }
            }
           
        }
        //List<TransportData> transportDatas = AssemblePacket(NetworkProtocolType.tcp);
        AssemblePacket(NetworkProtocolType.tcp);

    }

    public void AppendByUdp(byte[] buffer)
    {
        if(UdpBuffer== null)
        {
            UdpBuffer = buffer;
        }
        else
        {
            byte[] newBuffer = new byte[UdpBuffer.Length + buffer.Length];
            UdpBuffer.CopyTo(newBuffer, 0);
            buffer.CopyTo(newBuffer, UdpBuffer.Length);
            UdpBuffer = newBuffer;
        }


        //List<TransportData> transportDatas = AssemblePacket(NetworkProtocolType.udp);
        AssemblePacket(NetworkProtocolType.udp);
    }

}
