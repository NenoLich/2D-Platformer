using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationRenderer : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Transform topPlatformtransform;
    private float offset;
    private float margin;

    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform[] transforms = transform.root.GetComponentsInChildren<Transform>();
        foreach (Transform  item in transforms)
        {
            if (item.name == "TopPlatform")
            {
                topPlatformtransform = item;
                margin = item.position.y - transform.position.y;
                break;
            }
        }
    }
	
	void Update ()
    {
        offset = topPlatformtransform.position.y - margin - transform.position.y;
        transform.Translate(new Vector3(0f, offset, 0f), Space.World);
    }
}
