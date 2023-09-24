using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class UserApi
{
    //환경변수로 참조
    private string apiUrl = "http://localhost:8080";
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<string> login(LoginRequest loginRequest)
    {
        string url = apiUrl + "/api/v1/login";

        // JSON 문자열로 직렬화합니다.
        string json = JsonConvert.SerializeObject(loginRequest);

        // POST 요청을 생성하고 데이터를 첨부합니다.

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // POST 요청을 보냅니다.
        HttpResponseMessage response = await httpClient.PostAsync(url, content);

        // 응답을 문자열로 읽습니다.
        return await response.Content.ReadAsStringAsync();
    }
}
