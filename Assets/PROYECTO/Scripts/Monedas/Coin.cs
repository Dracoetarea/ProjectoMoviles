using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCollected = false; // Evita recoger varias veces la misma moneda

    public AudioClip coinSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la moneda ya fue recogida, no hacer nada
        if (isCollected) return;

        // Detecta si el objeto que colisiona tiene el Tag "Jugador"
        if (collision.CompareTag("Jugador"))
        {
            isCollected = true;

            if (audioSource != null && coinSound != null)
            {
                audioSource.PlayOneShot(coinSound);
            }

            // Suma una moneda al CoinManager
            CoinManager.instance.AddCoin();
            // Destruye el objeto moneda después de que termine el sonido
            Destroy(gameObject, coinSound != null ? coinSound.length : 0f);
        }
    }
}
