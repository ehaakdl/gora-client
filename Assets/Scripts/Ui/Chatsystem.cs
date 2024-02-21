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
using UnityEngine.EventSystems;
using System.Text;

public class Chatsystem : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public static Chatsystem Instance { get; private set; }
    //???? ?????? ???????? ???????? ???? ????
    private NetworkManager networkmanager;
    private NetworkInfo networkinfo;

    public TMP_InputField ChatInputField;

    // ä��â ����
    public bool State_Active; //���� Ȱ��ȭ ���� (����ڰ� ���� ä���� ��� ���� ����)
    public bool State_Inactive; //���� ��Ȱ��ȭ ���� (ä��â�� ������ ���� ��)
    public bool State_Standby; // ��� ���� (����ڰ� ä���� ġ�� �ʰ� ������ ���ο� �������� ���� ����)
    public bool IsMouseOver;

    public Transform[] ChildObj;

    //public TMP_InputField ChatInputField;

    private float StansdBytime;

    //���� ä�� �迭
    public List<GameObject> receivechatArr;
    GameObject chatreceive;

    GameObject ChatContent;

    public string RecvMsg;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChatInputField = GetComponentInChildren<TMP_InputField>();

        State_Active = false;
        State_Inactive = true;
        State_Standby = false;
        IsMouseOver = false;

        ChildObj = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i);
            ChildObj[i] = transform.GetChild(i);
            Debug.Log(transform.childCount);
            Debug.Log(transform.GetChild(i));
        }

        ChatInputField = GetComponentInChildren<TMP_InputField>();
        //Debug.Log(ChatInputField);

        //ChatScreen = obj;
        ChatContent = ChildObj[0].GetComponentInChildren<GridLayoutGroup>().gameObject;

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return) && ChatInputField.text.Length != 0)
        {
            messegeTest(ChatInputField.text);
            //Debug.Log(networkmanager);
            Debug.Log(networkinfo.packet);
            Debug.Log(networkinfo.packet.Data.Length);
            NetworkManager.Instance.send(networkinfo);
            ChatInputField.text = null;
        }

        ChatState();
        InactiveTrigger(5);
        InputFieldTrigger();

        string data;

        // ť���� ������ �������� �õ�
        if (NetworkDispatcher.Instance.TryDequeueChatData(out data))
        {
            // ������ ó��
            //OnTcpDataReceived(data);
            ChatReceiveArray(data);

        }
        //Debug.Log(StansdBytime);
        //if (RecvMsg != null)
        //{
        //    Debug.Log(RecvMsg);
        //    ChatReceiveArray(RecvMsg);
        //    RecvMsg = null;
        //    //ChatManager.Instance.ChatReceiveArray2(ChildObj);
        //    ChatContentSize();
        //}
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


    // ���콺�� UI�� �ö� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        State_Active = true;
        State_Standby = false;
        IsMouseOver = true;
    }

    // ���콺�� UI�� �������� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        State_Standby = true;
        State_Active = false;
        StansdBytime = 0;
        IsMouseOver = false;
    }

    // �Է��ʵ� Ȱ��ȭ �� ������
    void InputFieldTrigger()
    {
        //���� Ű(����) �Է½�
        if (Input.GetKey(KeyCode.Return))
        {
            //isFocused Ȱ��ȭ <-> ��Ȱ��ȭ
            ChatInputField.Select();
            //Debug.Log(ChatInputField.isFocused);
        }

        //isFocused�� ä��â Ȱ��ȭ ����
        if (ChatInputField.isFocused)
        {
            State_Standby = false;
            State_Active = true;
        }
        //�ƴϸ� isFocused�� �ƴϰ� ���콺���� ���°� �ƴҶ�
        else if (!ChatInputField.isFocused && !IsMouseOver && !State_Inactive)
        {
            State_Active = false;
            State_Standby = true;
        }

    }

    //��Ȱ��ȭ Ʈ����
    void InactiveTrigger(float Inactivetime)
    {

        //Debug.Log(State_Standby);
        if (State_Standby == true)
        {
            StansdBytime = StansdBytime + Time.deltaTime;
            //Debug.Log(StansdBytime);
        }
        if (StansdBytime > Inactivetime && State_Active != true)
        {
            //Debug.Log("TriggerOn");
            State_Standby = false;
            State_Inactive = true;
            StansdBytime = 0;
        }
    }

    //Chat UI ���� ����
    void ChatState()
    {
        if (State_Active == true)
        {
            State_Inactive = false;
            ChatActive(190);
            gameObject.GetComponent<Image>().raycastTarget = true;
            //State_Active = false;
        }

        if (State_Standby == true)
        {
            ChatStandby(80);
        }

        if (State_Inactive == true)
        {
            ChatInactive(0);
            gameObject.GetComponent<Image>().raycastTarget = false;
            //State_Inactive = false;
        }
    }

    // Chat UI Ȱ��ȭ
    void ChatActive(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // ä��â ��� 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // ä��â �� �ؽ�Ʈ
        TextMeshProUGUI[] ChatText;
        ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < ChatText.Length; i++)
        {
            Color TextAlpha;
            TextAlpha = ChatText[i].color;
            TextAlpha.a = (AlphaVelue + 65) / 255f;
            ChatText[i].color = TextAlpha;
            //Debug.Log(ChatText[i]);
        }

        // ä��â �� �Է�â
        Image ChatInputFild = ChildObj[1].GetComponent<Image>();
        Color ChatInputFildalpha = ChatInputFild.color;
        ChatInputFildalpha.a = AlphaVelue / 255f;
        ChatInputFild.color = ChatInputFildalpha;
        //Debug.Log(ChatInputFild.color.a);
        TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
        Color InputFildTMPColor1 = InputFildTMP[0].color;
        Color InputFildTMPColor2 = InputFildTMP[1].color;
        InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
        InputFildTMPColor2.a = AlphaVelue / 255f;
        InputFildTMP[0].color = InputFildTMPColor1;
        InputFildTMP[1].color = InputFildTMPColor2;

        // ä��â �� ê Ÿ��
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;



        // ä��â �� ��ũ�ѹ�
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //��ũ�ѹ� �� �ڵ鷯
        Image ChatScrollbarHandler;
        ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
        Color ChatScrollbarHandleralpha;
        ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
        ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
        ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
        //Debug.Log(ChatScrollbarHandler);
        //Debug.Log(ChatScrollbar.colors.normalColor);
    }

    // Chat UI ������
    void ChatStandby(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // ä��â ��� 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // ä��â �� �ؽ�Ʈ
        TextMeshProUGUI[] ChatText;
        ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < ChatText.Length; i++)
        {
            Color TextAlpha;
            TextAlpha = ChatText[i].color;
            TextAlpha.a = (AlphaVelue + 35) / 255f;
            ChatText[i].color = TextAlpha;
            //Debug.Log(ChatText[i]);
        }

        // ä��â �� �Է�â
        Image ChatInputFild = ChildObj[1].GetComponent<Image>();
        Color ChatInputFildalpha = ChatInputFild.color;
        ChatInputFildalpha.a = AlphaVelue / 255f;
        ChatInputFild.color = ChatInputFildalpha;
        //Debug.Log(ChatInputFild.color.a);
        TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
        Color InputFildTMPColor1 = InputFildTMP[0].color;
        Color InputFildTMPColor2 = InputFildTMP[1].color;
        InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
        InputFildTMPColor2.a = AlphaVelue / 255f;
        InputFildTMP[0].color = InputFildTMPColor1;
        InputFildTMP[1].color = InputFildTMPColor2;

        // ä��â �� ê Ÿ��
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;

        // ä��â �� ��ũ�ѹ�
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //��ũ�ѹ� �� �ڵ鷯
        Image ChatScrollbarHandler;
        ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
        Color ChatScrollbarHandleralpha;
        ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
        ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
        ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
        //Debug.Log(ChatScrollbarHandler);
        //Debug.Log(ChatScrollbar.colors.normalColor);
    }

    // Chat UI ��Ȱ��ȭ
    void ChatInactive(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // ä��â ��� 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // ä��â �� �ؽ�Ʈ
        TextMeshProUGUI[] ChatText;
        ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < ChatText.Length; i++)
        {
            Color TextAlpha;
            TextAlpha = ChatText[i].color;
            TextAlpha.a = (AlphaVelue) / 255f;
            ChatText[i].color = TextAlpha;
            //Debug.Log(ChatText[i]);
        }

        // ä��â �� �Է�â
        Image ChatInputFild = ChildObj[1].GetComponent<Image>();
        Color ChatInputFildalpha = ChatInputFild.color;
        ChatInputFildalpha.a = AlphaVelue / 255f;
        ChatInputFild.color = ChatInputFildalpha;
        //Debug.Log(ChatInputFild.color.a);
        TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
        Color InputFildTMPColor1 = InputFildTMP[0].color;
        Color InputFildTMPColor2 = InputFildTMP[1].color;
        InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
        InputFildTMPColor2.a = AlphaVelue / 255f;
        InputFildTMP[0].color = InputFildTMPColor1;
        InputFildTMP[1].color = InputFildTMPColor2;


        // ä��â �� ê Ÿ��
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;

        // ä��â �� ��ũ�ѹ�
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //��ũ�ѹ� �� �ڵ鷯
        Image ChatScrollbarHandler;
        ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
        Color ChatScrollbarHandleralpha;
        ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
        ChatScrollbarHandleralpha.a = (AlphaVelue) / 255f;
        ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
        //Debug.Log(ChatScrollbarHandler);
        //Debug.Log(ChatScrollbar.colors.normalColor);
    }


    // Chat Receive Array
    public void ChatReceiveArray(string receiveMsg)
    {
        chatreceive = Instantiate(ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject, ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject.GetComponentsInParent<Transform>()[1]);
        chatreceive.GetComponent<TextMeshProUGUI>().text = receiveMsg;
        receivechatArr.Add(chatreceive);
        ChatContentSize();
    }

    public void RecvData()
    {
        string data;

        // ť���� ������ �������� �õ�
        if (NetworkDispatcher.Instance.TryDequeueChatData(out data))
        {
            // ������ ó��
            //OnTcpDataReceived(data);
            ChatReceiveArray(data);

        }

    }

    //ä�� ����Ʈ ������ ũ��
    void ChatContentSize()

    {
        RectTransform ContenteTr = ChatContent.GetComponent<RectTransform>();
        if (((receivechatArr.Count) + 1) * 20 < 119)
        {
            ContenteTr.sizeDelta = new Vector2(ContenteTr.sizeDelta.x, 111.83f);
            ChildObj[0].GetComponentInChildren<Scrollbar>().value = 0;
        }
        else
        {
            Debug.Log(receivechatArr.Count);
            ContenteTr.sizeDelta = new Vector2(ContenteTr.sizeDelta.x, (((receivechatArr.Count) + 1) * 20));
            ChildObj[0].GetComponentInChildren<Scrollbar>().value = -0.001f;
        }
    }


}


