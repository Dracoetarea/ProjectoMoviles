using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public Rigidbody2D rbd;
    public float fuerza;

    public bool enSuelo;
    public LayerMask suelo;
    public float raycastDistancia = 0.1f;
    void Start()
    {
        Update();
    }

    void Update()
    {
        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, raycastDistancia, suelo);
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo) { 
            rbd.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
        }
    }
}
