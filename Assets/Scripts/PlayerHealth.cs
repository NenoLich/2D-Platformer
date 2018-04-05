using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;
	public AudioClip[] ouchClips;
    public GUIStyle healthSliderStyle;
    public GUIStyle healthThumbStyle;

	private PlayerController playerControl;		
	private Animator anim;						
    private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		playerControl = GetComponent<PlayerController>();
		anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    private void OnGUI()
    {
        GUI.HorizontalSlider(new Rect(Screen.width-Screen.width*0.2f, Screen.height*0.05f, Screen.width * 0.15f, Screen.height*0.03f),health,0f,100f, healthSliderStyle, healthThumbStyle);
    }

	public void TakeDamage (int damageAmount)
	{
		health -= damageAmount;

		int i = Random.Range (0, ouchClips.Length);
		AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);

        if (health < 0f)
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
}
