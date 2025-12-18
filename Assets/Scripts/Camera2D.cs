using UnityEngine;


public class Camera2D : MonoBehaviour
{
    public Transform targetPlayer;
    public float offsetX = 6f;
    public float offsetY = 0f;
    public float smoothSpeed = 5f;

    SpriteRenderer playerSR;


    void Start()
    {
        if (targetPlayer != null)
            playerSR = targetPlayer.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (targetPlayer == null) return;

        float lookOffsetX = offsetX;

        // Cambiar offset según dirección del jugador
        if (playerSR != null && playerSR.flipX)
            lookOffsetX = -offsetX;

        Vector3 desiredPosition = new Vector3(
            targetPlayer.position.x + lookOffsetX,
            targetPlayer.position.y + offsetY,
            -10f
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