//// ä��â ����
//public bool State_Active; //���� Ȱ��ȭ ���� (����ڰ� ���� ä���� ��� ���� ����)
//public bool State_Inactive; //���� ��Ȱ��ȭ ���� (ä��â�� ������ ���� ��)
//public bool State_Standby; // ��� ���� (����ڰ� ä���� ġ�� �ʰ� ������ ���ο� �������� ���� ����)
//bool IsMouseOver;

//public Transform[] ChildObj;

//public TMP_InputField ChatInputField;

//private float StansdBytime;

////���� ä�� �迭
//public List<GameObject> receivechatArr;
//GameObject chatreceive;

//GameObject ChatContent;

//void Start()
//{
//    State_Active = false;
//    State_Inactive = true;
//    State_Standby = false;
//    IsMouseOver = false;

//    ChildObj = new Transform[transform.childCount];
//    for (int i = 0; i < transform.childCount; i++)
//    {
//        transform.GetChild(i);
//        ChildObj[i] = transform.GetChild(i);
//        Debug.Log(transform.childCount);
//        Debug.Log(transform.GetChild(i));
//    }

//    ChatInputField = GetComponentInChildren<TMP_InputField>();
//    //Debug.Log(ChatInputField);

//    //ChatScreen = obj;
//    ChatContent = ChildObj[0].GetComponentInChildren<GridLayoutGroup>().gameObject;


