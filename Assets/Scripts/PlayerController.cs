using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    private ChatManager chatmanager;
    public GameObject ChatObj;

    private Vector3 _clickPos = Vector3.zero;
    private Camera _mainCam;

    void Start()
    {
        animator = GetComponent<Animator>();
        ChatObj = GameObject.Find("Chat");
        chatmanager = ChatObj.GetComponent<ChatManager>();
        _mainCam = Camera.main;
    }
    public float speed = 2;

    void Update()
    {
        //Debug.Log(chatmanager);
        //Debug.Log(chatmanager.ChatInputField);
        //Debug.Log(chatmanager.ChatInputField.isFocused);
        if (!chatmanager.ChatInputField.isFocused)
        {
            if (Input.GetMouseButton(0))
            {
                _clickPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                _clickPos.z = 0;
            }
            MoveCharacter();
        }
        else
        {
            animator.SetBool("IsMove", false);
        }

    }
    void MoveCharacter()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _clickPos = transform.position + new Vector3(horizontalInput, verticalInput, 0);
        }

        var moveDirection = _clickPos - transform.position;
        var isMove = moveDirection.magnitude > 0.1f;

        animator.SetBool("IsMove", isMove);
        
        if (!isMove) return;

        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        transform.localScale = new Vector3(moveDirection.x > 0 ? -1 : 1, 1);

        moveDirection = moveDirection.normalized;

        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
