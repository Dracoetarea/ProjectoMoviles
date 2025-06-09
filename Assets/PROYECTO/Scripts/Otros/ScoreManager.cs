using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore = 0; // Puntuaci�n actual del jugador
    public TextMeshProUGUI scoreText; // Referencia al UI para mostrar la puntuaci�n

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI(); // Actualizar la UI al iniciar el juego
    }


    // M�todo para aumentar la puntuaci�n
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    // M�todo para resetear la puntuaci�n a cero
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    // Actualiza el texto con la puntuaci�n actual
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
}
