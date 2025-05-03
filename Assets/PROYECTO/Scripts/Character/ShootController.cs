using UnityEngine;
using System.Collections;

public class ShootController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;
    private float nextFireTime = 0f;
    private CharacterLife vidaPersonaje;

    private Animator animator;
    private PlayerController controlJugador;

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

        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.controlJugador = controlJugador;
        }

        // Desactivar movimiento y activar animación de disparo
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
}
