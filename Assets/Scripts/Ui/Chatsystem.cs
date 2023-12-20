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
    //���� �޼��� ���ڴ��� �극��ũ �ɸ� ����
    private NetworkManager networkmanager;
    private NetworkInfo networkinfo;

    public const byte PAD = 0; // �е� ����Ʈ �� ����
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
            return new byte[blockSize]; // ��� ��Ұ� PAD(0)���� �ʱ�ȭ��
        }
        int padLength = blockSize;
        byte[] padded = new byte[original.Length + padLength];
        Array.Copy(original, padded, original.Length); // �迭 ����

        for (int i = original.Length; i < padded.Length; i++)
        {
            padded[i] = PAD; // ������ �κ��� PAD�� ä��
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
            Type = (uint)type //�̰� ��� ���� ������ ���� TEST�� �صξ����ϴ�.
        };
    }
}
