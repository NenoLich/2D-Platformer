using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
	public AudioClip[] ouchClips;

    private PlayerController playerControl;		
	private Animator anim;						

    void Awake ()
	{
		playerControl = GetComponent<PlayerController>();
		anim = GetComponent<Animator>();
    }

	public void TakeDamage (int damageAmount)
	{
        health = health - damageAmount < 0 ? 0 : health - damageAmount;

		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);

        if (health == 0f)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = true;
            }

            playerControl.enabled = false;

            anim.SetTrigger("Die");
        }
    }

    public void Restore()
    {
        health = maxHealth;
    }
}
