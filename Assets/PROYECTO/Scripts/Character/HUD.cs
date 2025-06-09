using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image[] vidas; // array de vidas en UID
    public Sprite corazonLleno; // Sprite del corazón lleno
    public Sprite corazonMedio; // Sprite del corazón a media vida
    public Sprite corazonVacio; // Sprite del corazón vacío

    public GameObject gameOverPanel; // Panel que aparece cuando se termina la partida            

    // Cambia el sprite del corazón indicado, reduciendo la vida visualmente
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

    // Método público que se puede llamar desde otros scripts cuando el personaje recibe daño
    public void recibirGolpe()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            if (vidas[i].sprite == corazonLleno || vidas[i].sprite == corazonMedio)
            {
                // Encuentra el primer corazón que no está vacío y le quita vida
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
