using System;
using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    private readonly AccountEmailSpecification _emailSpec = new AccountEmailSpecification();

    public AccountData FindByEmail(string email)
    {
        if (!PlayerPrefs.HasKey(email)) return null;

        string storedHash = PlayerPrefs.GetString(email);
        return new AccountData(email, storedHash);
    }

    public void Save(AccountData data)
    {
        if (!_emailSpec.IsSatisfiedBy(data.Email))
            throw new ArgumentException(_emailSpec.ErrorMessage);

        if (string.IsNullOrEmpty(data.HashedPassword))
            throw new ArgumentException("Hashed password cannot be empty.");

        PlayerPrefs.SetString(data.Email, data.HashedPassword);
        PlayerPrefs.Save();
    }

    public void Delete(string email)
    {
        PlayerPrefs.DeleteKey(email);
        PlayerPrefs.Save();
    }

    public bool Exists(string email)
    {
        return PlayerPrefs.HasKey(email);
    }
}
