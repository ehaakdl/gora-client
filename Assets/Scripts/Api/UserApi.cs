using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class UserApi
{
    //환경변수로 참조
    private string apiUrl = "http://localhost:8080";
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<HttpResponseMessage> checkToken(string token)
    {
        string url = apiUrl + "/api/v1/user/auth/token-status";
        HttpRequestMessage request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("Authorization", token);
        var response = httpClient.SendAsync(request).Result;

        return response;
    }

    public async Task<HttpResponseMessage> login(LoginRequest loginRequest)
    {
        string url = apiUrl + "/api/v1/login";
        string json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        return response;
    }
    public async Task<HttpResponseMessage> SignUp(LoginRequest loginRequest)
    {
        string url = apiUrl + "/api/v1/signup";
        string json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        return response;
    }
}
