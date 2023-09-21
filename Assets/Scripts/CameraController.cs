using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;

    public GameObject player;

    float minX, maxX, minY, maxY;
    private void Start()
    {
        minY = -Camera.main.orthographicSize;
        maxY = Camera.main.orthographicSize;
        minX = -Camera.main.orthographicSize * Camera.main.aspect;
        maxX = Camera.main.orthographicSize * Camera.main.aspect;
    }
    private void Update()
    {
            Vector3 cameraToPlayerDiff = player.transform.position - this.transform.position;
            Vector3 cameraPos= new Vector3(cameraToPlayerDiff.x * cameraSpeed * Time.deltaTime, cameraToPlayerDiff.y * cameraSpeed * Time.deltaTime, 0.0f);
            this.transform.Translate(cameraPos);
    }

    void LateUpdate()
    {
        //LimitToMove();
    }


    void LimitToMove()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                                         Mathf.Clamp(transform.position.y, minY, maxY));
    }
}