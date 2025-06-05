using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    private bool spawnActivo = false;

    public void IniciarSpawn()
    {
        if (!spawnActivo)
        {
            spawnActivo = true;
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject nuevo = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

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

    public void DetenerSpawn()
    {
        CancelInvoke("SpawnEnemy");
        spawnActivo = false;
    }
}
