using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D bullet;
    public float speed = 20f;
    public AudioClip shootClip;

    private PlayerController playerCtrl;
    private Animator anim;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        playerCtrl = GetComponent<PlayerController>();
    }
	
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");

            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = shootClip;
            audio.Play();

            if (playerCtrl.facingRight)
            {
                Rigidbody2D bulletInstance = Instantiate(bullet, transform.position+new Vector3(0.7f,0f,0f), Quaternion.Euler(new Vector3(0, 0, -90f))) as Rigidbody2D;
                //bulletInstance.velocity = new Vector2(speed, 0);
            }
            else
            {
                Rigidbody2D bulletInstance = Instantiate(bullet, transform.position + new Vector3(-0.7f, 0f, 0f), Quaternion.Euler(new Vector3(0, 0, 90f))) as Rigidbody2D;
                //bulletInstance.velocity = new Vector2(-speed, 0);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Melee");

            RaycastHit2D[] raycastHits = Physics2D.LinecastAll(transform.position,
                   new Vector3(playerCtrl.facingRight ? transform.position.x + 1f : transform.position.x - 1f,
                   transform.position.y, transform.position.z), LayerMask.GetMask("Enemies"));

            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                raycastHit.collider.GetComponent<Enemy>().Hurt();
            }
        }
    }
}