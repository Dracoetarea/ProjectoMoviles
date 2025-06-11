using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public Rigidbody2D rbd;
    public float fuerza; // Fuerza del salto
    public AudioSource audioSource;
    public AudioClip saltoClip;
    public Animator animator;
    private bool enSuelo = false;

    void Update()
    {
        // PC
#if !UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            RealizarSalto();
        }
#endif
    }

    // Android
    public void Saltar()
    {
#if UNITY_ANDROID
        if (enSuelo)
        {
            RealizarSalto();
        }
#endif
    }

    // PC
    private void RealizarSalto()
    {
        rbd.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);

        if (audioSource != null && saltoClip != null)
        {
            audioSource.PlayOneShot(saltoClip);
        }

        enSuelo = false;
        if (animator != null)
            animator.SetBool("isJumping", true);
    }

    // Detectamos cuándo el personaje toca el suelo
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
        {
            enSuelo = true;
            animator.SetBool("isJumping", false);
        }
    }

    // Detectamos si deja de estar en contacto con ciertas plataformas
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SueloCueva"))
        {
            enSuelo = false;
            animator.SetBool("isJumping", false);
        }
    }
}