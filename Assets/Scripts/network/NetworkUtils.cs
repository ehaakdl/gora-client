﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using NetowrkServiceType;
using Protobuf;
using static UnityEngine.GraphicsBuffer;

class NetworkUtils
{
    public const byte PAD = 0; // 패딩 바이트 값 설정
    public const int DATA_MAX_SIZE = 1421;
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

    public static NetworkPacket GetPacket(byte[] target, EServiceType type)
    {
        if (target == null)
        {
            return null;
        }

        int dataSize = target.Length;
        if (dataSize >= DATA_MAX_SIZE)
        {
            throw new Exception();
        }

        int paddingSize;
        if (dataSize < DATA_MAX_SIZE)
        {
            paddingSize = DATA_MAX_SIZE - dataSize;
        }
        else
        {
            paddingSize = 0;
        }

        if (paddingSize > 0)
        {
            target = AddPadding(target, paddingSize);
        }

        if (target.Length != DATA_MAX_SIZE)
        {
            throw new Exception();
        }
        return new NetworkPacket
        {

            Data = ByteString.CopyFrom(target),
            ChannelId = UDP_EMPTY_CHANNEL_ID,
            DataSize = (uint)dataSize,
            Type = (uint)type,
        };
    }


    public static NetworkPacket GetEmptyData(EServiceType type)
    {
        byte[] newBytes = AddPadding(null, NetworkUtils.DATA_MAX_SIZE);
        return new NetworkPacket
        {

            Data = ByteString.CopyFrom(newBytes),
            ChannelId = UDP_EMPTY_CHANNEL_ID,
            DataSize = (uint)0,
            Type = (uint)type,
        };
    }

    public static NetworkPacket GetEmptyData(EServiceType type, String udpChannelId)
    {
        byte[] newBytes = AddPadding(null, DATA_MAX_SIZE);
        return new NetworkPacket
        {

            Data = ByteString.CopyFrom(newBytes),
            ChannelId = udpChannelId,
            DataSize = (uint)0,
            Type = (uint)type,
        };
    }
}