using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;

    [Header("Daño al jugador")]
    public int damageToPlayer = 1;

    [Header("Vida enemigo")]
    public int vida = 3;
    public float fuerzaRebote = 6f;

    Rigidbody2D rb;
    Vector2 movement;

    bool enMovimiento;
    bool muerto;
    bool recibiendoDanio;
    bool playerVivo = true;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerVivo && !muerto)
        {
            Movimiento();
        }

        if (animator != null)
        {
            animator.SetBool("enMovimiento", enMovimiento);
            animator.SetBool("muerto", muerto);
        }
    }

    private void Movimiento()
    {
        if (player == null)
        {
            movement = Vector2.zero;
            enMovimiento = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Voltear sprite
            if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
            if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);

            movement = new Vector2(direction.x, 0);
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        if (!recibiendoDanio)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (muerto) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController2D playerScript = collision.gameObject.GetComponent<PlayerController2D>();
            if (playerScript != null)
            {
                playerScript.RecibeDanio(damageToPlayer);

                playerVivo = !playerScript.muerto;
                if (!playerVivo)
                    enMovimiento = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (muerto) return;

        // ✅ Daño por flecha (asegúrate de que la flecha tenga Tag "Arrow")
        if (collision.CompareTag("Arrow"))
        {
            Vector2 direccionDanio = new Vector2(collision.transform.position.x, 0);
            RecibeDanio(direccionDanio, 1);

            Destroy(collision.gameObject); // destruye la flecha al impactar
        }

        // Si todavía usas espada, puedes dejar esto también:
        // if (collision.CompareTag("Espada")) { ... }
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (recibiendoDanio || muerto) return;

        vida -= cantDanio;
        recibiendoDanio = true;

        if (vida <= 0)
        {
            muerto = true;
            enMovimiento = false;
        }
        else
        {
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            StartCoroutine(DesactivaDanio());
        }
    }

    IEnumerator DesactivaDanio()
    {
        yield return new WaitForSeconds(0.4f);
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void EliminarCuerpo()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
