using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using System.Text;

public class ChatManager : MonoBehaviour
{
    private static readonly Lazy<ChatManager> instance = new Lazy<ChatManager>(() => new ChatManager());

    public string receiveMsg;

    public static ChatManager Instance
    {
        get
        {
            return instance.Value;
        }
    }

    // Chat Receive Array
    public void ChatReceiveArray(GameObject chatreceive,List<GameObject> receivechatArr)
    {
        chatreceive.GetComponent<TextMeshProUGUI>().text = receiveMsg;
        receivechatArr.Add(chatreceive);
        Debug.Log(chatreceive.name);
    }
}
