using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore = 0; // Puntuación actual del jugador
    public TextMeshProUGUI scoreText; // Referencia al UI para mostrar la puntuación

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


    // Método para aumentar la puntuación
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    // Método para resetear la puntuación a cero
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    // Actualiza el texto con la puntuación actual
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
}
