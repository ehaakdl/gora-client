
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using NetowrkServiceType;
using Protobuf;

class NetworkUtils
{
    public const byte PAD = 0; // 패딩 바이트 값 설정
    public const int DATA_MAX_SIZE = 1377;
    public static int TOTAL_MAX_SIZE = 1500;
    public static string UDP_EMPTY_CHANNEL_ID = "0000000000000000000000000000000000000000000000000000000000000000";

    public static string? GetLocalIpAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static byte[] RemovePadding(byte[] target, int paddingSize)
    {
        if (paddingSize <= 0)
        {
            return target;
        }
        else if (target.Length < paddingSize)
        {
            throw new Exception("Padding size is greater than the length of the target array.");
        }
        else if (target.Length == paddingSize)
        {
            return target;
        }

        int lastLength = target.Length - paddingSize;
        byte[] result = new byte[lastLength];
        Array.Copy(target, 0, result, 0, lastLength);
        return result;
    }

    public static byte[] AddPadding(byte[]? original, int blockSize)
    {
        if (original == null)
        {
            return new byte[blockSize]; // 모든 요소가 PAD(0)으로 초기화됨
        }

        int padLength = blockSize;
        byte[] padded = new byte[original.Length + padLength];
        Array.Copy(original, padded, original.Length); // 배열 복사

        for (int i = original.Length; i < padded.Length; i++)
        {
            padded[i] = PAD; // 나머지 부분을 PAD로 채움
        }

        return padded;
    }
    public static string GenerateIdentify()
    {
        Guid uuid = Guid.NewGuid();
        string uuidString = uuid.ToString().Replace("-", "");
        return new string(uuidString);
    }

    public static NetworkPacket GetEmptyData(EServiceType type)
    {
        byte[] newBytes = AddPadding(null, DATA_MAX_SIZE);
        return new NetworkPacket
        {
            Data = ByteString.CopyFrom(newBytes),
            Sequence = 0,
            Identify = GenerateIdentify(),
            DataSize = 0,
            TotalSize = 0,
            ChannelId = UDP_EMPTY_CHANNEL_ID,
            Type = (uint)type,
        };
    }

    public static NetworkPacket GetEmptyData(EServiceType type, string udpChannelId)
    {
        byte[] newBytes = AddPadding(null, DATA_MAX_SIZE);
        return new NetworkPacket
        {
            Data = ByteString.CopyFrom(newBytes),
            Sequence = 0,
            Identify = GenerateIdentify(),
            DataSize = 0,
            TotalSize = 0,
            ChannelId = udpChannelId,

            Type = (uint)type,
        };
    }

    private static int CalcPaddingSize(int totalSize)
    {
        int paddingSize;
        if (totalSize < DATA_MAX_SIZE)
        {
            paddingSize = DATA_MAX_SIZE - totalSize;
        }
        else
        {
            paddingSize = totalSize % DATA_MAX_SIZE > 0 ?
                DATA_MAX_SIZE - totalSize % DATA_MAX_SIZE :
                totalSize % DATA_MAX_SIZE;
        }

        return paddingSize;
    }

    private static NetworkPacket CreateNetworkPacket(byte[] data, int dataSize, int totalSize, string identify,
            string udpChannelId, int sequence, EServiceType serviceType)
    {
        return new NetworkPacket
        {
            Data = ByteString.CopyFrom(data),
            DataSize = (uint)dataSize,
            TotalSize = (uint)totalSize,
            Type = (uint)serviceType,
            ChannelId = udpChannelId,
            Sequence = (uint)sequence,
            Identify = identify

        };
    }
    
    private static List<NetworkPacket> MakePackets(byte[] target, int paddingSize, EServiceType type, int totalSize,
        string identify, string udpChannelId)
    {
        int segmentTotalCount = target.Length / DATA_MAX_SIZE;
        int srcPos = 0;

        int dataSize = DATA_MAX_SIZE;
        int sequence = 0;
        byte[] copyBytes;
        List<NetworkPacket> result = new();
        for (int index = 0; index < segmentTotalCount; index++)
        {
            if (index == segmentTotalCount - 1)
            {
                dataSize = DATA_MAX_SIZE - paddingSize;
            }
            copyBytes = new byte[DATA_MAX_SIZE];
            Array.Copy(target, srcPos, copyBytes, 0, DATA_MAX_SIZE);
            NetworkPacket packet = CreateNetworkPacket(copyBytes, dataSize, totalSize, identify, udpChannelId,
                    sequence++, type);

            result.Add(packet);
            srcPos += DATA_MAX_SIZE;
        }

        return result;
    }

    public static List<NetworkPacket> GenerateSegmentPacket(byte[] target, EServiceType type,
        string identify, int totalSize, string udpChannelId)
    {
        if (target == null)
        {
            return null;
        }

        int paddingSize = CalcPaddingSize(totalSize);
        if (paddingSize > 0)
        {
            target = AddPadding(target, paddingSize);
        }

        return MakePackets(target, paddingSize, type, totalSize, identify, udpChannelId);
    }
}