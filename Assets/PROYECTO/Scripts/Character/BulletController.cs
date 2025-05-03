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
        if (controlJugador == null) controlJugador = FindObjectOfType<PlayerController>();

        rb.gravityScale = 0;
        Vector2 direction = controlJugador.mirandoDerecha ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * speed;

        Invoke("EnableCollision", delayBeforeDestroy);
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

            EnemyManager manager = FindObjectOfType<EnemyManager>();
            Destroy(collision.gameObject); 
            Destroy(gameObject);

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
            GameObject newCoin = Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }
}
