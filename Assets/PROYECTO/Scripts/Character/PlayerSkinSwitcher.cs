using UnityEngine;
using System.Collections;

public class PlayerSkinSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private int skinActual;
    private Sprite[] nuevaSkin2;
    private Sprite[] nuevaSkin3;
    public RuntimeAnimatorController animatorSkin2;
    public RuntimeAnimatorController animatorSkin3;
    public GameObject iconoCambioSkin;

    private AudioSource audioSource;

    void Start()
    {
        nuevaSkin2 = Resources.LoadAll<Sprite>("NewSkin2");
        nuevaSkin3 = Resources.LoadAll<Sprite>("NewSkin3");

        audioSource = GetComponent<AudioSource>();
    }

    public void CambiarSkin()
    {
        if (skinActual >= 2 || nuevaSkin2.Length == 0)
            return;

        skinActual = 2;
        spriteRenderer.sprite = null;
        GetComponent<Animator>().runtimeAnimatorController = animatorSkin2;
        spriteRenderer.sprite = nuevaSkin2[0];

        StartCoroutine(MostrarIconoCambio());
    }

    public void CambiarSkin3()
    {
        if (skinActual >= 3 || nuevaSkin3.Length == 0)
            return;

        skinActual = 3;
        spriteRenderer.sprite = null;
        GetComponent<Animator>().runtimeAnimatorController = animatorSkin3;
        spriteRenderer.sprite = nuevaSkin3[0];

        StartCoroutine(MostrarIconoCambio());
    }

    private IEnumerator MostrarIconoCambio()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        iconoCambioSkin.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        iconoCambioSkin.SetActive(false);
    }
}
