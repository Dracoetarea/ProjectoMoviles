using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    private bool spawnActivo = false;

    // M�todo para iniciar el spawn de enemigos

    public void IniciarSpawn()
    {
        if (!spawnActivo)
        {
            spawnActivo = true;
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
    }

    // M�todo que crea un enemigo en uno de los puntos de spawn aleatoriamente
    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject nuevo = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Instancia el enemigo en la posici�n y rotaci�n del punto seleccionado
        EnemyController ec = nuevo.GetComponent<EnemyController>();
        if (ec != null)
        {
            EnemyManager manager = Object.FindFirstObjectByType<EnemyManager>();
            if (manager != null && manager.EsEspecial(enemyPrefab))
            {
                ec.vida = 2;
            }
        }
    }

    // M�todo para detener el spawn de enemigos
    public void DetenerSpawn()
    {
        CancelInvoke("SpawnEnemy");
        spawnActivo = false;
    }
}
