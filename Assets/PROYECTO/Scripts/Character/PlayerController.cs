using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad;
    private float inputMovimiento;
    public bool mirandoDerecha = false; // true = derecha || false = izquierda
    private Animator animator;

    [SerializeField] private HUD hud;

    public bool EstaQuieto => inputMovimiento == 0;

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

    void moverse()
    {
        inputMovimiento = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = new Vector2(inputMovimiento * velocidad, rigidbody.linearVelocity.y);
    }

    void gestionarOrientacion(float inputMovimiento)
    {
        if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
        }
    }

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
