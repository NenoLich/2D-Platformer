using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public float thumbSize = 10f;
    public AudioClip[] ouchClips;
    public GUIStyle healthSliderStyle;
    public GUIStyle healthThumbStyle;
    public GUIStyle healthSliderBackgroundStyle;
    public GameObject hat;

    private SpriteRenderer spriteRenderer;
    private Texture healthSliderBackgroundTexture;
    private PlayerHealth playerHealth;
    private float health;
    private float maxHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSliderBackgroundTexture = new Texture();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        maxHealth = playerHealth.maxHealth;
    }

    private void Update()
    {
        if (playerHealth.health==0)
            onDeath();
    }

    private void OnGUI()
    {
        health = playerHealth.health;
        Rect sliderRect = new Rect(Screen.width - Screen.width * 0.2f, Screen.height * 0.05f, Screen.width * 0.15f, Screen.height * 0.03f);
        GUI.HorizontalSlider(sliderRect, health, 0f, maxHealth, healthSliderStyle, healthThumbStyle);
        if (health < maxHealth)
        {
            GUI.Box(new Rect(sliderRect.xMin + sliderRect.width * health / maxHealth + thumbSize-thumbSize* health / maxHealth, sliderRect.yMin,
            sliderRect.width - sliderRect.width * health / maxHealth - thumbSize+thumbSize * health / maxHealth, sliderRect.height),
            healthSliderBackgroundTexture, healthSliderBackgroundStyle);
        }
    }

    public void onDeath()
    {
        enabled = false;
        hat.SetActive(true);
    }
}
