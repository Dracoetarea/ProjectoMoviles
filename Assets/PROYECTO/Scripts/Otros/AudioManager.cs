using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musica; // Música de fondo

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Cargar volumen guardado o establecer valor por defecto
            float savedVolume = PlayerPrefs.GetFloat("VolumenMusica", 0.5f);
            SetVolume(savedVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Cambia el volumen de la música y guarda el valor en PlayerPrefs
    public void SetVolume(float volume)
    {
        if (musica != null)
        {
            musica.volume = volume;
            PlayerPrefs.SetFloat("VolumenMusica", volume);
            PlayerPrefs.Save();
        }
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
        if (musica == null)
        {
            musica = GetComponent<AudioSource>();
        }

        if (musica != null && !musica.isPlaying)
        {
            musica.Play();
        }
    }
}
