using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterLife : MonoBehaviour
{
    [Header("Vida")]
    public int vidaActual;
    public int vidaMaxima;
    // Evento para actualizar la UI al cambiar la vida
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
    private bool juegoPausado = false;

    [Header("Inmortalidad")]
    public float duracionInmortalidad = 1f;

    [Header("UI Android")]
    public GameObject UITactilAndroid;

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
    public GameObject MenuStats;
    public TextMeshProUGUI textoStats;

    void Start()
    {

#if UNITY_ANDROID
        UITactilAndroid.SetActive(true);
#else
        UITactilAndroid.SetActive(false);
#endif

        HasMuertoPanel.SetActive(false);
        MenuMuerte.SetActive(false);
        MenuPrincipal.SetActive(true);

        // Desactivamos todos los scripts menos este al iniciar
        componentesAdesactivar = GetComponents<MonoBehaviour>();
        DesactivarComponentesJuego();

        if (PlayerPrefs.HasKey("VidaActual"))
        {
            vidaActual = PlayerPrefs.GetInt("VidaActual");
        }

        // Cargamos las monedas acumuladas y reiniciamos la puntuacion anterior
        CoinManager.instance?.LoadCoinsFromPrefs();
        ScoreManager.instance?.ResetScore();
    }

    void Update()
    {
        // Pausar/Reanudar con ESC si el juego ha comenzado y el jugador está vivo
        if (juegoIniciado && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausa();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo recibimos daño si el juego ha comenzado y no estamos en modo inmortal
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
            // Reducimos la vida, asegurándonos que no baje de 0
            int vidaTemporal = vidaActual - cantidadDaño;
            vidaActual = Mathf.Max(vidaTemporal, 0);
            cambioVida.Invoke(vidaActual);

            if (vidaActual <= 0)
            {
                // Sonido y acciones tras morir
                if (musicaFondoAudioSource != null && musicaFondoAudioSource.isPlaying)
                {
                    musicaFondoAudioSource.Stop();
                }

                if (gameOverAudioSource != null)
                {
                    gameOverAudioSource.Play();
                }
                OcultarPersonaje();
                StartCoroutine(MostrarMenuMuerte());
            }
        }
    }

    // "Efecto invulnerabilidad", hace parpadear al personaje y lo vuelve invulnerable temporalmente tras recibir daño
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

    // Muestra el panel de muerte, guarda estadísticas y muestra el menú correspondiente
    private IEnumerator MostrarMenuMuerte()
    {
        HasMuertoPanel.SetActive(true);
        StartCoroutine(FadeOutText());

        yield return new WaitForSeconds(fadeDuration);

        HasMuertoPanel.SetActive(false);

        int puntos = ScoreManager.instance != null ? ScoreManager.instance.currentScore : 0;
        if (puntuacionTexto != null)
        {
            puntuacionTexto.text = "PUNTOS: " + puntos.ToString();
        }
        MenuMuerte.SetActive(true);

        // Guardar datos en segundo plano (sin bloquear el menú)
        string userUID = FirebaseManager.instance.GetUserUID();
        int coins = CoinManager.instance != null ? CoinManager.instance.GetCoins() : 0;

        Debug.Log($"Guardando stats en Firebase: {userUID}, {coins}, {puntos}");
        FirebaseManager.instance.SavePlayerStats(userUID, coins, puntos);
    }


    // Hace el texto de “Has Muerto” desvanecerse progresivamente
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

    // Reinicia la escena actual y resetea la puntuación
    public void Reintentar()
    {
        juegoIniciado = false;
        jugando = false;
        CoinManager.instance?.LoadCoinsFromPrefs();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ScoreManager.instance?.ResetScore();
    }

    // Reiniciamos el personaje y volvemos al juego desde el menú de muerte
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

    // Comenzar partida desactivando el HUD del menu principal al darle al boton jugar, carga los componentes necesarios
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

    // Habilita el menu de opciones
    public void Opciones()
    {
        if (MenuOpciones != null)
        {
            MenuOpciones.SetActive(true);
        }
    }

    // Cierra la aplicacion
    public void SalirJuegoPrincipal()
    {
        Application.Quit();
    }

    // Desactiva scripts
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

    // Activa los scripts
    private void ActivarComponentesJuego()
    {
        foreach (MonoBehaviour componente in componentesAdesactivar)
        {
            componente.enabled = true;
        }
    }

    // Añade vida y la guarda
    public void AddHearts(int amount)
    {
        vidaActual = Mathf.Min(vidaActual + amount, vidaMaxima);
        PlayerPrefs.SetInt("VidaActual", vidaActual);
        PlayerPrefs.Save();
        cambioVida.Invoke(vidaActual);
    }

    // Cierra las opciones
    public void CerrarOpciones()
    {
        if (MenuOpciones != null)
        {
            MenuOpciones.SetActive(false);
        }
    }

    // Muestra los disparos realizados, enemigos eliminados, y la puntuacion conseguida 
    public void MostrarStats()
    {
        if (MenuStats != null && textoStats != null)
        {
            StatsManager stats = StatsManager.instance;
            textoStats.text = $"DISPAROS: {stats.disparosRealizados}\nELIMINADOS: {stats.enemigosEliminados}\nMAYOR PTS: {stats.puntuacionMaxima}";
            MenuStats.SetActive(true);
        }
    }

    // Cerrar las stats
    public void CerrarStats()
    {
        if (MenuStats != null)
        {
            MenuStats.SetActive(false);
        }
    }

    // Ocultar el personaje
    private void OcultarPersonaje()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        foreach (SpriteRenderer childSr in GetComponentsInChildren<SpriteRenderer>())
        {
            childSr.enabled = false;
            childSr.sortingOrder = -100;
        }
        if (TryGetComponent<Collider2D>(out var collider)) collider.enabled = false;
        if (TryGetComponent<Rigidbody2D>(out var rb)) rb.simulated = false;
    }
    void TogglePausa()
    {
        juegoPausado = !juegoPausado;

        if (juegoPausado)
        {
            Time.timeScale = 0f;
            if (MenuOpciones != null)
                // Mostrar menu
                MenuOpciones.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (MenuOpciones != null)
                // Ocultar Menu
                MenuOpciones.SetActive(false);
        }
    }
}
