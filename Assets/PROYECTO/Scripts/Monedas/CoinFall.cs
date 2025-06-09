using UnityEngine;

public class CoinFall : MonoBehaviour
{
    public float fallSpeed = 2f; // Velocidad a la que cae la moneda
    private bool hasLanded = false; // Indica si la moneda ha tocado el suelo

    void Update()
    {
        if (!hasLanded)
        {
            // Mueve la moneda hacia abajo
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cuando la moneda colisiona con el tag "SueloCueva" se marca que ha caido
        if (collision.CompareTag("SueloCueva"))
        {
            hasLanded = true;
        }
    }
}
