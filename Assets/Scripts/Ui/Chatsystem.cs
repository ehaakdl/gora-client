using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protobuf;
using Google.Protobuf;

public class Chatsystem : MonoBehaviour
{
    //���� �޼��� ���ڴ��� �극��ũ �ɸ� ����
    private NetworkManager networkmanager;
    private NetworkInfo networkinfo;
    // Start is called before the first frame update
    void Start()
    {
        messegeTest();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Escape))
        //{
        //    Debug.Log(networkmanager);
        //    Debug.Log(networkinfo);
        //    NetworkManager.Instance.send(networkinfo);
        //}
        
    }

    void messegeTest()
    {
        NetworkPacket Messege = MessegeName("test");
        networkinfo = new NetworkInfo(NetworkProtocolType.tcp , Messege);
    }

    public static NetworkPacket MessegeName(string Messege)
    {
        ByteString bytesrting = ByteString.CopyFromUtf8(Messege);
        return new NetworkPacket
        {
            Data = bytesrting
        };
    }
}
