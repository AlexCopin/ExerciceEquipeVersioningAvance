using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 startPosition;
    private float direction;
    
    [SerializeField] private Vector2 xPatrolPosition;
    [SerializeField] private float xEpsilonDegree;
    [Space]
    [SerializeField] private float speed;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = (Vector2)transform.position + Vector2.right * direction * speed * Time.deltaTime;

        if (direction > 0 && transform.position.x >= startPosition.x + xPatrolPosition.y - xEpsilonDegree || 
            direction < 0 && transform.position.x <= startPosition.x - xPatrolPosition.x + xEpsilonDegree)
        {
            direction *= -1f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (!Application.isPlaying)
        {
            Gizmos.DrawLine((Vector2) transform.position + Vector2.left * xPatrolPosition.x,
                (Vector2) transform.position + Vector2.right * xPatrolPosition.y);
            Gizmos.DrawWireSphere((Vector2) transform.position + Vector2.left * xPatrolPosition.x, 0.25f);
            Gizmos.DrawWireSphere((Vector2) transform.position + Vector2.right * xPatrolPosition.y, 0.25f);
        }
        else
        {
            Gizmos.DrawLine(startPosition + Vector2.left * xPatrolPosition.x,
                (Vector2) startPosition + Vector2.right * xPatrolPosition.y);
            
            Gizmos.DrawWireSphere(startPosition + Vector2.left * xPatrolPosition.x, 0.25f);
            Gizmos.DrawWireSphere(startPosition + Vector2.right * xPatrolPosition.y, 0.25f);
        }
    }
}
