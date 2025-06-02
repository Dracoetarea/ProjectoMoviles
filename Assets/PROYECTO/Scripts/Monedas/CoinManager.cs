using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins = 0;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            coinText = FindObjectOfType<TextMeshProUGUI>(); 
        }

        LoadCoinsFromPrefs();
    }



    public void AddCoin()
    {
        coins++;
        PlayerPrefs.SetInt("PlayerCoins", coins); 
        PlayerPrefs.Save();
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coins.ToString();
    }

    public void ResetCoins()
    {
        coins = 0;
        PlayerPrefs.SetInt("PlayerCoins", 0);
        PlayerPrefs.Save();
        UpdateCoinUI();
    }

    public void SaveCoinsToPrefs()
    {
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.Save();
        UpdateCoinUI();
    }
    public void LoadCoinsFromPrefs()
    {
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        UpdateCoinUI();
    }

}
