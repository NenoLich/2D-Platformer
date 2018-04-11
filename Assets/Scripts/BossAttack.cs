using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour {

    public AudioClip shootClip;
    public Rigidbody2D bullet;
    public float bulletSpeed = 50f;
    public float offsetX = 0.7f;
    public float offsetY = 0.3f;
    public float repeatDamagePeriod;
    
    private Animator anim;
    private float lastHitTime;
    private AudioSource audioSource;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (lastHitTime > Time.time - repeatDamagePeriod)
            return;

        anim.SetTrigger("Fire");

        audioSource.clip = shootClip;

        for (int i = 0; i < 4; i++)
        {
            Invoke("BulletQueue",i*0.05f);
        }

        lastHitTime = Time.time;
    }

    private void BulletQueue()
    {
        audioSource.Play();
        float multiplier = Mathf.Sign(transform.localScale.x);
        Rigidbody2D bulletInstance = Instantiate(bullet,
            transform.position + new Vector3(offsetX * multiplier, offsetY, 0f), Quaternion.Euler(new Vector3(0, 0, -90f * multiplier))) as Rigidbody2D;
        bulletInstance.velocity = new Vector2(bulletSpeed * multiplier, 0);
    }
}
