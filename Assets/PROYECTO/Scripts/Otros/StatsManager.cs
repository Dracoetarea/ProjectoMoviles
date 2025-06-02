using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int disparosRealizados;
    public int enemigosEliminados;
    public int puntuacionMaxima;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CargarStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarDisparo()
    {
        disparosRealizados++;
        GuardarStats();
    }

    public void RegistrarEnemigoEliminado()
    {
        enemigosEliminados++;
        GuardarStats();
    }

    public void RegistrarPuntuacion(int score)
    {
        if (score > puntuacionMaxima)
        {
            puntuacionMaxima = score;
            GuardarStats();
        }
    }

    public void GuardarStats()
    {
        PlayerPrefs.SetInt("Disparos", disparosRealizados);
        PlayerPrefs.SetInt("Eliminados", enemigosEliminados);
        PlayerPrefs.SetInt("PuntuacionMax", puntuacionMaxima);
        PlayerPrefs.Save();
    }

    public void CargarStats()
    {
        disparosRealizados = PlayerPrefs.GetInt("Disparos", 0);
        enemigosEliminados = PlayerPrefs.GetInt("Eliminados", 0);
        puntuacionMaxima = PlayerPrefs.GetInt("PuntuacionMax", 0);
    }
}
