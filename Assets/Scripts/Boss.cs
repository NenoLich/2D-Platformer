using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss : MonoBehaviour {

    public Transform[] waypointsArray;
    public GameObject[] ladders;
    public float moveForce;
    public float maxWalkVelosity;
    public float maxRunVelosity;
    public float minDistanceToTarget;
    public float detectionRange;
    public AudioClip deathClip;
    private float AggressionTime;
    
    private LinkedListNode<Transform> destination;
    private Vector3 path;
    private Rigidbody2D rigBody;
    private LinkedList<Transform> waypoints;
    private bool isAggressive;
    private float lastAggressionTime;
    private float maxVelosity;
    private Animator anim;
    private GameObject player;
    private AudioSource audioSource;
    private EnemyHealth enemyHealth;
    private BossAttack bossAttack;
    private Transform groundCheck;

    void Awake ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        waypoints = new LinkedList<Transform>(waypointsArray);
        destination = waypoints.First;
        isAggressive = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        enemyHealth = GetComponent<EnemyHealth>();
        bossAttack = GetComponent<BossAttack>();
        enemyHealth.Death += OnDeath;
        groundCheck = transform.Find("GroundCheck").transform;
    }

    private void FixedUpdate()
    {
        if (enemyHealth.dead || player == null)
            return;

        path = destination.Value.position;
        FindPath(ladders);
        maxVelosity = isAggressive ? maxRunVelosity : maxWalkVelosity;

        rigBody.AddForce(Vector2.right * moveForce * Mathf.Sign((path.x - transform.position.x)));

        if (Mathf.Abs(rigBody.velocity.x) > maxVelosity)
            rigBody.velocity = new Vector2(Mathf.Sign(rigBody.velocity.x)* maxVelosity, rigBody.velocity.y);

        anim.SetFloat("VelosityX",Mathf.Abs(rigBody.velocity.x));
        anim.SetFloat("VelosityY", Mathf.Abs(rigBody.velocity.y));

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x)* detectionRange, transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            isAggressive = true;
            destination = new LinkedListNode<Transform>(player.transform);
            lastAggressionTime = Time.time;
            rigBody.velocity = Vector2.zero;
            bossAttack.Attack();
        }
    }

    void Update ()
    {
        if (Time.time-lastAggressionTime>AggressionTime)
        {
            FindNearestDestination();
            isAggressive = false;
        }

        if (!isAggressive)
        {
            if ((destination.Value.position - groundCheck.position).sqrMagnitude < minDistanceToTarget)
                destination = destination.Next != null ? destination.Next : waypoints.First;
        }

        if (rigBody.velocity.x < 0 == transform.localScale.x > 0)
            enemyHealth.Flip();
    }

    void FindPath(GameObject[] possibleLadders)
    {
        string inputGateway, outputGateway;
        if (path.y > groundCheck.position.y)
        {
            inputGateway = "UpperGateway";
            outputGateway = "LowerGateway";
        }
        else
        {
            inputGateway = "LowerGateway";
            outputGateway = "UpperGateway";
        }

        GameObject ladder = FindLadderIfNeeded(possibleLadders, inputGateway);
        if (ladder != null)
        {
            path = ladder.transform.Find(outputGateway).position;
            FindPath(possibleLadders.Where(x=>x!= ladder).ToArray());
        }
    }

    GameObject FindLadderIfNeeded(GameObject[] possibleLadders, string inputGateway)
    {
        GameObject nearestLadder = null;
        float minVectorDistance = float.PositiveInfinity;
        float currentVectorDistance = float.PositiveInfinity;
        Vector3 gatewayPosition;

        foreach (GameObject ladder in possibleLadders)
        {
            gatewayPosition = ladder.transform.Find(inputGateway).position;
            currentVectorDistance = (gatewayPosition - path).sqrMagnitude;

            if (Mathf.Abs(gatewayPosition.y - path.y) < 
                Mathf.Abs(groundCheck.position.y - path.y) && 
                currentVectorDistance < minVectorDistance)
            {
                minVectorDistance = currentVectorDistance;
                nearestLadder = ladder;
            }
        }

        return nearestLadder;
    }

    void FindNearestDestination()
    {
        LinkedListNode<Transform> currentWaypoint = waypoints.First;
        float minVectorDistance = float.PositiveInfinity;
        float currentVectorDistance = float.PositiveInfinity;

        while (currentWaypoint.Next!=null)
        {
            currentVectorDistance = (groundCheck.position - currentWaypoint.Value.position).sqrMagnitude;
            if (currentVectorDistance < minVectorDistance)
            {
                minVectorDistance = currentVectorDistance;
                destination = currentWaypoint;
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
