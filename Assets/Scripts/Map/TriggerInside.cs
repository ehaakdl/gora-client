using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInside : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player = new PlayerController();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // 씬에서 플레이어 오브젝트를 찾아서 참조합니다.
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
            // 트리거가 작동해야 하는 동작 수행
            player.GetComponent<SpriteRenderer>().sortingLayerName = "House";
            player.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌이 끝난 경우
            player.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
    }
}