//}


//void Update()
//{
//    ChatState();
//    InactiveTrigger(5);
//    InputFieldTrigger();
//    //Debug.Log(StansdBytime);
//    //if (Input.GetKeyDown(KeyCode.Return))
//    //{
//    //    ChatReceiveArray("test");
//    //    ChatContentSize();
//    //}
//}

//// ���콺�� UI�� �ö� �� ȣ��
//public void OnPointerEnter(PointerEventData eventData)
//{
//    State_Active = true;
//    State_Standby = false;
//    IsMouseOver = true;
//}

//// ���콺�� UI�� �������� �� ȣ��
//public void OnPointerExit(PointerEventData eventData)
//{
//    State_Standby = true;
//    State_Active = false;
//    StansdBytime = 0;
//    IsMouseOver = false;
//}

//// �Է��ʵ� Ȱ��ȭ �� ������
//void InputFieldTrigger()
//{
//    //���� Ű(����) �Է½�
//    if (Input.GetKey(KeyCode.Return))
//    {
//        //isFocused Ȱ��ȭ <-> ��Ȱ��ȭ
//        ChatInputField.Select();
//        //Debug.Log(ChatInputField.isFocused);
//    }

//    //isFocused�� ä��â Ȱ��ȭ ����
//    if (ChatInputField.isFocused)
//    {
//        State_Standby = false;
//        State_Active = true;
//    }
//    //�ƴϸ� isFocused�� �ƴϰ� ���콺���� ���°� �ƴҶ�
//    else if (!ChatInputField.isFocused && !IsMouseOver && !State_Inactive)
//    {
//        State_Active = false;
//        State_Standby = true;
//    }

