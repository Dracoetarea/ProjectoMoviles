using UnityEngine;
using UnityEngine.UI;

// Controla las opciones del menú, específicamente el volumen de la música
public class OpcionesMenu : MonoBehaviour
{
    public Slider sliderMusica; // Slider UI para controlar el volumen de la música

    void Start()
    {
        if (sliderMusica != null)
        {
            // Obtiene el volumen guardado previamente en PlayerPrefs, o asigna 0.5 por defecto
            float savedVolume = PlayerPrefs.GetFloat("VolumenMusica", 0.5f);
            sliderMusica.value = savedVolume;

            // Añade un listener para detectar cambios en el slider y actualizar el volumen
            sliderMusica.onValueChanged.AddListener(delegate
            {
                CambiarVolumen(sliderMusica.value);
            });
        }
    }

    // Método que cambia el volumen a través del AudioManager
    public void CambiarVolumen(float volumen)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volumen);
        }
    }
}
