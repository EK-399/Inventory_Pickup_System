using System.Security.Cryptography;
using UnityEngine;

public enum EnemyState
{
    Patrolling,
    Chasing,
    Attacking
}

public class EnemyDetection : MonoBehaviour
{
    public PlayerMovement player; // Reference to the player object
    public float detectionRadius = 35f; // Detection radius of the enemy
    public float moveSpeed = 5f; // Speed at which the enemy follows the player
    private Vector3 originalPosition; // Original position of the enemy

    public EnemyState currentEnemyState;
    public LayerMask detectLayers;

    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;



    private void Start()
    {
        // Save the enemy's original position
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        anim.SetBool("isRunning", true);
    }

    private void Update()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Patrolling:

                Patrol();

                break;
            case EnemyState.Chasing:

                Chase();

                break;
            case EnemyState.Attacking:

                Attack();

                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    public void Patrol()
    {
        Debug.Log("Patrol");
        //move back and forth or waypoints

        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            flip();
            currentPoint = PointA.transform;
            Debug.Log("Hit PointA");
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            flip();
            currentPoint = PointB.transform;
            Debug.Log("Hit PointB");
        }

        Debug.DrawRay(transform.position, Vector2.left * 10, Color.red);

        //if we're close to the player or can see them
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.left, 10, detectLayers);
        if (ray.collider != null)
        {
            Debug.Log("Hitting something");
            if (ray.collider.CompareTag("Player"))
            {
                if (player.isHiding == false)
                {
                    currentEnemyState = EnemyState.Chasing;
                }
                //and the player is NOT hiding
                //then we start chasing

               
            }
        }

     }

    public void Chase()
    {
        Debug.Log("Chase");

        MoveToPosition(player.transform.position);
        //if close enough, attack player

        //check distance to the player
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 1)
        {
            //attack
            currentEnemyState = EnemyState.Attacking;
        }

        if (distance > 5)
        {
            currentEnemyState = EnemyState.Patrolling;
        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
        //remove 1 heart from the player
        //knockback player

        //go back to patrol.
        transform.position = originalPosition;

        currentEnemyState = EnemyState.Patrolling;
       
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        // Calculate direction to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move towards the target position
        float step = moveSpeed * Time.deltaTime; // Calculate the distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
}