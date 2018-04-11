using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    public float liftForce = 500f;
    public float maxYPosition = 5f;
    public int platformDamage = 20;

    private Rigidbody2D rigBody;

	void Start ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        InvokeRepeating("Elevate",1.5f,3f);
	}

    private void FixedUpdate()
    {
        if (transform.position.y>maxYPosition)
        {
            rigBody.velocity = Vector2.zero;
        }
    }

    void Elevate()
    {
        rigBody.AddForce(new Vector2(0f, liftForce));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Enemy")
        {
            other.GetComponent<EnemyHealth>().Hurt(platformDamage);
        }

        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(platformDamage);
        }
    }
}
