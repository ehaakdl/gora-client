using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSetting : MonoBehaviour
{

    SpriteRenderer MapSprite;
    // Start is called before the first frame update
    void Start()
    {
        MapSprite = GetComponent<SpriteRenderer>();
        MapSprite.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