//}

////��Ȱ��ȭ Ʈ����
//void InactiveTrigger(float Inactivetime)
//{

//    //Debug.Log(State_Standby);
//    if (State_Standby == true)
//    {
//        StansdBytime = StansdBytime + Time.deltaTime;
//        //Debug.Log(StansdBytime);
//    }
//    if (StansdBytime > Inactivetime && State_Active != true)
//    {
//        //Debug.Log("TriggerOn");
//        State_Standby = false;
//        State_Inactive = true;
//        StansdBytime = 0;
//    }
//}

////Chat UI ���� ����
//void ChatState()
//{
//    if (State_Active == true)
//    {
//        State_Inactive = false;
//        ChatActive(190);
//        //State_Active = false;
//    }

//    if (State_Standby == true)
//    {
//        ChatStandby(80);
//    }

//    if (State_Inactive == true)
//    {
//        ChatInactive(0);
//        //State_Inactive = false;
//    }
//}

//// Chat UI Ȱ��ȭ
//void ChatActive(int AlphaVelue)
//{
//    // childObj - 0 : ChatScreen 
//    //            1 : InputFild
//    //            2 : ChatType

//    // ä��â ��� 
//    Image ChatScreen = ChildObj[0].GetComponent<Image>();
//    Color ChatScreenalpha = ChatScreen.color;
//    ChatScreenalpha.a = AlphaVelue / 255f;
//    ChatScreen.color = ChatScreenalpha;
//    //Debug.Log(ChatScreen.color.a);

//    // ä��â �� �ؽ�Ʈ
//    TextMeshProUGUI[] ChatText;
//    ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
//    for (int i = 0; i < ChatText.Length; i++)
//    {
//        Color TextAlpha;
//        TextAlpha = ChatText[i].color;
//        TextAlpha.a = (AlphaVelue + 65) / 255f;
//        ChatText[i].color = TextAlpha;
//        //Debug.Log(ChatText[i]);
//    }

//    // ä��â �� �Է�â
//    Image ChatInputFild = ChildObj[1].GetComponent<Image>();
//    Color ChatInputFildalpha = ChatInputFild.color;
//    ChatInputFildalpha.a = AlphaVelue / 255f;
//    ChatInputFild.color = ChatInputFildalpha;
//    //Debug.Log(ChatInputFild.color.a);
//    TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
//    Color InputFildTMPColor1 = InputFildTMP[0].color;
//    Color InputFildTMPColor2 = InputFildTMP[1].color;
//    InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
//    InputFildTMPColor2.a = AlphaVelue / 255f;
//    InputFildTMP[0].color = InputFildTMPColor1;
//    InputFildTMP[1].color = InputFildTMPColor2;

