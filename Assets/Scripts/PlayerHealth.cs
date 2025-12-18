
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Vida jugador: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Jugador murió");
            // Aquí luego hacemos respawn o GameOver
            gameObject.SetActive(false);
        }
    }
}
