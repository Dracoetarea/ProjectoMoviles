using UnityEngine;

public class ActivadorDeSpawners : MonoBehaviour
{
    public Spawner[] spawners;
    private bool spawnersActivados = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!spawnersActivados && collision.gameObject.CompareTag("SueloCueva"))
        {
            ActivarSpawners();
        }
    }

    private void ActivarSpawners()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.IniciarSpawn();
        }
        spawnersActivados = true;
    }
}
