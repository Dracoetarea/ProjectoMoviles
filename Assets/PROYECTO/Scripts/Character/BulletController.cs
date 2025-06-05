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
        if (controlJugador == null) controlJugador = Object.FindFirstObjectByType<PlayerController>();

        rb.gravityScale = 0;
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

            TryDropCoin(spawnPosition);

            EnemyController enemigo = collision.gameObject.GetComponent<EnemyController>();
            if (enemigo != null)
            {
                int daño = 1;
                EnemyManager shoot = Object.FindFirstObjectByType<EnemyManager>();
                if (shoot != null)
                {
                    daño = shoot.ObtenerDaño();
                }

                enemigo.RecibirDaño(daño);
            }

            Destroy(gameObject);

            EnemyManager manager = Object.FindFirstObjectByType<EnemyManager>();
            if (manager != null) manager.EnemigoEliminado();
        }
        else if (collision.gameObject.CompareTag("pared") || collision.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    void TryDropCoin(Vector3 position)
    {
        float chance = Random.Range(0f, 1f);

        if (chance <= 0.5f)
        {
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
