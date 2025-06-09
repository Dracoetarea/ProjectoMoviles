using UnityEngine;
public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    public float delayBeforeDestroy;
    public PlayerController controlJugador;
    public GameObject coinPrefab;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Referencia al jugador para saber hacia dónde dispara
        if (controlJugador == null) controlJugador = Object.FindFirstObjectByType<PlayerController>();

        rb.gravityScale = 0;

        // Establecemos la dirección en función de hacia dónde mira el jugador
        Vector2 direction = controlJugador.mirandoDerecha ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * speed;

        Invoke("EnableCollision", delayBeforeDestroy);
        StatsManager.instance?.RegistrarDisparo();
    }

    private void EnableCollision()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            Vector3 spawnPosition = collision.transform.position;

            // Posibilidad de soltar moneda al eliminar al enemigo
            TryDropCoin(spawnPosition);

            EnemyController enemigo = collision.gameObject.GetComponent<EnemyController>();
            if (enemigo != null)
            {
                // Obtenemos el daño del EnemyManager si existe
                int daño = 1;
                EnemyManager shoot = Object.FindFirstObjectByType<EnemyManager>();
                if (shoot != null)
                {
                    daño = shoot.ObtenerDaño();
                }
                // Aplicamos el daño al enemigo
                enemigo.RecibirDaño(daño);
            }

            Destroy(gameObject); // Destruimos la bala tras el impacto

            // Avisamos al EnemyManager de que un enemigo ha sido eliminado
            EnemyManager manager = Object.FindFirstObjectByType<EnemyManager>();
            if (manager != null) manager.EnemigoEliminado();
        }
        else if (collision.gameObject.CompareTag("pared") || collision.gameObject.CompareTag("Suelo"))
        {
            // Destruimos la bala si impacta con el suelo o una pared
            Destroy(gameObject);
        }
    }

    // Método que tiene posibilidad de generar una moneda al eliminar un enemigo
    void TryDropCoin(Vector3 position)
    {
        float chance = Random.Range(0f, 1f);

        if (chance <= 0.5f)
        {
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
