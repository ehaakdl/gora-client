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
    //???? ?????? ???????? ???????? ???? ????
    private NetworkManager networkmanager;
    private NetworkInfo networkinfo;

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

    

    void messegeTest(string typing)
    {

        NetworkPacket Messege = MessegeName(EServiceType.Test, typing);
        Debug.Log(typing);
        //Debug.Log(System.Text.Encoding.UTF8.GetBytes(typing).Length);
        networkinfo = new NetworkInfo(NetworkProtocolType.tcp, Messege);
    }

    public static NetworkPacket MessegeName(EServiceType type, string Messege)
    {

        ByteString bytesrting = ByteString.CopyFromUtf8(Messege);

        Test sendmessege = new Test { Msg = bytesrting };
        Debug.Log(bytesrting);
        byte[] data = sendmessege.ToByteArray();
        string identify = NetworkUtils.GenerateIdentify();
        List<NetworkPacket> packets = NetworkUtils.GenerateSegmentPacket(data, type, identify, data.Length, NetworkUtils.UDP_EMPTY_CHANNEL_ID);
        // ?? ???? 1500??? ???? ??? ???? ????
        Debug.Log(packets[0].GetType());
        return packets[0];
    }
}
