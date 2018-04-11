using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public Transform[] waypointsArray;
    public float moveForce;
    public float maxWalkVelosity;
    public float maxRunVelosity;
    public float minDistanceToTarget;
    public float detectionRange;
    public AudioClip deathClip;

    private LinkedListNode<Transform> currentDestination;
    private Rigidbody2D rigBody;
    private LinkedList<Transform> waypoints;
    private bool isAggressive;
    private float maxVelosity;
    private Animator anim;
    private GameObject player;
    private AudioSource audioSource;
    private EnemyHealth enemyHealth;
    private BossAttack bossAttack;

    void Awake ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        waypoints = new LinkedList<Transform>(waypointsArray);
        currentDestination = waypoints.First;
        isAggressive = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        enemyHealth = GetComponent<EnemyHealth>();
        bossAttack = GetComponent<BossAttack>();
        enemyHealth.Death += OnDeath;
    }

    private void FixedUpdate()
    {
        if (enemyHealth.dead || player == null)
            return;

        maxVelosity = isAggressive ? maxRunVelosity : maxWalkVelosity;

        rigBody.AddForce(Vector2.right * moveForce * Mathf.Sign((currentDestination.Value.position.x - transform.position.x)));

        if (Mathf.Abs(rigBody.velocity.x) > maxVelosity)
            rigBody.velocity = new Vector2(Mathf.Sign(rigBody.velocity.x)* maxVelosity, rigBody.velocity.y);

        anim.SetFloat("VelosityX",Mathf.Abs(rigBody.velocity.x));
        anim.SetFloat("VelosityY", Mathf.Abs(rigBody.velocity.y));

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x)* detectionRange, transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            rigBody.velocity = Vector2.zero;
            bossAttack.Attack();
        }
    }

    void Update ()
    {
        if ((currentDestination.Value.position - transform.position).sqrMagnitude < minDistanceToTarget)
            currentDestination = currentDestination.Next != null ? currentDestination.Next : waypoints.First;

        if (rigBody.velocity.x < 0 == transform.localScale.x > 0)
            enemyHealth.Flip();
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

    void OnDeath()
    {
        audioSource.clip = deathClip;
        audioSource.Play();
        Destroy(gameObject, 2f);

        rigBody.velocity = Vector2.zero;
        rigBody.isKinematic = true;
    }

}
