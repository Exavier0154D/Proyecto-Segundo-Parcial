using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HUD")]
    public TMP_Text textScore;   // ScoreText
    public TMP_Text textLives;   // LivesText

    [Header("Valores")]
    public int score = 0;
    public int lives = 3;

    private void Awake()
    {
        // Singleton simple (una sola instancia por escena)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        lives = 3;
        UpdateHUD();
    }

    /* =======================
       SCORE
       ======================= */
    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateHUD();
    }

    /* =======================
       VIDAS
       ======================= */
    public void LoseLife(int amount = 1)
    {
        lives -= amount;
        UpdateHUD();

        if (lives <= 0)
        {
            // Reinicia la escena cuando se quedan sin vidas
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /* =======================
       HUD
       ======================= */
    private void UpdateHUD()
    {
        if (textScore != null)
            textScore.text = "Puntos: " + score;

        if (textLives != null)
            textLives.text = "Vidas: " + lives;
    }
}
