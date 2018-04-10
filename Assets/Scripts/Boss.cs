using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public Transform[] waypointsArray;
    public float moveForce;
    public float maxWalkVelosity;
    public float maxRunVelosity;
    public float minDistanceToTarget;
    public float repeatDamagePeriod;
    public float detectionRange;
    public int HP;
    public GameObject thousandPointsUI;
    public AudioClip deathScream;
    public AudioClip deathBlow;

    private LinkedListNode<Transform> currentDestination;
    private Rigidbody2D rigBody;
    private LinkedList<Transform> waypoints;
    private float lastHitTime;
    private bool isAggressive;
    private float maxVelosity;
    private Animator anim;
    private bool dead = false;
    private GameObject player;
    private Score score;
    private AudioSource audioSource;

    void Awake ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        waypoints = new LinkedList<Transform>(waypointsArray);
        currentDestination = waypoints.First;
        isAggressive = false;
        anim = GetComponent<Animator>();
        score = GameObject.Find("Score").GetComponent<Score>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (dead || player == null)
            return;

        maxVelosity = isAggressive ? maxRunVelosity : maxWalkVelosity;

        rigBody.AddForce(Vector2.right * moveForce * Mathf.Sign((currentDestination.Value.position.x - transform.position.x)));

        if (Mathf.Abs(rigBody.velocity.x) > maxVelosity)
            rigBody.velocity = new Vector2(Mathf.Sign(rigBody.velocity.x)* maxVelosity, rigBody.velocity.y);

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x)* detectionRange, transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null && lastHitTime <= Time.time - repeatDamagePeriod)
            Attack();
    }

    void Update ()
    {
        if (dead || player == null)
        {
            Destroy(gameObject,2f);
            return;
        }

        if ((currentDestination.Value.position - transform.position).sqrMagnitude < minDistanceToTarget)
            currentDestination = currentDestination.Next != null ? currentDestination.Next : waypoints.First;

        if (rigBody.velocity.x < 0 == transform.localScale.x > 0)
            Flip();

        if (HP <= 0 && !dead)
            Death();
    }

    void FindNearestWaypoint()
    {
        LinkedListNode<Transform> currentWaypoint = waypoints.First;
        float minVectorDistance = float.PositiveInfinity;
        float currentVectorDistance = float.PositiveInfinity;

        while (currentWaypoint.Next!=null)
        {
            currentVectorDistance = (transform.position - currentWaypoint.Value.position).sqrMagnitude;
            if (currentVectorDistance < minVectorDistance)
            {
                minVectorDistance = currentVectorDistance;
                currentDestination = currentWaypoint;
            }

            currentWaypoint = currentWaypoint.Next;
        }
    }

    private void Attack()
    {
        Debug.Log("Boss Attack");
        lastHitTime = Time.time;
    }

    public void Hurt(int hp)
    {
        HP -= hp;
    }

    void Death()
    {
        audioSource.clip = deathScream;
        audioSource.Play();
        Invoke("Blow", 1f);
        anim.SetTrigger("Die");

        score.score += 1000;

        dead = true;

        rigBody.velocity = Vector2.zero;
        rigBody.isKinematic = true;

        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;
        Instantiate(thousandPointsUI, scorePos, Quaternion.identity);
    }

    void Blow()
    {
        audioSource.clip=deathBlow;
        audioSource.Play();
    }

    private void Flip()
    {
        Vector3 bossScale = transform.localScale;
        bossScale.x *= -1;
        transform.localScale = bossScale;
    }
}
