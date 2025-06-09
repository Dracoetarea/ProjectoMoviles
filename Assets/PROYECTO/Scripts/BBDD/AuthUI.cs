using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button registerButton;
    public Button loginButton;


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
        FirebaseManager.instance.loginUser();
        emailInput.text = "";
        passwordInput.text = "";
    }
}