//    // ä��â �� ê Ÿ��
//    Image ChatType = ChildObj[2].GetComponent<Image>();
//    Color ChatTypealpa = ChatType.color;
//    ChatTypealpa.a = AlphaVelue / 255f;
//    ChatType.color = ChatTypealpa;
//    //Debug.Log(ChatType.color.a);
//    TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
//    Color ChatTypeTMPColor = ChatTypeTMP.color;
//    ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
//    ChatTypeTMP.color = ChatTypeTMPColor;



//    // ä��â �� ��ũ�ѹ�
//    Scrollbar ChatScrollbar;
//    ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
//    Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
//    Color ChatScrollbarImgAlpha;
//    ChatScrollbarImgAlpha = ChatScrollbarImg.color;
//    ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
//    ChatScrollbarImg.color = ChatScrollbarImgAlpha;

//    //��ũ�ѹ� �� �ڵ鷯
//    Image ChatScrollbarHandler;
//    ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
//    Color ChatScrollbarHandleralpha;
//    ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
//    ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
//    ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
//    //Debug.Log(ChatScrollbarHandler);
//    //Debug.Log(ChatScrollbar.colors.normalColor);
//}

//// Chat UI ������
//void ChatStandby(int AlphaVelue)
//{
//    // childObj - 0 : ChatScreen 
//    //            1 : InputFild
//    //            2 : ChatType

//    // ä��â ��� 
//    Image ChatScreen = ChildObj[0].GetComponent<Image>();
//    Color ChatScreenalpha = ChatScreen.color;
//    ChatScreenalpha.a = AlphaVelue / 255f;
//    ChatScreen.color = ChatScreenalpha;
//    //Debug.Log(ChatScreen.color.a);

//    // ä��â �� �ؽ�Ʈ
//    TextMeshProUGUI[] ChatText;
//    ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
//    for (int i = 0; i < ChatText.Length; i++)
//    {
//        Color TextAlpha;
//        TextAlpha = ChatText[i].color;
//        TextAlpha.a = (AlphaVelue + 35) / 255f;
//        ChatText[i].color = TextAlpha;
//        //Debug.Log(ChatText[i]);
//    }

//    // ä��â �� �Է�â
//    Image ChatInputFild = ChildObj[1].GetComponent<Image>();
//    Color ChatInputFildalpha = ChatInputFild.color;
//    ChatInputFildalpha.a = AlphaVelue / 255f;
//    ChatInputFild.color = ChatInputFildalpha;
//    //Debug.Log(ChatInputFild.color.a);
//    TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
//    Color InputFildTMPColor1 = InputFildTMP[0].color;
//    Color InputFildTMPColor2 = InputFildTMP[1].color;
//    InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
//    InputFildTMPColor2.a = AlphaVelue / 255f;
//    InputFildTMP[0].color = InputFildTMPColor1;
//    InputFildTMP[1].color = InputFildTMPColor2;

//    // ä��â �� ê Ÿ��
//    Image ChatType = ChildObj[2].GetComponent<Image>();
//    Color ChatTypealpa = ChatType.color;
//    ChatTypealpa.a = AlphaVelue / 255f;
//    ChatType.color = ChatTypealpa;
//    //Debug.Log(ChatType.color.a);
//    TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
//    Color ChatTypeTMPColor = ChatTypeTMP.color;
//    ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
//    ChatTypeTMP.color = ChatTypeTMPColor;

//    // ä��â �� ��ũ�ѹ�
//    Scrollbar ChatScrollbar;
//    ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
//    Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
//    Color ChatScrollbarImgAlpha;
//    ChatScrollbarImgAlpha = ChatScrollbarImg.color;
//    ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
//    ChatScrollbarImg.color = ChatScrollbarImgAlpha;

//    //��ũ�ѹ� �� �ڵ鷯
//    Image ChatScrollbarHandler;
//    ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
//    Color ChatScrollbarHandleralpha;
//    ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
//    ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
//    ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
//    //Debug.Log(ChatScrollbarHandler);
//    //Debug.Log(ChatScrollbar.colors.normalColor);
//}

//// Chat UI ��Ȱ��ȭ
//void ChatInactive(int AlphaVelue)
//{
//    // childObj - 0 : ChatScreen 
//    //            1 : InputFild
//    //            2 : ChatType

