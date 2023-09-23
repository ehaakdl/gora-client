using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserApi
{
    //환경변수로 참조
    private string apiUrl = "http://localhost:8080";
    public void login()
    {
        string url = apiUrl + "/api/v1/login";
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
        }
    }
}
