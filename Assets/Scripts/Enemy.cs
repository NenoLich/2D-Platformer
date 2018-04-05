using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 20f;
    public int HP = 2;
    public int damage = 10;
    public Material deadEnemy;
    public GameObject hundredPointsUI;
    public float minDistanceToTarget=0.7f;
    public float repeatDamagePeriod = 2f;

    private SpriteRenderer ren;
    private bool dead = false;
    private Score score;
    private GameObject player;
    private float distanceToTarget;
    private Animator anim;
    private Rigidbody2D rigBody;
    private float lastHitTime;

    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        rigBody = GetComponent<Rigidbody2D>();
        score = GameObject.Find("Score").GetComponent<Score>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (dead || player == null)
            return;

        distanceToTarget = transform.position.x - player.transform.position.x;
        anim.SetFloat("distanceToTarget", distanceToTarget);

        if (Mathf.Abs(distanceToTarget) > minDistanceToTarget)
        {
            rigBody.velocity = new Vector2(transform.localScale.x * moveSpeed, rigBody.velocity.y);
            if (distanceToTarget < 0 != transform.localScale.x > 0)
                Flip();
        }

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x), transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null && lastHitTime <= Time.time-repeatDamagePeriod)
            Attack();

        if (HP <= 0 && !dead)
            Death();
    }

    private void Update()
    {
        
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        lastHitTime = Time.time;
    }

    public void Hurt(int hp)
    {
        HP-=hp;
        GetComponent<AudioSource>().Play();
    }

    void Death()
    {
        ren.material.color=Color.black;
        anim.SetTrigger("Die");

        score.score += 100;

        dead = true;

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;
        Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
    }


    public void Flip()
    {
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
