using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image[] vidas; // array de vidas en UID
    public Sprite corazonLleno; // Sprite del coraz�n lleno
    public Sprite corazonMedio; // Sprite del coraz�n a media vida
    public Sprite corazonVacio; // Sprite del coraz�n vac�o

    public GameObject gameOverPanel; // Panel que aparece cuando se termina la partida            

    // Cambia el sprite del coraz�n indicado, reduciendo la vida visualmente
    private void quitarVida(int indice)
    {
        if (vidas[indice].sprite == corazonLleno)
        {

            vidas[indice].sprite = corazonMedio;
        }
        else if (vidas[indice].sprite == corazonMedio)
        {
            vidas[indice].sprite = corazonVacio;
        }
    }

    // M�todo p�blico que se puede llamar desde otros scripts cuando el personaje recibe da�o
    public void recibirGolpe()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            if (vidas[i].sprite == corazonLleno || vidas[i].sprite == corazonMedio)
            {
                // Encuentra el primer coraz�n que no est� vac�o y le quita vida
                quitarVida(i);
                break;
            }
        }

        // Comprueba si se han perdido todas las vidas
        terminarJuego();
    }

    // Muestra el panel de Game Over
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
