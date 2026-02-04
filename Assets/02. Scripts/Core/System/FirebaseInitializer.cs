using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Cysharp.Threading.Tasks;

public class FirebaseInitializer : MonoBehaviour
{
    private static FirebaseInitializer s_instance;
    public static FirebaseInitializer Instance
    {
        get
        {
            if (s_instance == null)
            {
                var go = new GameObject("FirebaseInitializer");
                s_instance = go.AddComponent<FirebaseInitializer>();
                DontDestroyOnLoad(go);
            }
            return s_instance;
        }
    }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseFirestore DB { get; private set; }
    public bool IsInitialized { get; private set; }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeFirebaseAsync().Forget();
    }

    private async UniTask InitializeFirebaseAsync()
    {
        var result = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

        if (result == DependencyStatus.Available)
        {
            App = FirebaseApp.DefaultInstance;
            Auth = FirebaseAuth.DefaultInstance;
            DB = FirebaseFirestore.DefaultInstance;
            IsInitialized = true;
            Debug.Log("[FirebaseInitializer] Firebase 초기화 성공!");
        }
        else
        {
            Debug.LogError($"[FirebaseInitializer] Firebase 초기화 실패: {result}");
        }
    }
}
