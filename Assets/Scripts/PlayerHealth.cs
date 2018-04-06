using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float thumbSize = 10f;
	public AudioClip[] ouchClips;
    public GUIStyle healthSliderStyle;
    public GUIStyle healthThumbStyle;
    public GUIStyle healthSliderBackgroundStyle;

    private PlayerController playerControl;		
	private Animator anim;						
    private SpriteRenderer spriteRenderer;
    private Texture healthSliderBackgroundTexture;

    void Awake ()
	{
		playerControl = GetComponent<PlayerController>();
		anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSliderBackgroundTexture = new Texture();

    }

    private void OnGUI()
    {
        Rect sliderRect=new Rect(Screen.width - Screen.width * 0.2f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.03f);
        GUI.HorizontalSlider(sliderRect, health,0f, maxHealth, healthSliderStyle, healthThumbStyle);
        if (health < maxHealth)
        {
            GUI.Box(new Rect(sliderRect.xMin + sliderRect.width * health / maxHealth + thumbSize * (1-health / maxHealth) + thumbSize, sliderRect.yMin,
            sliderRect.width - sliderRect.width* health / maxHealth - thumbSize * (1 - health / maxHealth) - thumbSize, sliderRect.height),
            healthSliderBackgroundTexture, healthSliderBackgroundStyle);
        }
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
}
