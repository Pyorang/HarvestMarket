using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    private IAccountRepository _repository;
    private Account _currentAccount = null;

    public bool IsLoggedIn => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _repository = new FirebaseResourceRepository();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async UniTask<AccountResult> TryLogin(string email, string password)
    {
        try
        {
            var account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }

        var result = await _repository.Login(email, password);

        if (result.Success)
        {
            _currentAccount = result.Account;
            UserDataManager.Instance.Initialize(email).Forget();
        }

        return result;
    }

    public async UniTask<AccountResult> TryRegister(string email, string password, string passwordConfirm)
    {
        if (password != passwordConfirm)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = "비밀번호 확인이 일치하지 않습니다."
            };
        }

        try
        {
            var account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }

        var result = await _repository.Register(email, password);

        return result;
    }

    public void Logout()
    {
        _repository.Logout();
        _currentAccount = null;
    }
}
