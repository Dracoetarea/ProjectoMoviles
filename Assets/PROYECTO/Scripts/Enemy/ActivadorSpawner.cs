using UnityEngine;

public class ActivadorDeSpawners : MonoBehaviour
{
    public Spawner[] spawners;
    private bool spawnersActivados = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Activa los spawn si aún no han sido activados y el objeto colisiona con la etiqueta "SueloCueva"
        if (!spawnersActivados && collision.gameObject.CompareTag("SueloCueva"))
        {
            ActivarSpawners();
        }
    }

    // Activa todos los Spawners vinculados
    private void ActivarSpawners()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.IniciarSpawn();
        }
        spawnersActivados = true;
    }
}
