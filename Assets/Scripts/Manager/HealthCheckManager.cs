﻿using System;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class HealthCheckManager : MonoBehaviour
{
    private UserApi userApi;
    private static HealthCheckManager instance = null;
    public static HealthCheckManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        userApi = new UserApi();
    }


    private async Task checkToken()
    {
        if (UserAuthRepository.Instance.accessToken == null)
        {
            return;
        }
        HttpResponseMessage response = await userApi.checkToken(UserAuthRepository.Instance.accessToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            HttpHeaders headers = response.Headers;
            IEnumerable<string> values;
            if (headers.TryGetValues("Authorization", out values))
            {
                string token = values.First();
                UserAuthRepository.Instance.accessToken = token;
            }
            return;
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
            return;
        }
    }

    public async void Health()
    {
        while (!GameManager.isQuit)
        {
            await checkToken();
            Thread.Sleep(1000 * 60 * 3);
        }
    }
}
