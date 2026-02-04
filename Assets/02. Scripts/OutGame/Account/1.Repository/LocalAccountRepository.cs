using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LocalAccountRepository : IAccountRepository
{
    public UniTask<AccountResult> Register(string email, string password)
    {
        try
        {
            if (PlayerPrefs.HasKey(email))
            {
                return UniTask.FromResult(new AccountResult
                {
                    Success = false,
                    ErrorMessage = "이미 사용 중인 이메일입니다."
                });
            }

            string hashedPassword = PasswordHasher.Hash(password);
            PlayerPrefs.SetString(email, hashedPassword);
            PlayerPrefs.Save();

            return UniTask.FromResult(new AccountResult
            {
                Success = true
            });
        }
        catch (Exception e)
        {
            return UniTask.FromResult(new AccountResult
            {
                Success = false,
                ErrorMessage = e.Message
            });
        }
    }

    public UniTask<AccountResult> Login(string email, string password)
    {
        try
        {
            if (!PlayerPrefs.HasKey(email))
            {
                return UniTask.FromResult(new AccountResult
                {
                    Success = false,
                    ErrorMessage = "이메일 또는 비밀번호를 확인해주세요."
                });
            }

            string storedHash = PlayerPrefs.GetString(email);
            if (!PasswordHasher.Verify(password, storedHash))
            {
                return UniTask.FromResult(new AccountResult
                {
                    Success = false,
                    ErrorMessage = "이메일 또는 비밀번호를 확인해주세요."
                });
            }

            var account = new Account(email, password);
            return UniTask.FromResult(new AccountResult
            {
                Success = true,
                Account = account
            });
        }
        catch (Exception e)
        {
            return UniTask.FromResult(new AccountResult
            {
                Success = false,
                ErrorMessage = e.Message
            });
        }
    }

    public void Logout()
    {
        
    }
}
