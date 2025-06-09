using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int disparosRealizados; // Total disparos
    public int enemigosEliminados; // Total enemigos muertos
    public int puntuacionMaxima; // Puntuacion Total

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Carga las estadísticas almacenadas en PlayerPrefs al iniciar
            CargarStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metodo para guardar los disparos realizados
    public void RegistrarDisparo()
    {
        disparosRealizados++;
        GuardarStats();
    }

    // Metodo para guardar los enemigos eliminados
    public void RegistrarEnemigoEliminado()
    {
        enemigosEliminados++;
        GuardarStats();
    }

    // Metodo para guardar la puntuacion conseguida
    public void RegistrarPuntuacion(int score)
    {
        if (score > puntuacionMaxima)
        {
            puntuacionMaxima = score;
            GuardarStats();
        }
    }

    // Guarda las estadisticas actuales en PlayerPrefs
    public void GuardarStats()
    {
        PlayerPrefs.SetInt("Disparos", disparosRealizados);
        PlayerPrefs.SetInt("Eliminados", enemigosEliminados);
        PlayerPrefs.SetInt("PuntuacionMax", puntuacionMaxima);
        PlayerPrefs.Save();
    }

    // Carga las estadisticas desde PlayerPrefs
    public void CargarStats()
    {
        disparosRealizados = PlayerPrefs.GetInt("Disparos", 0);
        enemigosEliminados = PlayerPrefs.GetInt("Eliminados", 0);
        puntuacionMaxima = PlayerPrefs.GetInt("PuntuacionMax", 0);
    }
}
