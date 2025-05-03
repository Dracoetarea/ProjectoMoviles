using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image[] vidas;
    public Sprite corazonLleno;
    public Sprite corazonMedio;
    public Sprite corazonVacio;

    public GameObject gameOverPanel;
    void Start()
    {
    }

    void Update()
    {
        
    }

    private void quitarVida(int indice)
    {
        if (vidas[indice].sprite == corazonLleno ) {

            vidas[indice].sprite = corazonMedio;
        }
        else if (vidas[indice].sprite == corazonMedio)
        {
            vidas[indice].sprite = corazonVacio;
        }
    }

    public void recibirGolpe()
    {
        for (int i = 0; i < vidas.Length; i++) {
            if (vidas[i].sprite == corazonLleno || vidas[i].sprite == corazonMedio)
            {
                quitarVida(i);
                break;
            }
        }
        terminarJuego();
    }

    private void terminarJuego()
    {

        foreach (Image vida in vidas)
        {
            if (vidas[2].sprite == corazonVacio)
            {
                gameOverPanel.SetActive(true);

                Time.timeScale = 0f;

            }
        }

    }

}
