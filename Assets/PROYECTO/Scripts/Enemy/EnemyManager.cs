using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    private int enemigosEliminados = 0;
    public float incrementoVelocidad = 0.05f;
    private float velocidadAcumulada = 0f;

    public PlayerSkinSwitcher playerSkinSwitcher;
    public GameObject[] enemigosEspeciales;
    private int dañoBala = 1;
    private bool vidaAumentada = false;

    public GameObject enemigoEspecialPrefab;
    public Transform puntoAparicionEspecial;
    private bool enemigoEspecialInstanciado = false;
    [SerializeField] private GameObject[] spawnersADesactivar;

    void Start()
    {
    }

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

    public float ObtenerVelocidadAcumulada()
    {
        return velocidadAcumulada;
    }

    public bool EsEspecial(GameObject prefab)
    {
        foreach (GameObject especial in enemigosEspeciales)
        {
            if (prefab.name == especial.name)
                return true;
        }
        return false;
    }

    public void AumentarDaño()
    {
        dañoBala = 2;
    }

    public int ObtenerDaño()
    {
        return dañoBala;
    }

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

    private IEnumerator ReactivarSpawners()
    {
        yield return new WaitForSeconds(3f);

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
