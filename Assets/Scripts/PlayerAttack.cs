using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D bullet;
    public float bulletSpeed = 50f;
    public float offset = 0.7f;
    public AudioClip shootClip;
    public GameObject trap;

    private PlayerController playerController;
    private int multiplier;
    private Animator anim;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
	
	void Update ()
    {
        multiplier = playerController.facingRight ? 1 : -1;

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");

            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = shootClip;
            audio.Play();

            Rigidbody2D bulletInstance = Instantiate(bullet, 
                transform.position + new Vector3(offset * multiplier, 0f, 0f), Quaternion.Euler(new Vector3(0, 0, -90f * multiplier))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(bulletSpeed * multiplier, 0);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Melee");

            RaycastHit2D[] raycastHits = Physics2D.LinecastAll(transform.position, 
                new Vector3(transform.position.x + multiplier, transform.position.y, transform.position.z), 
                LayerMask.GetMask("Enemies"));

            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                raycastHit.collider.GetComponent<EnemyHealth>().Hurt(2);
            }
        }

        if (Input.GetButtonDown("Trap"))
        {
            Instantiate(trap, transform.position, Quaternion.identity);
        }
    }
}