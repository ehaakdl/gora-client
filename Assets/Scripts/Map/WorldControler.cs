using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControler : MonoBehaviour
{
    public List<GameObject> WorldObj;

    // Start is called before the first frame update
    void Start()
    {
        WorldObj = new List<GameObject>(); 
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        // 각 Transform을 GameObject로 변환하여 List에 추가합니다.
        foreach (Transform childTransform in childTransforms)
        {
            // 부모 오브젝트는 제외합니다.
            if (childTransform != transform && childTransform.tag == "MapObjP")
            {
                WorldObj.Add(childTransform.gameObject);
            }
        }

        Debug.Log(WorldObj.Count);

        for (int i = 0; i < WorldObj.Count; i++)
        {
            Debug.Log("WorldObj name = " + WorldObj[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
