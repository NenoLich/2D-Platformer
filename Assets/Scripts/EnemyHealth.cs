using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour {

    public int HP;
    public GameObject PointsUI;
    public int killPoints;

    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public event Action Death;

    private Score score;
    private Animator anim;

    void Awake ()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
        anim = GetComponent<Animator>();
        Death += OnDeath;
    }
	
	void Update ()
    {
        if (HP <= 0 && !dead)
            Death();
    }

    public void Hurt(int hp)
    {
        HP -= hp;

        if (tag=="Enemy")
            GetComponent<AudioSource>().Play();
    }

    void OnDeath()
    {
        anim.SetTrigger("Die");

        score.score += killPoints;

        dead = true;

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;
        Instantiate(PointsUI, scorePos, Quaternion.identity);
    }


    public void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
