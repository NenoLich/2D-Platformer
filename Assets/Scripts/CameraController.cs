using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xMargin = 4f;      
    public float yMargin = 1f;      
    public float xSmooth = 0.5f;      
    public float ySmooth = 0f;      
    public Vector2 maxXAndY;        
    public Vector2 minXAndY;

    private Transform player;
    private Settings settings;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
    }

    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }


    void LateUpdate()
    {
        //AudioListener.volume = settings.volume/100;
        TrackPlayer();
    }


    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        if (CheckYMargin())
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);

    }
}

