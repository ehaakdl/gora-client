using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ChatManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ä��â ����
    public bool State_Active; //���� Ȱ��ȭ ���� (����ڰ� ���� ä���� ��� ���� ����)
    public bool State_Inactive; //���� ��Ȱ��ȭ ���� (ä��â�� ������ ���� ��)
    public bool State_Standby; // ��� ���� (����ڰ� ä���� ġ�� �ʰ� ������ ���ο� �������� ���� ����)
    bool IsMouseOver;

    public Transform[] ChildObj;

    public TMP_InputField ChatInputField;

    private float StansdBytime;

    void Start()
    {
        State_Active = false;
        State_Inactive = true;
        State_Standby = false;
        IsMouseOver = false;

        ChildObj = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i);
            ChildObj[i] = transform.GetChild(i);
            //Debug.Log(transform.childCount);
            //Debug.Log(transform.GetChild(i));
        }

        ChatInputField = GetComponentInChildren<TMP_InputField>();
        //Debug.Log(ChatInputField);

        //ChatScreen = obj;
    }


    void Update()
    {
        ChatState();
        InactiveTrigger(5);
        InputFieldTrigger();
        //Debug.Log(StansdBytime);
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
            Debug.Log(ChatInputField.isFocused);
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
            //State_Active = false;
        }

        if (State_Standby == true)
        {
            ChatStandby(80);
        }

        if (State_Inactive == true)
        {
            ChatInactive(0);
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
}
