using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ChatManager : MonoBehaviour
{
    private static readonly Lazy<ChatManager> instance = new Lazy<ChatManager>(() => new ChatManager());

    public string RecvMsg;

    public static ChatManager Instance
    {
        get
        {
            return instance.Value;
        }
    }



    // Chat Receive Array
    public string ChatReceiveArray()
    {
        return RecvMsg;
    }

    // Chat Receive Array
    public void ChatReceiveArray2(Transform[] ChildObj)
    {
        GameObject chatreceive;
        chatreceive = Instantiate(ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject, ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject.GetComponentsInParent<Transform>()[1]);
        //chatreceive.GetComponent<TextMeshProUGUI>().text = receiveMsg;
        //receivechatArr.Add(chatreceive);
        Debug.Log(chatreceive.name);
    }
}
