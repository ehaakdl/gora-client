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
    public Button signUpButton;
    public Selectable firstInputField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    private UserApi userApi;

    public async void LogIn()
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
            //SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("login fail");
        }

    }

    public async void SignUp()
    {
        LoginRequest loginReq = new LoginRequest
        {
            email = emailField.text,
            password = passwordField.text,
        };
        HttpResponseMessage response = await userApi.SignUp(loginReq);

        string responseJson = await response.Content.ReadAsStringAsync();
        Debug.Log(response.StatusCode);
        Debug.Log(responseJson.Replace(",", ",\n"));
        if (response.StatusCode == HttpStatusCode.OK)
        {
            CommonResponse commonResponse = JsonConvert.DeserializeObject<CommonResponse>(responseJson);
            Debug.Log("SignUp Ok");
        }
        else
        {
            Debug.Log("SignUp fail");
        }

    }

    void Start()
    {
        userApi = new UserApi();
        loginButton.onClick.AddListener(LogIn);
        signUpButton.onClick.AddListener(SignUp);

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
    public void GoogleLogin()
    {
        var urlBase = "https://accounts.google.com/o/oauth2/v2/auth?scope=email%20profile&response_type=code&state=security_token%3D138r5719ru3e1%26url%3Dhttps%3A%2F%2Foauth2.example.com%2Ftoken";
        var urlRedirection = "redirect_uri=http://localhost:8080/";
        var urlID = "client_id=361182045625-mn689vkqc2ukaavtg8l5q547gt5h9q1p.apps.googleusercontent.com";
        Application.OpenURL(urlBase+"&"+urlRedirection+"&"+urlID);
    }
}
