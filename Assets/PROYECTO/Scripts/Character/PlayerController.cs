using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad; // Velocidad de desplazamiento del jugador
    private float inputMovimiento; // Valor de entrada horizontal (-1 a 1)
    public bool mirandoDerecha = false; // Dirección actual del jugador
    private Animator animator; // Controlador de animaciones

    [SerializeField] private HUD hud; // Referencia al HUD para gestionar golpes

    public bool EstaQuieto => inputMovimiento == 0; // Propiedad para saber si está quieto

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moverse();
        gestionarOrientacion(inputMovimiento);
        gestionarAnimacion(inputMovimiento);
    }


    // Lógica para el movimiento horizontal usando Rigidbody2D
    void moverse()
    {
        inputMovimiento = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = new Vector2(inputMovimiento * velocidad, rigidbody.linearVelocity.y);
    }

    // Cambia la orientación del personaje si cambia la dirección de movimiento
    void gestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }

    // Activa/desactiva la animación de caminar según el movimiento
    void gestionarAnimacion(float inputMovimiento)
    {
        if (inputMovimiento != 0)
        {
            animator.SetBool("isMoving", true);
            animator.speed = 0.29f;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    // Detecta colisiones con enemigos y comunica al HUD que se ha recibido un golpe
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (hud != null)
            {
                hud.recibirGolpe();
            }
        }
    }
}
