using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musica;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            float savedVolume = PlayerPrefs.GetFloat("VolumenMusica", 0.5f);
            SetVolume(savedVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (musica != null)
        {
            musica.volume = volume;
            PlayerPrefs.SetFloat("VolumenMusica", volume);
            PlayerPrefs.Save();
        }
    }
}
