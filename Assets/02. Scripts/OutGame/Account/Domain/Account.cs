public class Account
{
    public readonly string Email;
    public readonly string Password;

    public Account(string email, string password)
    {
        // Email validation
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            throw new System.ArgumentException(emailSpec.ErrorMessage);
        }

        // Password validation
        var passwordSpec = new AccountPasswordSpecification();
        if (!passwordSpec.IsSatisfiedBy(password))
        {
            throw new System.ArgumentException(passwordSpec.ErrorMessage);
        }

        Email = email;
        Password = password;
    }
}
