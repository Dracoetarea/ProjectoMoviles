using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        // Actualiza los sprites según la vida actual
        CambiarVida(vidaActual);
    }



    private void CrearCorazones(int cantidadVidaMaxima)
    {

        for (int i = 0; i < cantidadVidaMaxima; i++) { 
            GameObject corazon = Instantiate(corazonPrefab, transform);

            listaCorazones.Add(corazon.GetComponent<Image>());
        }
        indexActual = cantidadVidaMaxima - 1;
    }
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



    private void agregarCorazon(int vidaActual)
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
    private void quitarCorazon(int vidaActual)
    {
        for (int i = 0; i < listaCorazones.Count; i++) {
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
