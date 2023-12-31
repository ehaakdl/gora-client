using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ChatManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 채팅창 상태
    public bool State_Active; //완전 활성화 상태 (사용자가 직접 채팅을 사용 중인 상태)
    public bool State_Inactive; //완전 비활성화 상태 (채팅창이 사용되지 않을 때)
    public bool State_Standby; // 대기 상태 (사용자가 채팅은 치지 않고 있지만 새로운 정보들이 들어가는 상태)
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

    // 마우스가 UI로 올라갈 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        State_Active = true;
        State_Standby = false;
        IsMouseOver = true;
    }

    // 마우스가 UI를 빠져나갈 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        State_Standby = true;
        State_Active = false;
        StansdBytime = 0;
        IsMouseOver = false;
    }

    // 입력필드 활성화 시 변경점
    void InputFieldTrigger()
    {
        //리턴 키(엔터) 입력시
        if (Input.GetKey(KeyCode.Return))
        {
            //isFocused 활성화 <-> 비활성화
            ChatInputField.Select();
            //Debug.Log(ChatInputField.isFocused);
        }

        //isFocused시 채팅창 활성화 유지
        if (ChatInputField.isFocused)
        {
            State_Standby = false;
            State_Active = true;
        }
        //아니면 isFocused가 아니고 마우스오버 상태가 아닐때
        else if (!ChatInputField.isFocused && !IsMouseOver && !State_Inactive)
        {
            State_Active = false;
            State_Standby = true;
        }

    }

    //비활성화 트리거
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

    //Chat UI 상태 구분
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

    // Chat UI 활성화
    void ChatActive(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // 채팅창 배경 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // 채팅창 내 텍스트
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

        // 채팅창 내 입력창
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

        // 채팅창 내 챗 타입
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;



        // 채팅창 내 스크롤바
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //스크롤바 내 핸들러
        Image ChatScrollbarHandler;
        ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
        Color ChatScrollbarHandleralpha;
        ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
        ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
        ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
        //Debug.Log(ChatScrollbarHandler);
        //Debug.Log(ChatScrollbar.colors.normalColor);
    }

    // Chat UI 대기상태
    void ChatStandby(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // 채팅창 배경 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // 채팅창 내 텍스트
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

        // 채팅창 내 입력창
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

        // 채팅창 내 챗 타입
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;

        // 채팅창 내 스크롤바
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue - 115) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //스크롤바 내 핸들러
        Image ChatScrollbarHandler;
        ChatScrollbarHandler = ChatScrollbar.GetComponentsInChildren<Image>()[1];
        Color ChatScrollbarHandleralpha;
        ChatScrollbarHandleralpha = ChatScrollbarHandler.color;
        ChatScrollbarHandleralpha.a = (AlphaVelue + 65) / 255f;
        ChatScrollbarHandler.color = ChatScrollbarHandleralpha;
        //Debug.Log(ChatScrollbarHandler);
        //Debug.Log(ChatScrollbar.colors.normalColor);
    }

    // Chat UI 비활성화
    void ChatInactive(int AlphaVelue)
    {
        // childObj - 0 : ChatScreen 
        //            1 : InputFild
        //            2 : ChatType

        // 채팅창 배경 
        Image ChatScreen = ChildObj[0].GetComponent<Image>();
        Color ChatScreenalpha = ChatScreen.color;
        ChatScreenalpha.a = AlphaVelue / 255f;
        ChatScreen.color = ChatScreenalpha;
        //Debug.Log(ChatScreen.color.a);

        // 채팅창 내 텍스트
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

        // 채팅창 내 입력창
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


        // 채팅창 내 챗 타입
        Image ChatType = ChildObj[2].GetComponent<Image>();
        Color ChatTypealpa = ChatType.color;
        ChatTypealpa.a = AlphaVelue / 255f;
        ChatType.color = ChatTypealpa;
        //Debug.Log(ChatType.color.a);
        TextMeshProUGUI ChatTypeTMP = ChildObj[2].GetComponentInChildren<TextMeshProUGUI>();
        Color ChatTypeTMPColor = ChatTypeTMP.color;
        ChatTypeTMPColor.a = (AlphaVelue + 65) / 255f;
        ChatTypeTMP.color = ChatTypeTMPColor;

        // 채팅창 내 스크롤바
        Scrollbar ChatScrollbar;
        ChatScrollbar = ChildObj[0].GetComponentInChildren<Scrollbar>();
        Image ChatScrollbarImg = ChatScrollbar.GetComponentsInChildren<Image>()[0];
        Color ChatScrollbarImgAlpha;
        ChatScrollbarImgAlpha = ChatScrollbarImg.color;
        ChatScrollbarImgAlpha.a = (AlphaVelue) / 255f;
        ChatScrollbarImg.color = ChatScrollbarImgAlpha;

        //스크롤바 내 핸들러
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
