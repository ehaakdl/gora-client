using NetowrkServiceType;
using Newtonsoft.Json.Linq;
using Protobuf;
using System;
using System.Collections.Generic;
using System.IO;


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
    private byte[] UdpBuffer;

    private static Dictionary<string, NetworkBuffer> DataBuffers; // Define this dictionary

    private List<TransportData> AssembleData(List<NetworkPacket> packets, string resourceKey, NetworkProtocolType networkType)
    {
        List<TransportData> result = new List<TransportData>();

      

        foreach (var packet in packets)
        {
            string identify = packet.Identify;
            uint totalSize = packet.TotalSize;
            byte[] data = packet.Data.ToByteArray();
            EServiceType? serviceType = EServiceTypeExtensions.Convert((int)packet.Type);
            uint dataNonPaddingSize = packet.DataSize;
            if (DataBuffers.TryGetValue(identify, out NetworkBuffer dataBuffer))
            {
                byte[] newBytes = new byte[dataBuffer.GetBuffer().Length + data.Length];
                dataBuffer.GetBuffer().CopyTo(newBytes, 0);
                data.CopyTo(newBytes, dataBuffer.GetBuffer().Length);
                dataBuffer.SetBuffer(newBytes);
            }
            else
            {
                dataBuffer = new NetworkBuffer(data);
            }

            if (dataNonPaddingSize < NetworkUtils.DATA_MAX_SIZE)
            {
                // 세션 체크용 패킷만 데이터가 비어있을수가 있다. 그외의 서비스 패킷은 다 에러 처리
                if (dataNonPaddingSize == 0)
                {
                    if (serviceType == EServiceType.HealthCheck)
                    {
                        TransportData transportData = new TransportData
                        {
                            data
                        }
                                
                        result.add(transportData);
                        clientNetworkBuffer.removeDataWrapper(identify, networkType);
                    }
                    else
                    {
                        throw new RuntimeException();
                    }
                }
                else
                {
                    try
                    {
                        TransportData transportData = TransportData.convert(dataBuffer, data, dataNonPaddingSize,
                                dataWrapper,
                                totalSize, serviceType, identify, networkType, resourceKey, clientNetworkBuffer);
                        if (transportData != null)
                        {
                            result.add(transportData);
                            clientNetworkBuffer.removeDataWrapper(identify, networkType);
                        }
                    }
                    catch (ExpiredPacketException e)
                    {
                        clientNetworkBuffer.removeDataWrapper(identify, networkType);
                    }
                }
            }
            else if (dataNonPaddingSize > NetworkUtils.DATA_MAX_SIZE)
            {
                throw new RuntimeException();
            }
            else
            {
                try
                {
                    TransportData transportData = TransportData.convert(dataBuffer, data, dataNonPaddingSize,
                            dataWrapper,
                            totalSize, serviceType, identify, networkType, resourceKey, clientNetworkBuffer);
                    if (transportData != null)
                    {
                        result.add(transportData);
                        clientNetworkBuffer.removeDataWrapper(identify, networkType);
                    }
                }
                catch (ExpiredPacketException e)
                {
                    clientNetworkBuffer.removeDataWrapper(identify, networkType);
                }
            }
        }

        return result;
    }

    public List<TransportData> AssemblePacket(string resourceKey, NetworkProtocolType networkType, byte[] packetBytes)
    {
        byte[] buffer;
        if(networkType == NetworkProtocolType.tcp)
        {
            buffer = TcpBuffer;
        }
        else
        {
            buffer = UdpBuffer;
        }

        if (buffer == null)
        {
            return null;
        }

        List<NetworkPacket> packets = new List<NetworkPacket>();
        

        int assembleTotalCount = 0;
        if (buffer.Length >= NetworkUtils.TOTAL_MAX_SIZE)
        {
            int remainRecvByte = buffer.Length % NetworkUtils.TOTAL_MAX_SIZE;
            assembleTotalCount = buffer.Length / NetworkUtils.TOTAL_MAX_SIZE;

            // 네트워크 패킷 클래스로 역직렬화
            byte[] convertBytes = new byte[NetworkUtils.TOTAL_MAX_SIZE];
            int from = 0;
            int to;

            for (int count = 0; count < assembleTotalCount; count++)
            {
                from = count * NetworkUtils.TOTAL_MAX_SIZE;
                to = (count + 1) * NetworkUtils.TOTAL_MAX_SIZE;
                buffer.CopyTo(convertBytes, 0);
                packets.Add(NetworkPacket.Parser.ParseFrom(convertBytes));
            }

            // 역직렬화 후 남은 데이터는 추출해서 buffer reset후 다시 buffer에 추가
            // 공유자원인 버퍼 리셋은 무조건 여기서만 해야한다. 다른 스레드에서 리셋을 하면안됨(배열 인덱스 예외 발생함)
            // 리셋 하더라도 배열 값이 0으로 초기화 되기때문에 여기서만 리셋한다.
            // 자원 해제는 버퍼를 담는 맵 자체를 삭제하고 버퍼 자체는 건들지 말자.
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
        else
        {
            return AssembleData(packets, resourceKey, networkType);
        }
    }
    public void AppendByTcp(byte[] buffer)
    {
        if (TcpBuffer == null)
        {
            TcpBuffer = buffer;
        }
        else
        {
            byte[] newBuffer = new byte[TcpBuffer.Length + buffer.Length];
            TcpBuffer.CopyTo(newBuffer, 0);
            buffer.CopyTo(newBuffer, TcpBuffer.Length);
            TcpBuffer = newBuffer;
        }
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
    }

}
