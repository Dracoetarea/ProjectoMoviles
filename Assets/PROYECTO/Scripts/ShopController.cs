using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt;     // “E → Abrir tienda para comprar”
    public GameObject shopMenu;           // Panel de botones de compra
    public TextMeshProUGUI priceText;     // coste del item
    public TextMeshProUGUI giveText;      // cantidad obtenida

    [Header("Compras")]
    public int heartCost = 5;             // Coste de cada compra
    public int heartValue = 1;            // Corazones que entrega

    [Header("Referencias")]
    public CharacterLife playerLife;

    bool playerInRange = false;

    void Start()
    {
        interactPrompt.SetActive(false);
        shopMenu.SetActive(false);
    }

    void Update()
    {
        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                ToggleShop();
            }

            if (shopMenu.activeSelf && Input.GetKeyDown(KeyCode.B))
            {
                BuyHearts();
            }
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        bool opening = !shopMenu.activeSelf;

        shopMenu.SetActive(opening);
        interactPrompt.SetActive(!opening);

        if (opening)
        {
            Time.timeScale = 0f;  // Pausar el juego al abrir
        }
        else
        {
            Time.timeScale = 1f;  // Reanudar al cerrar
        }
    }
    public void BuyHearts()
    {
        int balance = CoinManager.instance.coins;
        if (balance >= heartCost)
        {
            CoinManager.instance.coins -= heartCost;
            CoinManager.instance.UpdateCoinUI(); 

            playerLife.AddHearts(heartValue);
        }
        else
        {
            Debug.Log("No tienes suficientes monedas para comprar corazones.");
        }
    }

    // Detectar al jugador al entrar en la tienda
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = true;
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = false;
            interactPrompt.SetActive(false);
            if (shopMenu.activeSelf)
                ToggleShop();
        }
    }
}