//    // ä��â ��� 
//    Image ChatScreen = ChildObj[0].GetComponent<Image>();
//    Color ChatScreenalpha = ChatScreen.color;
//    ChatScreenalpha.a = AlphaVelue / 255f;
//    ChatScreen.color = ChatScreenalpha;
//    //Debug.Log(ChatScreen.color.a);

//    // ä��â �� �ؽ�Ʈ
//    TextMeshProUGUI[] ChatText;
//    ChatText = ChildObj[0].GetComponentsInChildren<TextMeshProUGUI>();
//    for (int i = 0; i < ChatText.Length; i++)
//    {
//        Color TextAlpha;
//        TextAlpha = ChatText[i].color;
//        TextAlpha.a = (AlphaVelue) / 255f;
//        ChatText[i].color = TextAlpha;
//        //Debug.Log(ChatText[i]);
//    }

//    // ä��â �� �Է�â
//    Image ChatInputFild = ChildObj[1].GetComponent<Image>();
//    Color ChatInputFildalpha = ChatInputFild.color;
//    ChatInputFildalpha.a = AlphaVelue / 255f;
//    ChatInputFild.color = ChatInputFildalpha;
//    //Debug.Log(ChatInputFild.color.a);
//    TextMeshProUGUI[] InputFildTMP = ChildObj[1].GetComponentsInChildren<TextMeshProUGUI>();
//    Color InputFildTMPColor1 = InputFildTMP[0].color;
//    Color InputFildTMPColor2 = InputFildTMP[1].color;
//    InputFildTMPColor1.a = (AlphaVelue - 60) / 255f;
//    InputFildTMPColor2.a = AlphaVelue / 255f;
//    InputFildTMP[0].color = InputFildTMPColor1;
//    InputFildTMP[1].color = InputFildTMPColor2;


//    // ä��â �� ê Ÿ��
//    Image ChatType = ChildObj[2].GetComponent<Image>();
//    Color ChatTypealpa = ChatType.color;
//    ChatTypealpa.a = AlphaVelue / 255f;
//    ChatType.color = ChatTypealpa;
//    //Debug.Log(ChatType.color.a);
//    TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
//    Color ChatTypeTMPColor = ChatTypeTMP.color;
//    ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
//    ChatTypeTMP.color = ChatTypeTMPColor;

//    // ä��â �� ��ũ�ѹ�
//    Scrollbar ChatScrollbar;
//    ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
//    Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
//    Color ChatScrollbarImgAlpha;
//    ChatScrollbarImgAlpha = ChatScrollbarImg.color;
//    ChatScrollbarImgAlpha.a = (AlphaVelue) / 255f;
//    ChatScrollbarImg.color = ChatScrollbarImgAlpha;

//    //��ũ�ѹ� �� �ڵ鷯
//    Image ChatScrollbarHandler;
//    ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
//    Color ChatScrollbarHandleralpha;
//    ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
//    ChatScrollbarHandleralpha.a = (AlphaVelue) / 255f;
//    ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
//    //Debug.Log(ChatScrollbarHandler);
//    //Debug.Log(ChatScrollbar.colors.normalColor);
//}


//// Chat Receive Array
//void ChatReceiveArray(string receiveMsg)
//{
//    chatreceive = Instantiate(ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject, ChildObj[0].GetComponentInChildren<TextMeshProUGUI>().gameObject.GetComponentsInParent<Transform>()[1]);
//    chatreceive.GetComponent<TextMeshProUGUI>().text = receiveMsg;
//    receivechatArr.Add(chatreceive);
//}

////ä�� ����Ʈ ������ ũ��
//void ChatContentSize()
//{
//    RectTransform ContenteTr = ChatContent.GetComponent<RectTransform>();
//    if ((receivechatArr.Count) * 20 < 111.83)
//    {
//        ContenteTr.sizeDelta = new Vector2(ContenteTr.sizeDelta.x, 111.83f);
//    }
//    else
//    {
//        ContenteTr.sizeDelta = new Vector2(ContenteTr.sizeDelta.x, (receivechatArr.Count) * 20);
//        ChildObj[0].GetComponentInChildren<Scrollbar>().value = 0;
//    }
//}