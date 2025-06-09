using UnityEngine;

public class followTarget : MonoBehaviour
{
    public Transform objetivo;
    public CharacterLife cf;

    void Update()
    {
        // Hace que un objeto, la (camara de Unity) siga al jugador
        if (cf.jugando == true)
        {
            transform.position = new Vector3(objetivo.transform.position.x, objetivo.transform.position.y, objetivo.transform.position.z);
        }
    }
}
