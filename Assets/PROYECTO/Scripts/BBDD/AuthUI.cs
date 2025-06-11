using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button registerButton;
    public Button loginButton;
    public TMP_Text mensajeTexto;


    void Start()
    {
        // funciones que se ejecutarán al hacer clic en los botones
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);
    }

    public void Register()
    {
        Debug.Log("createUser called");

        // Guardamos los valores introducidos en los campos de entrada en el manager de Firebase
        FirebaseManager.instance.email = emailInput.text;
        FirebaseManager.instance.password = passwordInput.text;
        FirebaseManager.instance.createUser();
        emailInput.text = "";
        passwordInput.text = "";
    }

    public void Login()
    {
        Debug.Log("loginUser called");

        // Guardamos los valores introducidos en los campos de entrada en el manager de Firebase
        FirebaseManager.instance.email = emailInput.text;
        FirebaseManager.instance.password = passwordInput.text;
        FirebaseManager.instance.loginUser(success =>
        {
            if (success)
            {
                mensajeTexto.text = "Login exitoso";
                mensajeTexto.color = Color.green;
                mensajeTexto.gameObject.SetActive(true);
                StartCoroutine(OcultarMensaje(2f));
            }
        });
        emailInput.text = "";
        passwordInput.text = "";
    }

    IEnumerator OcultarMensaje(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        mensajeTexto.gameObject.SetActive(false);
    }
}
