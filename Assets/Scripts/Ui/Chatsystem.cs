using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protobuf;
using Google.Protobuf;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using NetowrkServiceType;

public class Chatsystem : MonoBehaviour
{
    //서버 메세지 디코더에 브레이크 걸면 잡힘
    private NetworkManager networkmanager;
    private NetworkInfo networkinfo;

    public const byte PAD = 0; // 패딩 바이트 값 설정
    public const int DATA_MAX_SIZE = 1487;

    TMP_InputField ChatInputField;


    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        ChatInputField = GetComponentInChildren<TMP_InputField>();




    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return) && ChatInputField.text.Length != 0)
        {
            messegeTest(ChatInputField.text);
            //Debug.Log(networkmanager);
            Debug.Log(networkinfo.packet);
            NetworkManager.Instance.send(networkinfo);
            ChatInputField.text = null;
        }
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

    void messegeTest(string typing)
    {

        NetworkPacket Messege = MessegeName(EServiceType.Test, typing);
        Debug.Log(typing);
        Debug.Log(System.Text.Encoding.UTF8.GetBytes(typing).Length);
        networkinfo = new NetworkInfo(NetworkProtocolType.tcp, Messege);
    }

    public static NetworkPacket MessegeName(EServiceType type, string Messege)
    {
        byte[] newBytes = AddPadding(System.Text.Encoding.UTF8.GetBytes(Messege), DATA_MAX_SIZE);
        ByteString bytesrting = ByteString.CopyFrom(newBytes);
        return new NetworkPacket
        {
            Data = bytesrting,
            DataSize = (uint)newBytes.Length,
            Type = (uint)type //이건 사실 뭐가 들어가는지 몰라서 TEST로 해두었습니다.
        };
    }
}
