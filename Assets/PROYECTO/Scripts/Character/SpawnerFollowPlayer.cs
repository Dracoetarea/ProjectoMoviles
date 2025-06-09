using UnityEngine;

public class SpawnerFollowPlayer : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador
    public Vector3 offset; // Distancia deseada entre el jugador y el spawner

    void Update()
    {
        // Actualiza la posición del spawner para que siga al jugador con el offset
        if (player != null)
        {
            transform.position = player.position + offset;
            Camera.main.orthographicSize = 10f;
        }
    }
}
