
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using NetowrkServiceType;
using Protobuf;

class NetworkUtils
{
    public const byte PAD = 0; // 패딩 바이트 값 설정
    public const int DATA_MAX_SIZE = 1448;
    public static int TOTAL_MAX_SIZE = 1500;
    public int HEADER_SIZE = 659;
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

    public static byte[]? RemovePadding(byte[] target, int paddingSize)
    {
        if (target.Length < paddingSize)
        {
            throw new InvalidOperationException("Target array is smaller than padding size.");
        }
        else if (target.Length == paddingSize)
        {
            return null;
        }

        int newSize = target.Length - paddingSize;
        return target.Take(newSize).ToArray();
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
    public static string GetIdentify()
    {
        Guid uuid = Guid.NewGuid();
        string uuidString = uuid.ToString().Replace("-", "");
        DateTime now = DateTime.Now;
        int milliseconds = now.Millisecond;
        return new string(uuidString);
    }
    public static NetworkPacket GetEmptyData(EServiceType type, string identify)
    {
        byte[] newBytes = AddPadding(null, DATA_MAX_SIZE);

        ByteString dateByteString = ByteString.CopyFrom(newBytes);
        return new NetworkPacket
        {

            Data = dateByteString,
            DataSize = (uint)newBytes.Length,
            Type = (uint)type,
            Identify = identify,
            TotalSize = (uint)newBytes.Length
        };
    }

    public static List<NetworkPacket>? getSegment(byte[] target, EServiceType type,
            string identify)
    {
        List<NetworkPacket> result = new();
        byte[] newBytes;
        if (target == null)
        {
            return null;
        }

        int segmentTotalSize = target.Length;
        int paddingSize;
        if (segmentTotalSize < DATA_MAX_SIZE)
        {
            paddingSize = DATA_MAX_SIZE - segmentTotalSize;
        }
        else if (segmentTotalSize > DATA_MAX_SIZE)
        {
            int remainSize = segmentTotalSize % DATA_MAX_SIZE;
            if (remainSize > 0)
            {
                paddingSize = DATA_MAX_SIZE - remainSize;
            }
            else
            {
                paddingSize = 0;
            }
        }
        else
        {
            paddingSize = 0;
        }

        if (paddingSize > 0)
        {
            target = AddPadding(target, paddingSize);
        }

        int segmentCount = target.Length / DATA_MAX_SIZE;
        int srcPos = 0;

        int dataSize = DATA_MAX_SIZE;
        for (int index = 0; index < segmentCount; index++)
        {
            // 마지막 데이터는 패딩이 붙기 때문에 실제 데이터 사이즈를 구해준다.
            if (index == segmentCount - 1)
            {
                dataSize = DATA_MAX_SIZE - paddingSize;
            }
            newBytes = new byte[DATA_MAX_SIZE];
            Array.Copy(target, srcPos, newBytes, 0, DATA_MAX_SIZE);
            NetworkPacket packet = new NetworkPacket
            {
                Data = ByteString.CopyFrom(newBytes),
                DataSize = (uint)dataSize,
                TotalSize = (uint)segmentTotalSize,
                Type = (uint)type,
                Identify = identify
            };

            result.Add(packet);
            srcPos += DATA_MAX_SIZE;
        }

        if (result.Count == 0)
        {
            return null;
        }
        else
        {
            return result;
        }
    }
}