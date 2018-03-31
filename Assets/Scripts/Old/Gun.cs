using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
    public AudioClip shootClip;


	private PlayerController playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.


	void Awake()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		playerCtrl = GetComponent<PlayerController>();
	}


	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Fire1"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");

            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = shootClip;
            audio.Play();

            // If the player is facing right...
            if (playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,-90f))) as Rigidbody2D;
				//bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 90f))) as Rigidbody2D;
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
