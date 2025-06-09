using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemigosEliminados = 0;
    public float incrementoVelocidad = 0.01f;
    private float velocidadAcumulada = 0f;

    public PlayerSkinSwitcher playerSkinSwitcher;
    public GameObject[] enemigosEspeciales; // Enemigos especiales tienen 2 de vida
    private int dañoBala = 1;

    public GameObject enemigoEspecialPrefab; // Prefab del boss
    public Transform puntoAparicionEspecial;
    private bool enemigoEspecialInstanciado = false;
    [SerializeField] private GameObject[] spawnersADesactivar; // Spawners que se desactivan cuando aparece un boss

    // Ejecuta ciertas funciones al matar un enemigo
    public void EnemigoEliminado()
    {
        StatsManager.instance?.RegistrarEnemigoEliminado();
        enemigosEliminados++;

        if (enemigosEliminados % 10 == 0)
        {
            AumentarVelocidadEnemigos();
        }

        if (enemigosEliminados == 10)
        {
            playerSkinSwitcher.CambiarSkin();

            ShootController shoot = Object.FindFirstObjectByType<ShootController>();
            if (shoot != null)
            {
                shoot.CambiarSpriteDeBala();
                AumentarDaño();
            }
        }
        else if (enemigosEliminados == 20)
        {
            playerSkinSwitcher.CambiarSkin3();
        }
        else if (enemigosEliminados == 50)
        {
            if (!enemigoEspecialInstanciado)
            {
                InstanciarEnemigoEspecial();
                enemigoEspecialInstanciado = true;
            }
        }
    }

    // Incrementa la velocidad de todos los enemigos activos si no supera un máximo
    public void AumentarVelocidadEnemigos()
    {
        if (velocidadAcumulada < 0.15f)
        {
            velocidadAcumulada += incrementoVelocidad;
            EnemyController[] enemigosActivos = Object.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);


            foreach (EnemyController enemigo in enemigosActivos)
            {
                enemigo.speed += incrementoVelocidad;
            }
        }
    }

    // Devuelve la velocidad acumulada
    public float ObtenerVelocidadAcumulada()
    {
        return velocidadAcumulada;
    }

    // Comprueba si un enemigo pertenece a la lista de especiales
    public bool EsEspecial(GameObject prefab)
    {
        foreach (GameObject especial in enemigosEspeciales)
        {
            if (prefab.name == especial.name)
                return true;
        }
        return false;
    }

    // Cambia el daño que hacen las balas del jugador, se activa al matar ciertos enemigos
    public void AumentarDaño()
    {
        dañoBala = 2;
    }

    // Devuelve el daño de la bala
    public int ObtenerDaño()
    {
        return dañoBala;
    }

    // Instancia un enemigo especial tipo jefe y detiene algunos spawners
    private void InstanciarEnemigoEspecial()
    {
        if (enemigoEspecialPrefab != null && puntoAparicionEspecial != null)
        {
            GameObject boss = Instantiate(enemigoEspecialPrefab, puntoAparicionEspecial.position, Quaternion.identity);
            boss.transform.localScale = new Vector3(-1f, 1f, 1f);

            EnemyController controller = boss.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.vida = 15;
            }

            DesactivarSpawners();
        }
    }

    // Detiene los spawners normales temporalmente
    private void DesactivarSpawners()
    {
        foreach (GameObject spawner in spawnersADesactivar)
        {
            if (spawner != null)
            {
                Spawner spawnerScript = spawner.GetComponent<Spawner>();
                if (spawnerScript != null)
                {
                    spawnerScript.DetenerSpawn();
                }

                spawner.SetActive(false);
            }
        }

        StartCoroutine(ReactivarSpawners());
    }

    // Reactiva los spawners después de unos segundos
    private IEnumerator ReactivarSpawners()
    {
        yield return new WaitForSeconds(4f);

        foreach (GameObject spawner in spawnersADesactivar)
        {
            if (spawner != null)
            {
                spawner.SetActive(true);

                Spawner spawnerScript = spawner.GetComponent<Spawner>();
                if (spawnerScript != null)
                {
                    spawnerScript.IniciarSpawn();
                }
            }
        }
    }
}
