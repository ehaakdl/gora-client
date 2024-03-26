using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObj : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player = new PlayerController();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // ������ �÷��̾� ������Ʈ�� ã�Ƽ� �����մϴ�.
        Debug.Log(player);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("TriggerOn");
        if (other.CompareTag("Player"))
        {
            // Ʈ���Ű� �۵��ؾ� �ϴ� ���� ����
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� �浹�� ���� ���
        }
    }
}