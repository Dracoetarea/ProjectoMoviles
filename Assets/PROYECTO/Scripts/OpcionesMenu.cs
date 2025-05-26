using UnityEngine;
using UnityEngine.UI;

public class OpcionesMenu : MonoBehaviour
{
    public Slider sliderMusica;

    void Start()
    {
        if (sliderMusica != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumenMusica", 0.5f);
            sliderMusica.value = savedVolume;

            sliderMusica.onValueChanged.AddListener(delegate {
                CambiarVolumen(sliderMusica.value);
            });
        }
    }

    public void CambiarVolumen(float volumen)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volumen);
        }
    }
}
