using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemigosEliminados = 0;
    public float incrementoVelocidad = 0.05f;
    private float velocidadAcumulada = 0f;

    public PlayerSkinSwitcher playerSkinSwitcher;
    public void EnemigoEliminado()
    {
        enemigosEliminados++;
        Debug.Log($"Enemigos eliminados: {enemigosEliminados}");

        if (enemigosEliminados % 5 == 0)
        {
            Debug.Log("Aumentando velocidad...");
            AumentarVelocidadEnemigos();
        }

        if (enemigosEliminados == 2)
        {
            playerSkinSwitcher.CambiarSkin();
        }else if(enemigosEliminados == 4)
        {
            playerSkinSwitcher.CambiarSkin3();
        }
    }

    public void AumentarVelocidadEnemigos()
    {
        if (velocidadAcumulada < 0.15f)
        {
            velocidadAcumulada += incrementoVelocidad;
            EnemyController[] enemigosActivos = FindObjectsOfType<EnemyController>();

            Debug.Log($"Enemigos activos: {enemigosActivos.Length}");
            foreach (EnemyController enemigo in enemigosActivos)
            {
                enemigo.speed += incrementoVelocidad;
                Debug.Log($"Velocidad del enemigo: {enemigo.speed}");
            }
        }
    }

    public float ObtenerVelocidadAcumulada()
    {
        return velocidadAcumulada;
    }
}