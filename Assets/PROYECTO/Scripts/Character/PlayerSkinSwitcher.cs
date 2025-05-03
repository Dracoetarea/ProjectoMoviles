using UnityEngine;

public class PlayerSkinSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private int skinActual;
    private Sprite[] nuevaSkin2;
    private Sprite[] nuevaSkin3;
    public RuntimeAnimatorController animatorSkin2;
    public RuntimeAnimatorController animatorSkin3;

    void Start()
    {
        nuevaSkin2 = Resources.LoadAll<Sprite>("NewSkin2");
        nuevaSkin3 = Resources.LoadAll<Sprite>("NewSkin3");
    }

    public void CambiarSkin()
    {
        if (skinActual >=2 || nuevaSkin2.Length == 0)
            return;

        skinActual = 2;
        spriteRenderer.sprite = null;
        GetComponent<Animator>().runtimeAnimatorController = animatorSkin2;
        spriteRenderer.sprite = nuevaSkin2[0];
    }
    public void CambiarSkin3()
    {
        if (skinActual >=3 || nuevaSkin3.Length == 0)
            return;

        skinActual = 3;
        spriteRenderer.sprite = null;
        GetComponent<Animator>().runtimeAnimatorController = animatorSkin3;
        spriteRenderer.sprite = nuevaSkin3[0];
    }
}
