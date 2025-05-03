using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class CharacterLife : MonoBehaviour
{
    public int vidaActual;
    public int vidaMaxima;
    public UnityEvent<int> cambioVida;
    public GameObject HasMuertoPanel;
    public TextMeshProUGUI HasMuertoTexto;
    public float fadeDuration = 3f;
    public GameObject MenuMuerte;
    public GameObject MenuPrincipal;
    public Boolean jugando = false;
    public GameObject imagen1;
    public GameObject imagen2;
    public Spawner[] spawners;

    private bool juegoIniciado = false; // Bandera para controlar el inicio del juego.
    private MonoBehaviour[] componentesAdesactivar; // Componentes a desactivar al inicio

    void Start()
    {
        HasMuertoPanel.SetActive(false);
        MenuMuerte.SetActive(false);
        MenuPrincipal.SetActive(true);

        componentesAdesactivar = GetComponents<MonoBehaviour>();
        DesactivarComponentesJuego();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (juegoIniciado)
        {
            Debug.Log("Colisión con: " + other.name);
            if (other.CompareTag("Enemigo"))
            {
                recibirDaño(1);
            }
        }
    }

    public void recibirDaño(int cantidadDaño)
    {
        if (juegoIniciado) // Solo recibir daño si el juego ha comenzado
        {
            int vidaTemporal = vidaActual - cantidadDaño;

            if (vidaTemporal < 0)
            {
                vidaActual = 0;
            }
            else
            {
                vidaActual = vidaTemporal;
            }
            cambioVida.Invoke(vidaActual);

            if (vidaActual <= 0)
            {
                StartCoroutine(MostrarMenuMuerte());
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;
            }
        }
    }

    private IEnumerator MostrarMenuMuerte()
    {
        HasMuertoPanel.SetActive(true);
        StartCoroutine(FadeOutText());
        yield return new WaitForSeconds(fadeDuration);
        HasMuertoPanel.SetActive(false);
        MenuMuerte.SetActive(true);
    }
    private IEnumerator FadeOutText()
    {
        Color originalColor = HasMuertoTexto.color;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            HasMuertoTexto.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }

    public void Reintentar()
    {
        juegoIniciado = false;
        jugando = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirJuegoMuerte()
    {
        GetComponent<Renderer>().enabled = true;
        ActivarComponentesJuego();
        vidaActual = vidaMaxima;
        cambioVida.Invoke(vidaActual);
        jugando = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        MenuPrincipal.SetActive(false);
        MenuMuerte.SetActive(false);
    }

    public void Jugar()
    {
        MenuPrincipal.SetActive(false);
        GetComponent<Renderer>().enabled = true; // Activar el renderizador
        juegoIniciado = true; // Marca el juego como iniciado.
        ActivarComponentesJuego();
        cambioVida.Invoke(vidaActual);
        imagen1.SetActive(false);
        imagen2.SetActive(false);
        jugando = true;
    }

    public void Opciones()
    {
        Debug.Log("Opciones del juego");
    }
    public void SalirJuegoPrincipal()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
    public void UnaFuncionQueNoHagaNada(int vida)
    {

    }
    private void DesactivarComponentesJuego()
    {
        foreach (MonoBehaviour componente in componentesAdesactivar)
        {
            if (componente != this)
            {
                componente.enabled = false;
            }
        }
    }
    private void ActivarComponentesJuego()
    {
        foreach (MonoBehaviour componente in componentesAdesactivar)
        {
            componente.enabled = true;
        }
    }

    public void AddHearts(int amount)
    {
        vidaActual = Mathf.Min(vidaActual + amount, vidaMaxima);
        cambioVida.Invoke(vidaActual);
    }
}