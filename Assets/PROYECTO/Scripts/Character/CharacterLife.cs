using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterLife : MonoBehaviour
{
    [Header("Vida")]
    public int vidaActual;
    public int vidaMaxima;
    public UnityEvent<int> cambioVida;

    [Header("UI y Menu")]
    public GameObject HasMuertoPanel;
    public TextMeshProUGUI HasMuertoTexto;
    public float fadeDuration = 3f;
    public GameObject MenuMuerte;
    public GameObject MenuPrincipal;
    public GameObject imagen1;
    public GameObject imagen2;
    public TextMeshProUGUI puntuacionTexto;

    [Header("Inmortalidad")]
    public float duracionInmortalidad = 1f;

    [Header("Otros")]
    public bool jugando = false;
    public Spawner[] spawners;

    private bool juegoIniciado = false;
    private bool esInvulnerable = false;
    private MonoBehaviour[] componentesAdesactivar;
    public GameObject MenuOpciones;
    public AudioSource gameOverAudioSource;
    public AudioSource musicaFondoAudioSource;
    public AudioSource hitAudioSource;

    void Start()
    {
        HasMuertoPanel.SetActive(false);
        MenuMuerte.SetActive(false);
        MenuPrincipal.SetActive(true);

        componentesAdesactivar = GetComponents<MonoBehaviour>();
        DesactivarComponentesJuego();

        if (PlayerPrefs.HasKey("VidaActual"))
        {
            vidaActual = PlayerPrefs.GetInt("VidaActual");
        }

        CoinManager.instance?.LoadCoinsFromPrefs();
        ScoreManager.instance?.ResetScore();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (juegoIniciado && !esInvulnerable)
        {
            if (other.CompareTag("Enemigo"))
            {
                hitAudioSource.Play();
                recibirDaño(1);
                StartCoroutine(InmortalidadTemporal());
            }
        }
    }

    public void recibirDaño(int cantidadDaño)
    {
        if (juegoIniciado)
        {
            int vidaTemporal = vidaActual - cantidadDaño;
            vidaActual = Mathf.Max(vidaTemporal, 0);
            cambioVida.Invoke(vidaActual);

            if (vidaActual <= 0)
            {
                if (musicaFondoAudioSource != null && musicaFondoAudioSource.isPlaying)
                {
                    musicaFondoAudioSource.Stop();
                }

                if (gameOverAudioSource != null)
                {
                    gameOverAudioSource.Play();
                }
                StartCoroutine(MostrarMenuMuerte());
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;
            }
        }
    }

    private IEnumerator InmortalidadTemporal()
    {
        esInvulnerable = true;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float tiempoTranscurrido = 0f;
        float intervaloParpadeo = 0.1f;

        while (tiempoTranscurrido < duracionInmortalidad)
        {
            if (renderer != null)
                renderer.enabled = !renderer.enabled;

            yield return new WaitForSeconds(intervaloParpadeo);
            tiempoTranscurrido += intervaloParpadeo;
        }

        if (renderer != null)
            renderer.enabled = true; 

        esInvulnerable = false;
    }


    private IEnumerator MostrarMenuMuerte()
    {
        HasMuertoPanel.SetActive(true);
        StartCoroutine(FadeOutText());
        yield return new WaitForSeconds(fadeDuration);
        HasMuertoPanel.SetActive(false);
        if (puntuacionTexto != null)
        {
            int puntos = ScoreManager.instance != null ? ScoreManager.instance.currentScore : 0;
            puntuacionTexto.text += puntos.ToString();
        }
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
        CoinManager.instance?.LoadCoinsFromPrefs();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreManager.instance?.ResetScore();
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
        GetComponent<Renderer>().enabled = true;
        juegoIniciado = true;
        ActivarComponentesJuego();
        cambioVida.Invoke(vidaActual);
        imagen1.SetActive(false);
        imagen2.SetActive(false);
        jugando = true;
    }

    public void Opciones()
    {
        if (MenuOpciones != null)
        {
            MenuOpciones.SetActive(true);
        }
    }

    public void SalirJuegoPrincipal()
    {
        Application.Quit();
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
        PlayerPrefs.SetInt("VidaActual", vidaActual);
        PlayerPrefs.Save();
        cambioVida.Invoke(vidaActual);
    }
    public void CerrarOpciones()
    {
        if (MenuOpciones != null)
        {
            MenuOpciones.SetActive(false);
        }
    }
}
