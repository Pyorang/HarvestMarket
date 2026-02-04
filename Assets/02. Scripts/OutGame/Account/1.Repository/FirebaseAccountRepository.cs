using UnityEngine;
using Firebase.Auth;
using Cysharp.Threading.Tasks;
using Firebase;

public class FirebaseResourceRepository : IAccountRepository
{
    private FirebaseAuth _auth;

    public FirebaseResourceRepository()
    {
        _auth = FirebaseInitializer.Instance.Auth;
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.Log($"[FirebaseAccountRepository] 회원가입 성공: {result.User.Email}");

            return new AccountResult
            {
                Success = true
            };
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"[FirebaseAccountRepository] 회원가입 실패: {e.Message}");
            return new AccountResult
            {
                Success = false,
                ErrorMessage = GetErrorMessage(e)
            };
        }
    }

    public async UniTask<AccountResult> Login(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            var account = new Account(result.User.Email, password);
            Debug.Log($"[FirebaseAccountRepository] 로그인 성공: {result.User.Email}");

            return new AccountResult
            {
                Success = true,
                Account = account
            };
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"[FirebaseAccountRepository] 로그인 실패: {e.Message}");
            return new AccountResult
            {
                Success = false,
                ErrorMessage = GetErrorMessage(e)
            };
        }
    }

    public void Logout()
    {
        _auth.SignOut();
        Debug.Log("[FirebaseAccountRepository] 로그아웃 완료");
    }

    private string GetErrorMessage(FirebaseException e)
    {
        return e.ErrorCode switch
        {
            (int)AuthError.EmailAlreadyInUse => "이미 사용 중인 이메일입니다.",
            (int)AuthError.InvalidEmail => "유효하지 않은 이메일 형식입니다.",
            (int)AuthError.WeakPassword => "비밀번호가 너무 약합니다. (6자 이상)",
            (int)AuthError.WrongPassword => "이메일 또는 비밀번호를 확인해주세요.",
            (int)AuthError.UserNotFound => "이메일 또는 비밀번호를 확인해주세요.",
            _ => "인증 오류가 발생했습니다."
        };
    }
}
