using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject explosion;
    public float speed=20f;

    private Rigidbody2D rigBody;

	void Start ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Enemy")
        {
            collision.GetComponent<Enemy>().Hurt();

            Hit();
        }

        else
        {
            if (collision.tag!="Player")
            {
                Hit();
            }
        }
    }
    private void Hit()
    {
        Instantiate(explosion,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
