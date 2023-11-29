using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

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
            password = passwordField.text,
        };
        HttpResponseMessage response = await userApi.login(loginReq);

        string responseJson = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            CommonResponse commonResponse = JsonConvert.DeserializeObject<CommonResponse>(responseJson);
            UserAuthRepository.Instance.accessToken = (string)commonResponse.data;
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("login fail");
        }

    }

    void Start()
    {
        userApi = new UserApi();
        loginButton.onClick.AddListener(onClick);

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
            loginButton.onClick.Invoke();
        }
    }
}
