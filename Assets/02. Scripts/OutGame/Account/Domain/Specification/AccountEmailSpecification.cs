using System.Text.RegularExpressions;

public class AccountEmailSpecification
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    private string _errorMessage = string.Empty;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _errorMessage = "Email cannot be empty.";
            return false;
        }

        if (!EmailRegex.IsMatch(email))
        {
            _errorMessage = "Invalid email format.";
            return false;
        }

        _errorMessage = string.Empty;
        return true;
    }
}
