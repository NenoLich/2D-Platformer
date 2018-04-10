using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject explosion;
    public float speed=20f;

	void Start ()
    {
        Destroy(gameObject, 2f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                collision.GetComponent<Enemy>().Hurt(1);
                break;
            case "Player":
                collision.GetComponent<PlayerHealth>().TakeDamage(10);
                break;
            case "Boss":
                collision.GetComponent<Boss>().Hurt(1);
                break;
            case "Trap":
                return;
        }

        Hit();
        
    }
    private void Hit()
    {
        Instantiate(explosion,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
