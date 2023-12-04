
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
    public const int DATA_MAX_SIZE = 1487;
    public static int TOTAL_MAX_SIZE = 1500;

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
        };
    }
}