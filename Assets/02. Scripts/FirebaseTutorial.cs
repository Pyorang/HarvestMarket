using Firebase;

using Firebase.Auth;

using Firebase.Extensions;

using JetBrains.Annotations;

using UnityEngine;



public class FirebaseTutorial : MonoBehaviour

{

    // _app : 파이어베이스에 접근할 수 있는 접근자

    private FirebaseApp _app = null;



    private FirebaseAuth _auth = null;



    private void Start()

    {

        // CheckAndFixDependenciesAsync : 잘 체크 되었는가? 의존성이 잘 매칭 되어있는가? (Async : 비동기 처리)

        // ContinueWithOnMainThread : 콜백 함수 -> 특정 이벤트가 발생하고 나면 자동으로 호출되는 함수

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>

        {

            if (task.Result == DependencyStatus.Available)

            {

                // 1. 파이어베이스 연결에 성공했다면

                _app = FirebaseApp.DefaultInstance;   // 파이어베이스 앱   모듈 가져오기

                _auth = FirebaseAuth.DefaultInstance; // 파이어베이스 인증 모듈 가져오기

                Debug.Log("초기화 성공");

            }

            else

            {

                Debug.LogError("초기화 실패" + task.Result);

            }

        });

    }

    public void Register(string email, string password)

    {

        // 이메일과 패스워드로 회원가입을 시도

        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {

            if (task.IsCanceled)

            {

                Debug.LogError("회원가입이 취소 되었습니다.");

                return;

            }

            if (task.IsFaulted)

            {

                Debug.LogError("회원가입이 실패했습니다. " + task.Exception);

                return;

            }



            // Firebase user has been created.

            Firebase.Auth.AuthResult result = task.Result;

            Debug.LogFormat("회원가입에 성공했습니다. : {0} ({1})", result.User.DisplayName, result.User.UserId);

        });

    }



    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인이 취소 되었습니다.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인이 실패했습니다. " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("로그인에 성공했습니다. : {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }



public void CheckLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user != null)
        {
            Debug.LogFormat("현재 로그인된 유저: {0} ({1})", user.DisplayName, user.UserId);
            Debug.LogFormat("이메일: {0}, 이메일 인증 여부: {1}", user.Email, user.IsEmailVerified);
        }
        else
        {
            Debug.Log("현재 로그인된 유저가 없습니다.");
        }
    }

public void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 되었습니다.");
    }

    private void Update()
    {
        if (_app == null) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("sudangi143@gmail.com", "Sudangi86@");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Login("sudangi143@gmail.com", "Sudangi86@");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckLoginStatus();
        }
    }

}