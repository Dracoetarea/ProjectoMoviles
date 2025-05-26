using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public Rigidbody2D rbd;
    public float fuerza;
    public AudioSource audioSource;
    public AudioClip saltoClip;
    public Animator animator;
    private bool enSuelo = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rbd.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);

            if (audioSource != null && saltoClip != null)
            {
                audioSource.PlayOneShot(saltoClip);
            }

            enSuelo = false;
            animator.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Suelo"))
        {
            enSuelo = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("SueloCueva"))
        {
            enSuelo = false;
            animator.SetBool("isJumping", false);
        }
    }
}