using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public float speed = 2;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        var isMove = horizontalInput != 0 || verticalInput != 0;
        animator.SetBool("IsMove", isMove);
        
        if (!isMove) return;
        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);
        transform.localScale = new Vector3(horizontalInput>0 ? -1 : 1, 1);

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        moveDirection = moveDirection.normalized;
        
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
