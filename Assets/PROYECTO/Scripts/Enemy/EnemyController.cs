using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private CharacterLife cf;
    public bool mirandoDerecha = false;
    public Animator animator;
    private bool muerto = false;
    public int vida = 1;

    private AudioSource muerteAudio;

    void Start()
    {
        animator = GetComponent<Animator>();
        muerteAudio = GetComponent<AudioSource>();

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
        if (cf != null && cf.jugando && target != null && !muerto)
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
    public void RecibirDaño(int cantidad)
    {
        if (muerto) return;

        vida -= cantidad;

        if (vida <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null) animator.SetTrigger("Hit");
        }
    }
    public void Die()
    {
        if (muerto) return;
        muerto = true;

        ScoreManager.instance?.AddScore(50);
        animator.SetTrigger("Die");
        speed = 0f;
        target = null;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        float delay = 0.5f;
        if (muerteAudio != null && muerteAudio.clip != null)
        {
            muerteAudio.Play();
            delay = muerteAudio.clip.length;
        }

        Destroy(gameObject, delay);
    }
}
