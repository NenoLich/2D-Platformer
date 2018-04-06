using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    private Rigidbody2D rigBody;

	void Start ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("Elevate",1.5f,3f);
	}
	
	void Elevate()
    {
        rigBody.AddForce(new Vector2(0f, 1000f));
    }
}
