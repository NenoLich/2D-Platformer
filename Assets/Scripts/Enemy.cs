using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 20f;
    public int damage = 10;
    public float minDistanceToTarget=0.7f;
    public float repeatDamagePeriod = 2f;

    private SpriteRenderer ren;
    
    private GameObject player;
    private float distanceToTarget;
    private Animator anim;
    private Rigidbody2D rigBody;
    private float lastHitTime;
    private EnemyHealth enemyHealth;


    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        rigBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.Death += OnDeath;
    }

    void FixedUpdate()
    {
        if (enemyHealth.dead || player == null)
            return;

        distanceToTarget = transform.position.x - player.transform.position.x;

        if (Mathf.Abs(distanceToTarget) > minDistanceToTarget)
        {
            rigBody.velocity = new Vector2(transform.localScale.x * moveSpeed, rigBody.velocity.y);
            if (distanceToTarget < 0 != transform.localScale.x > 0)
                enemyHealth.Flip();
        }

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x), transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null && lastHitTime <= Time.time-repeatDamagePeriod)
            Attack();
    }

    private void Update()
    {
        anim.SetFloat("distanceToTarget", distanceToTarget);
    }

    void OnDeath()
    {
        ren.material.color = Color.black;

        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        lastHitTime = Time.time;
    }
}
