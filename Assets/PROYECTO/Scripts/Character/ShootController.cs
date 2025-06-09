using System.Collections;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f; // Tiempo entre disparos
    private float nextFireTime = 0f; // Control del tiempo para el próximo disparo

    private CharacterLife vidaPersonaje;
    private Animator animator;
    private PlayerController controlJugador;

    [Header("Sprites")]
    public Sprite nuevoSpriteBala; // Sprite alternativo para la bala
    private bool spriteCambiado = false;

    public AudioSource audioSource;
    public AudioClip disparoClip;

    public ShootController(PlayerController playerController)
    {
        this.controlJugador = playerController;
    }

    void Start()
    {
        vidaPersonaje = GetComponent<CharacterLife>();
        animator = GetComponent<Animator>();
        controlJugador = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Comprobamos si el jugador puede disparar
        if (Time.time >= nextFireTime && Input.GetButtonDown("Fire1") && vidaPersonaje.vidaActual > 0)
        {
            Fire();
            nextFireTime = Time.time + shootCooldown;
        }
    }

    // Método que ejecuta el disparo
    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        if (spriteCambiado && nuevoSpriteBala != null)
        {
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = nuevoSpriteBala;
            }
        }

        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.controlJugador = controlJugador;
        }

        if (disparoClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(disparoClip);
        }

        if (animator != null && controlJugador != null)
        {
            animator.SetBool("isShooting", true);
            StartCoroutine(ResetDisparo());
        }
    }

    // Espera para desactivar la animación de disparo
    IEnumerator ResetDisparo()
    {
        yield return new WaitForSeconds(0.5f);
        if (animator != null && controlJugador != null)
        {
            animator.SetBool("isShooting", false);
        }
    }

    // Activa otra skin para la bala
    public void CambiarSpriteDeBala()
    {
        spriteCambiado = true;
        Debug.Log("Sprite de bala cambiado correctamente.");
    }
}
