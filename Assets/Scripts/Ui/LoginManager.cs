using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using BCrypt.Net;

public class LoginManager : MonoBehaviour
{
    EventSystem system;
    public Button loginButton;
    public Selectable firstInputField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    private UserApi userApi;
    public async void onClick()
    {
        LoginRequest loginReq = new LoginRequest 
        { 
            email = emailField.text,
            password = passwordField.text
        };
        Debug.Log(loginReq.email);
        Debug.Log(loginReq.password);
        string dd = await userApi.login(loginReq);
        Debug.Log(dd);
    }
    void Start()
    {
        userApi = new UserApi();
        loginButton.onClick.AddListener(onClick);

        /*GameObject emailGameObj = GameObject.Find("Email");
        GameObject passwordGameObj = GameObject.Find("Password");
        emailField = emailGameObj.GetComponent<InputField>();
        passwordField = passwordGameObj.GetComponent<InputField>();
*/
        system = EventSystem.current;
        firstInputField.Select();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // 엔터키를 치면 로그인 (제출) 버튼을 클릭
            
            loginButton.onClick.Invoke();
            
        }
    }
}
