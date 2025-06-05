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
        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);
    }

    public void Register()
    {
        Debug.Log("createUser called");

        FirebaseManager.instance.email = emailInput.text;
        FirebaseManager.instance.password = passwordInput.text;
        FirebaseManager.instance.createUser();
    }

    public void Login()
    {
        Debug.Log("loginUser called");

        FirebaseManager.instance.email = emailInput.text;
        FirebaseManager.instance.password = passwordInput.text;
        FirebaseManager.instance.loginUser();
    }
}
