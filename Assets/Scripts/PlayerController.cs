using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
       
    }
    public float speed = 2;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(0f,0f,0f);
        if (horizontalInput != 0)
        {
            moveDirection = new Vector3(horizontalInput, 0f, 0f);
        }
        
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            moveDirection = new Vector3(0f, verticalInput, 0f);
        }
        
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
