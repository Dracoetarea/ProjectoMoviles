using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorazonesUI : MonoBehaviour
{
    public List<Image> listaCorazones;
    public GameObject corazonPrefab;
    public CharacterLife vidaJugador;
    public int indexActual;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    private void Awake()
    {
        vidaJugador.cambioVida.AddListener(cambiarCorazones);
    }

    private void cambiarCorazones(int vidaActual)
    {
        // Si la cantidad de vida actual supera los corazones existentes, crea mas
        if (vidaActual > listaCorazones.Count)
        {
            int cantidadACrear = vidaActual - listaCorazones.Count;

            for (int i = 0; i < cantidadACrear; i++)
            {
                GameObject nuevoCorazon = Instantiate(corazonPrefab, transform);
                listaCorazones.Add(nuevoCorazon.GetComponent<Image>());
            }
        }

        // Actualiza los sprites seg�n la vida actual
        CambiarVida(vidaActual);
    }

    // Hace el cambio de imagen para los corazones
    private void CambiarVida(int vidaActual)
    {
        for (int i = 0; i < listaCorazones.Count; i++)
        {
            if (i < vidaActual)
            {
                listaCorazones[i].sprite = corazonLleno;
            }
            else
            {
                listaCorazones[i].sprite = corazonVacio;
            }
        }
    }
}
