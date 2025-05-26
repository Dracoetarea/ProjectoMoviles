using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCollected = false;

    public AudioClip coinSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Jugador"))
        {
            isCollected = true;

            if (audioSource != null && coinSound != null)
            {
                audioSource.PlayOneShot(coinSound);
            }

            CoinManager.instance.AddCoin();
            Destroy(gameObject, coinSound != null ? coinSound.length : 0f);
        }
    }
}
