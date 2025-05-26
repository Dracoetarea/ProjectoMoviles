using UnityEngine;
using System.Collections;

public class ShootController : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;
    private float nextFireTime = 0f;

    private CharacterLife vidaPersonaje;
    private Animator animator;
    private PlayerController controlJugador;

    [Header("Sprites")]
    public Sprite nuevoSpriteBala;
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
        if (Time.time >= nextFireTime && Input.GetButtonDown("Fire1") && vidaPersonaje.vidaActual > 0)
        {
            Fire();
            nextFireTime = Time.time + shootCooldown;
        }
    }

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

    IEnumerator ResetDisparo()
    {
        yield return new WaitForSeconds(0.5f);
        if (animator != null && controlJugador != null)
        {
            animator.SetBool("isShooting", false);
        }
    }

    public void CambiarSpriteDeBala()
    {
        spriteCambiado = true;
        Debug.Log("Sprite de bala cambiado correctamente.");
    }
}
