using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10f;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Rotar según dirección
        if (direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        else
            transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemigo
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        // Suelo
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
