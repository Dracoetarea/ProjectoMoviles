using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0; // Contador de monedas
    public TextMeshProUGUI coinText; // Texto UI para mostrar las monedas
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir entre escenas (mantener las monedas)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (coinText == null)
        {
            coinText = Object.FindFirstObjectByType<TextMeshProUGUI>();

        }

        LoadCoinsFromPrefs(); // Carga las monedas guardadas en prefs
        UpdateCoinUI();
    }

    // Incrementa el contador de monedas
    public void AddCoin()
    {
        coins++;
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
        UpdateCoinUI();
    }

    // Actualiza el texto con la cantidad actual de monedas
    public void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }

    // Resetea el contador de monedas a 0
    public void ResetCoins()
    {
        coins = 0;
        PlayerPrefs.SetInt("PlayerCoins", 0);
        PlayerPrefs.Save();
        UpdateCoinUI();
    }

    // Guarda el estado actual de monedas en PlayerPrefs
    public void SaveCoinsToPrefs()
    {
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
    }

    // Resta monedas cuando se gastan en la tienda
    public void SpendCoins(int amount)
    {
        coins -= amount;
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
        UpdateCoinUI();
    }

    // Carga las monedas guardadas de PlayerPrefs
    public void LoadCoinsFromPrefs()
    {
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        UpdateCoinUI();
    }

    // Devuelve la cantidad actual de monedas
    public int GetCoins()
    {
        return coins;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Vuelve a buscar el CoinCounter después de cargar la nueva escena
        if (coinText == null)
        {
            GameObject go = GameObject.Find("CoinCounter");
            if (go != null)
            {
                coinText = go.GetComponent<TextMeshProUGUI>();
                UpdateCoinUI();
            }
        }
    }
}
