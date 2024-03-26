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

        // �� Transform�� GameObject�� ��ȯ�Ͽ� List�� �߰��մϴ�.
        foreach (Transform childTransform in childTransforms)
        {
            // �θ� ������Ʈ�� �����մϴ�.
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
