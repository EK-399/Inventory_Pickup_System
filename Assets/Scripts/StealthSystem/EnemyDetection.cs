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

    private bool isPlayerDetected = false; // Flag to check if the player is detected

    private void Start()
    {
        // Save the enemy's original position
        originalPosition = transform.position;
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

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.transform == player)
    //    {
    //        Debug.Log("Player detected!");
    //        isPlayerDetected = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.transform == player)
    //    {
    //        Debug.Log("Player escaped!");
    //        isPlayerDetected = false;
    //    }
    //}

    public void Patrol()
    {
        //move back and forth or waypoints
        Debug.Log("Patrol");


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
        if (distance < 2)
        {
            //attack
            currentEnemyState = EnemyState.Attacking;
        }

        if (distance > 10)
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

    //private void OnDrawGizmos()
    //{
    //    // Visualize the detection radius in the editor
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}