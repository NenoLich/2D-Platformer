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
    public float AggressionTime;
    public float climbingSpeed;
    
    private LinkedListNode<Transform> destination;
    private GameObject ladderPath;
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
    private Vector3 currentPath;

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

        currentPath = destination.Value.position;
        ladderPath = null;
        FindPath(ladders);

        maxVelosity = isAggressive ? maxRunVelosity : maxWalkVelosity;

        rigBody.AddForce(Vector2.right * moveForce * Mathf.Sign(
            ladderPath!=null ? ladderPath.transform.position.x - transform.position.x :
            destination.Value.position.x - transform.position.x));

        if (Mathf.Abs(rigBody.velocity.x) > maxVelosity)
            rigBody.velocity = new Vector2(Mathf.Sign(rigBody.velocity.x)* maxVelosity, rigBody.velocity.y);

        anim.SetFloat("VelosityX",Mathf.Abs(rigBody.velocity.x));
        anim.SetFloat("VelosityY", Mathf.Abs(rigBody.velocity.y));

        if (rigBody.velocity.x < 0 == transform.localScale.x > 0)
            enemyHealth.Flip();

        RaycastHit2D hit = Physics2D.Linecast(transform.position, 
            new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x)* detectionRange, 
            transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            isAggressive = true;
            destination = new LinkedListNode<Transform>(player.transform);
            lastAggressionTime = Time.time;
            rigBody.velocity = Vector2.Lerp(rigBody.velocity,Vector2.zero,0.99f);
            bossAttack.Attack();
        }
    }

    void Update ()
    {
        if (isAggressive && Time.time-lastAggressionTime>AggressionTime)
        {
            FindNearestDestination();
            isAggressive = false;
        }

        if (ladderPath!=null && Mathf.Abs(ladderPath.transform.position.x - groundCheck.position.x) < minDistanceToTarget/3)
        {
            StartCoroutine(ClimbingRoutine());
        }

        if (!isAggressive)
        {
            if ((destination.Value.position - transform.position).sqrMagnitude < minDistanceToTarget)
                destination = destination.Next != null ? destination.Next : waypoints.First;
        }
    }

    IEnumerator ClimbingRoutine()
    {
        enabled = false;

        float climbingForce = groundCheck.position.y < ladderPath.transform.position.y ? climbingSpeed : -climbingSpeed;
        climbingForce *= isAggressive ? 2 : 1;

        while (Mathf.Sign(climbingForce) == Mathf.Sign(climbingSpeed)? 
            groundCheck.position.y < ladderPath.transform.Find("UpperGateway").transform.position.y: 
            groundCheck.position.y > ladderPath.transform.Find("LowerGateway").transform.position.y)
        {
            rigBody.isKinematic = true;
            rigBody.velocity = new Vector2(0f, climbingForce);
            anim.SetFloat("VelosityX", Mathf.Abs(rigBody.velocity.x));
            anim.SetFloat("VelosityY", Mathf.Abs(rigBody.velocity.y));

            yield return new WaitForEndOfFrame();
        }

        rigBody.isKinematic = false;
        enabled = true;
    }

    void FindPath(GameObject[] possibleLadders)
    {
        string inputGateway, outputGateway;
        if (currentPath.y > transform.position.y)
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
            ladderPath = ladder;
            currentPath = ladder.transform.Find(outputGateway).position;
            if (Mathf.Abs(currentPath.y-groundCheck.position.y)>0.1f)
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
            currentVectorDistance = Mathf.Abs(gatewayPosition.y - currentPath.y)+ Mathf.Abs(gatewayPosition.x - currentPath.x)/10;

            if (Mathf.Abs(gatewayPosition.y - currentPath.y) < 
                Mathf.Abs(transform.position.y - currentPath.y) && 
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

        StopAllCoroutines();
        rigBody.velocity = Vector2.zero;
        rigBody.isKinematic = false;
    }

}
