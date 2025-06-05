using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    private DatabaseReference reference;
    private FirebaseAuth auth;

    public string email;
    public string password;

    [System.Serializable]
    public class PlayerData
    {
        public int monedas;
        public int puntuacion;
    }

    void Start()
    {
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

    public void createUser()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
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

    public void loginUser()
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Logeo cancelado.");
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

        });
    }
    public string GetUserUID()
    {
        var user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
        return user != null ? user.UserId : null;
    }
}
