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
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
