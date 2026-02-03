public class AccountData
{
    public string Email { get; set; }
    public string HashedPassword { get; set; }

    public AccountData(string email, string hashedPassword)
    {
        Email = email;
        HashedPassword = hashedPassword;
    }
}
