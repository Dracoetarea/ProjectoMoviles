using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Jugador"))
        {
            isCollected = true;
            CoinManager.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
