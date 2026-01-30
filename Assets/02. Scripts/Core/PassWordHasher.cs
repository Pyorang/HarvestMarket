using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 비밀번호 해시 유틸리티 클래스
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// 비밀번호를 SHA-256으로 해시합니다.
    /// </summary>
    /// <param name="password">평문 비밀번호</param>
    /// <returns>Base64로 인코딩된 해시 문자열</returns>
    public static string Hash(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    /// <summary>
    /// 입력된 비밀번호가 저장된 해시와 일치하는지 확인합니다.
    /// </summary>
    /// <param name="password">평문 비밀번호</param>
    /// <param name="storedHash">저장된 해시 값</param>
    /// <returns>일치 여부</returns>
    public static bool Verify(string password, string storedHash)
    {
        string inputHash = Hash(password);
        return inputHash == storedHash;
    }
}
