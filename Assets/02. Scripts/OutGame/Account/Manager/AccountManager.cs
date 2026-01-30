using System;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    private Account _currentAccount = null;
    public bool IsLoggedIn => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

            if (!PlayerPrefs.HasKey(email))
            {
                return AuthResult.Fail("Please check your email and password.");
            }

            string storedHash = PlayerPrefs.GetString(email);
            if (!PasswordHasher.Verify(password, storedHash))
            {
                return AuthResult.Fail("Please check your email and password.");
            }

            _currentAccount = account;
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
        {
            return AuthResult.Fail("Password confirmation does not match.");
        }

        try
        {
            var account = new Account(email, password);

            if (PlayerPrefs.HasKey(email))
            {
                return AuthResult.Fail("This email is already in use.");
            }

            string hashedPassword = PasswordHasher.Hash(password);
            PlayerPrefs.SetString(email, hashedPassword);
            PlayerPrefs.Save();

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
