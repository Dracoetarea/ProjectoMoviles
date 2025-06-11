using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    private DatabaseReference reference;
    private FirebaseAuth auth;
    public string email;
    public string password;

    // Clase para guardar los datos del jugador en la base de datos
    [System.Serializable]
    public class PlayerData
    {
        public int monedas;
        public int puntuacion;
    }

    void Start()
    {
        // comprobacion para que no se duplique al cambiar de escena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebaseAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeFirebaseAsync()
    {
        // inicializacion de Firebase de forma asíncrona
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                app.Options.DatabaseUrl = new System.Uri("https://proyectounityshadetrigger-default-rtdb.firebaseio.com/");
                reference = FirebaseDatabase.GetInstance(app).RootReference;
                Debug.Log("Firebase Inicializado correctamente.");
            }
            else
            {
                Debug.LogError($"No se pudieron resolver las dependencias de Firebase: {dependencyStatus}");
            }
        });
    }

    // Guarda las estadísticas del jugador en la base de datos
    public void SavePlayerStats(string userId, int coins, int score)
    {
        var playerData = new PlayerData()
        {
            monedas = coins,
            puntuacion = score
        };
        string json = JsonUtility.ToJson(playerData);
        reference.Child("players").Child(userId).SetRawJsonValueAsync(json);
    }

    // Crea un nuevo usuario en Firebase usando email y contraseña
    public void createUser()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Registro cancelado.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Error al registrarse: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Usuario de firebase creado Correctamente: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

        });
    }

    // Inicia sesión con los datos proporcionados
    public void loginUser(System.Action<bool> onComplete = null)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Error al logearse: " + (task.Exception != null ? task.Exception.ToString() : "Cancelado"));
                onComplete?.Invoke(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Error al logearse: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Usuario Logeado correctamente: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            onComplete?.Invoke(true);
        });
    }

    // Devuelve el UID del usuario actualmente logueado
    public string GetUserUID()
    {
        var user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        return user != null ? user.UserId : null;
    }
}
