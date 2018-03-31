using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int HP = 2;
    public Material deadEnemy;
    public GameObject hundredPointsUI;


    private SpriteRenderer ren;
    private Transform frontCheck;
    private bool dead = false;
    private Score score;
    private GameObject player;
    private bool isMovingRight;
    private float distanceToTarget;
    private Animator anim;

    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        frontCheck = transform.Find("frontCheck").transform;
        score = GameObject.Find("Score").GetComponent<Score>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);	
    }

    private void Update()
    {
        if (dead || player==null)
            return;

        distanceToTarget = transform.position.x - player.transform.position.x;
        anim.SetFloat("distanceToTarget", distanceToTarget);

        if (Mathf.Abs(distanceToTarget) > 0.7f)
        {
            isMovingRight = distanceToTarget < 0;
            transform.Translate(isMovingRight ? moveSpeed * Time.deltaTime : -moveSpeed * Time.deltaTime, 0f, 0f);
            if (isMovingRight != transform.localScale.x > 0)
                Flip();
        }

        if (HP <= 0 && !dead)
            Death();
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Hurt()
    {
        HP--;
        GetComponent<AudioSource>().Play();
    }

    void Death()
    {
        ren.material = deadEnemy;
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
