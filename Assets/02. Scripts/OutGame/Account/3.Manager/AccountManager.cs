using System;
using UnityEngine;

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
            _repository = new LocalAccountRepository();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AuthResult TryLogin(string email, string password)
    {
        try
        {
            var account = new Account(email, password);
            var stored = _repository.FindByEmail(email);

            if (stored == null)
                return AuthResult.Fail("Please check your email and password.");

            if (!PasswordHasher.Verify(password, stored.HashedPassword))
                return AuthResult.Fail("Please check your email and password.");

            _currentAccount = account;
            UserDataManager.Instance.Initialize(email);

            return AuthResult.Ok(account);
        }
        catch (ArgumentException e)
        {
            return AuthResult.Fail(e.Message);
        }
    }

    public AuthResult TryRegister(string email, string password, string passwordConfirm)
    {
        if (password != passwordConfirm)
            return AuthResult.Fail("Password confirmation does not match.");

        try
        {
            var account = new Account(email, password);

            if (_repository.Exists(email))
                return AuthResult.Fail("This email is already in use.");

            var data = new AccountData(email, PasswordHasher.Hash(password));
            _repository.Save(data);

            UserDataManager.Instance.Initialize(email);

            return AuthResult.Ok(account);
        }
        catch (ArgumentException e)
        {
            return AuthResult.Fail(e.Message);
        }
    }

    public void Logout()
    {
        _currentAccount = null;
    }
}
