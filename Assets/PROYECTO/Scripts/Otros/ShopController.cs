using TMPro;
using UnityEngine;

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
    CoinManager coinManager;

    void Start()
    {
        // Inicialmente ocultamos la UI de interacción y la tienda
        interactPrompt.SetActive(false);
        shopMenu.SetActive(false);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona 'E', se abre o cierra la tienda
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }

        // Si la tienda está abierta y el jugador presiona 'B', se realiza la compra de corazones
        if (shopMenu.activeSelf && Input.GetKeyDown(KeyCode.B))
        {
            BuyHearts();
        }
    }

    // Método para abrir/cerrar la tienda y pausar o reanudar el juego
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

    // Método para comprar corazones si hay suficientes monedas
    public void BuyHearts()
    {
        int balance = CoinManager.instance.coins;
        if (balance >= heartCost)
        {
            CoinManager.instance.SpendCoins(heartCost);

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

    // Detecta cuando el jugador sale del área para ocultar la UI y cerrar tienda si estaba abierta
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
