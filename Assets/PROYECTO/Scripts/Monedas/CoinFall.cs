using UnityEngine;

public class CoinFall : MonoBehaviour
{
    public float fallSpeed = 2f;
    private bool hasLanded = false;

    void Update()
    {
        if (!hasLanded)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SueloCueva"))
        {
            hasLanded = true;
        }
    }
}
