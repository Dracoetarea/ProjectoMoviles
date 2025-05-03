using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private CharacterLife cf;
    public bool mirandoDerecha = false;

    void Start()
    {
        // Aplicar velocidad acumulada al spawnear
        EnemyManager manager = FindObjectOfType<EnemyManager>();
        if (manager != null) speed += manager.ObtenerVelocidadAcumulada();

        cf = (CharacterLife)FindFirstObjectByType(typeof(CharacterLife));
        if (cf == null) Debug.LogError("No se encontró CharacterLife.");

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Jugador");
            if (player != null) target = player.transform;
        }
    }

    void Update()
    {
        if (cf != null && cf.jugando && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if ((target.position.x > transform.position.x && !mirandoDerecha) ||
                (target.position.x < transform.position.x && mirandoDerecha))
            {
                Flip();
            }
        }

}
    void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}