using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt;     // “E → Abrir tienda para comprar”
    public GameObject shopMenu;           // Panel de botones de compra
    public TextMeshProUGUI priceText;     // coste del item
    public TextMeshProUGUI giveText;      // cantidad obtenida

    [Header("UI Android")]
    public GameObject botonAbrirTiendaAndroid;
    public GameObject botonComprarAndroid;

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
#if UNITY_ANDROID
        botonAbrirTiendaAndroid.SetActive(false);
        botonComprarAndroid.SetActive(false);
#else
        botonAbrirTiendaAndroid.SetActive(false);
        botonComprarAndroid.SetActive(false);
#endif
    }

    void Update()
    {
        // Si el jugador está cerca y presiona 'E', se abre o cierra la tienda
#if !UNITY_ANDROID
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }

        if (shopMenu.activeSelf && Input.GetKeyDown(KeyCode.B))
        {
            BuyHearts();
        }
#endif
    }

    // Método para abrir/cerrar la tienda y pausar o reanudar el juego
    public void ToggleShop()
    {
        bool opening = !shopMenu.activeSelf;

        shopMenu.SetActive(opening);
        interactPrompt.SetActive(!opening);

#if UNITY_ANDROID
        botonComprarAndroid.SetActive(opening);
#endif

        Time.timeScale = opening ? 0f : 1f;
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
#if UNITY_ANDROID
            botonAbrirTiendaAndroid.SetActive(true);
#endif
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

#if UNITY_ANDROID
            botonAbrirTiendaAndroid.SetActive(false);
            botonComprarAndroid.SetActive(false);
#endif
        }
    }
}
