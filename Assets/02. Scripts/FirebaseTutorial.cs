using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using Cysharp.Threading.Tasks;

public class FirebaseTutorial : MonoBehaviour
{
    private FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    private TextMeshProUGUI _progressText;
    [SerializeField] private TMP_Text _statusText;

    private async void Start()
    {
        await InitializeFirebaseAsync();
        UpdateStatus("1. Firebase 초기화 완료");

        Logout();
        UpdateStatus("2. 로그아웃 완료");

        await LoginAsync("hongil@skku.re.kr", "12345678");
        UpdateStatus("3. 로그인 완료");

        await SaveDogAsync();
        UpdateStatus("4. 강아지 추가 완료");
    }

    private void UpdateStatus(string message)
    {
        if (_statusText != null)
        {
            _statusText.text = message;
        }
        Debug.Log(message);
    }

    private async UniTask InitializeFirebaseAsync()
    {
        var result = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (result == DependencyStatus.Available)
        {
            _app = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;
            _db = FirebaseFirestore.DefaultInstance;
            Debug.Log("Firebase 초기화 성공!");
        }
        else
        {
            Debug.LogError("Firebase 초기화 실패: " + result);
        }
    }


    private void Register(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("회원가입이 실패했습니다: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("회원가입에 성공했습니다.: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }

    private async UniTask LoginAsync(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.LogFormat("로그인 성공!: {0} ({1})", result.User.Email, result.User.UserId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }

    private void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 성공!");
    }

    private void CheckLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            Debug.Log("로그인 안됨");
        }
        else
        {
            Debug.LogFormat("로그인 중: {0} ({1})", user.Email, user.UserId);
        }
    }

    private async UniTask SaveDogAsync()
    {
        try
        {
            Dog dog = new Dog("소똥이", 4);
            var docRef = await _db.Collection("Dogs").AddAsync(dog);
            Debug.Log("저장 성공! 문서 ID: " + docRef.Id);
        }
        catch (System.Exception e)
        {
            Debug.LogError("저장 실패: " + e.Message);
        }
    }

    private void LoadMyDog()
    {
        _db.Collection("Dogs").Document("홍일이 개").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name}({myDog.Age})");
                }
                else
                {
                    Debug.LogError("데이터가 없습니다.");
                }
            }
            else
            {
                Debug.LogError("불러오기 실패: " + task.Exception);
            }
        });

    }

    private void LoadDogs()
    {
        _db.Collection("Dogs").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                Debug.Log("강아지들-------------------------------------------");
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name}({myDog.Age})");
                }

                Debug.LogError("불러오기 성공!");
            }
            else
            {
                Debug.LogError("불러오기 실패: " + task.Exception);
            }
        });
    }

    private void DeleteDogs()
    {
        // 목표: 소똥이들 삭제
        _db.Collection("Dogs").WhereEqualTo("Name", "소똥이").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                Debug.Log("강아지들-------------------------------------------");
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    if (myDog.Name == "소똥이")
                    {
                        _db.Collection("Dogs").Document(myDog.Id).DeleteAsync().ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompletedSuccessfully)
                            {
                                Debug.Log("데이터가 삭제됐습니다.");
                            }
                        });
                    }
                }

                Debug.LogError("불러오기 성공!");
            }
            else
            {
                Debug.LogError("불러오기 실패: " + task.Exception);
            }
        });
    }


    private void Update()
    {
        if (_app == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("hongil@skku.re.kr", "12345678");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoginAsync("hongil@skku.re.kr", "12345678").Forget();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckLoginStatus();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveDogAsync().Forget();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadMyDog();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadDogs();
        }
    }

}