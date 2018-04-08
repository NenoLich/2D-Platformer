using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public Transform[] waypointsArray;
    public float moveForce;
    public float maxVelosity;
    public float minDistanceToTarget;
    public float repeatDamagePeriod;
    public float detectionRange;

    private LinkedListNode<Transform> currentDestination;
    private Rigidbody2D rigBody;
    private LinkedList<Transform> waypoints;
    private float lastHitTime;

    void Awake ()
    {
        rigBody = GetComponent<Rigidbody2D>();
        waypoints = new LinkedList<Transform>(waypointsArray);
        currentDestination = waypoints.First;
    }

    private void FixedUpdate()
    {
        rigBody.AddForce(Vector2.right * moveForce * Mathf.Sign((currentDestination.Value.position.x - transform.position.x)));

        if (Mathf.Abs(rigBody.velocity.x) > maxVelosity)
            rigBody.velocity = new Vector2(Mathf.Sign(rigBody.velocity.x)* maxVelosity, rigBody.velocity.y);

        RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + Mathf.Sign(rigBody.velocity.x)* detectionRange, transform.position.y), LayerMask.GetMask("Player"));
        if (hit.collider != null && lastHitTime <= Time.time - repeatDamagePeriod)
            Attack();
    }

    void Update ()
    {
        if ((currentDestination.Value.position - transform.position).sqrMagnitude < minDistanceToTarget)
            currentDestination = currentDestination.Next != null ? currentDestination.Next : waypoints.First;

        if (rigBody.velocity.x < 0 == transform.localScale.x > 0)
            Flip();
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

    private void Flip()
    {
        Vector3 bossScale = transform.localScale;
        bossScale.x *= -1;
        transform.localScale = bossScale;
    }
}
