using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 12f;

    public GameObject arrowPrefab;
    public float fireCooldown = 2f;

    public int vida = 3;
    public float invencibleTiempo = 0.7f;

    Rigidbody2D rb;
    SpriteRenderer sr;

    float moveX;
    bool isGrounded;
    float nextFireTime;

    bool recibiendoDanio;
    public bool muerto;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (muerto) return;

        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        else if (Keyboard.current.dKey.isPressed) moveX = 1f;
        else moveX = 0f;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !recibiendoDanio)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (!recibiendoDanio &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            Time.time >= nextFireTime)
        {
            DispararFlecha();
            nextFireTime = Time.time + fireCooldown;
        }

        if (moveX < 0) sr.flipX = true;
        else if (moveX > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        if (muerto) return;
        if (recibiendoDanio) return;

        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    void DispararFlecha()
    {
        if (arrowPrefab == null) return;

        bool mirandoDerecha = !sr.flipX;
        Vector2 direccion = mirandoDerecha ? Vector2.right : Vector2.left;

        float offsetX = mirandoDerecha ? 0.8f : -0.8f;
        Vector3 spawnPos = transform.position + new Vector3(offsetX, 0.2f, 0f);

        GameObject flecha = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        Collider2D arrowCol = flecha.GetComponent<Collider2D>();
        Collider2D playerCol = GetComponent<Collider2D>();
        if (arrowCol != null && playerCol != null)
            Physics2D.IgnoreCollision(arrowCol, playerCol);

        Arrow arrowScript = flecha.GetComponent<Arrow>();
        if (arrowScript != null)
            arrowScript.direction = direccion;
    }

    public void RecibeDanio(int cantDanio)
    {
        if (muerto) return;
        if (recibiendoDanio) return;

        recibiendoDanio = true;
        vida -= cantDanio;

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife(cantDanio);

        if (vida <= 0)
        {
            muerto = true;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Invoke(nameof(DesactivaDanio), invencibleTiempo);
        }
    }

    void DesactivaDanio()
    {
        recibiendoDanio = false;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            RecibeDanio(1);
    }
}
