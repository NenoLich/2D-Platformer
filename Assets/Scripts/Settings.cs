using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    public float volume=100;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
