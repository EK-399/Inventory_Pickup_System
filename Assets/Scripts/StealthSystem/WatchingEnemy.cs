using UnityEngine;

public class WatchingEnemy : MonoBehaviour
{
    public float visionDistance;
    public LineRenderer lineOfSight;

    void Update()
    {
        lineOfSight.SetPosition(0, transform.position);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, visionDistance);
        if (hitInfo.collider !=null)
        {
            Debug.DrawRay(transform.position, Vector2.left, Color.red);
            lineOfSight.SetPosition(1, hitInfo.point);
            lineOfSight.startColor = Color.red;
            lineOfSight.endColor = Color.red;

            //if (hitInfo.collider.tag == "Player")
            //{

            //}
        }
        else
        {
            Debug.DrawRay(transform.position, transform.position + transform.right * visionDistance, Color.green);
            lineOfSight.SetPosition(1, transform.position + transform.right * visionDistance);
            lineOfSight.startColor = Color.green;
            lineOfSight.endColor = Color.green;
        }
    }
}
