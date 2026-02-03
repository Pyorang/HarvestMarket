public class AuthResult
{
    public bool Success { get; }
    public string ErrorMessage { get; }
    public Account Account { get; }

    public static AuthResult Ok(Account account) => new AuthResult(true, string.Empty, account);

    public static AuthResult Fail(string errorMessage) => new AuthResult(false, errorMessage, null);

    private AuthResult(bool success, string errorMessage, Account account)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Account = account;
    }
}
