using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Daño")]
    public int damage = 1;
    public float hitCooldown = 0.5f;

    Vector3 target;
    SpriteRenderer sr;
    float nextHitTime;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (pointA != null && pointB != null)
            target = pointB.position;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            target = pointB.position;

        if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            target = pointA.position;

        // Girar sprite según dirección
        if (sr != null)
            sr.flipX = (target.x < transform.position.x);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextHitTime) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                nextHitTime = Time.time + hitCooldown;
            }
        }
    }
}

