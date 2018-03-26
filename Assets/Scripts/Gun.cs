using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.


	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.


	void Awake()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		playerCtrl = GetComponent<PlayerControl>();
	}


	void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Fire1"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();

			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 180f, 0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}

        if (Input.GetButtonDown("Fire2"))
        {
            // ... set the animator Shoot trigger parameter and play the audioclip.
            anim.SetTrigger("Melee");

            RaycastHit raycastHit = new RaycastHit();
            // If the player is facing right...

            float posX = playerCtrl.facingRight ? transform.position.x + 2f : transform.position.x - 2f;
            Physics.Linecast(transform.position,
                    new Vector3(posX, transform.position.y, transform.position.z),
                    out raycastHit, LayerMask.NameToLayer("Enemies"));

            raycastHit.collider.GetComponent<Enemy>().Hurt();
        }
    }
}
