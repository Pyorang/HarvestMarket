using System.Text.RegularExpressions;

public class AccountPasswordSpecification
{
    private static readonly Regex AllowedCharsRegex = new Regex(
        @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$",
        RegexOptions.Compiled
    );

    private string _errorMessage = string.Empty;
    public string ErrorMessage => _errorMessage;

    public bool IsSatisfiedBy(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _errorMessage = "Password cannot be empty.";
            return false;
        }

        if (!AllowedCharsRegex.IsMatch(password))
        {
            _errorMessage = "Password can only contain letters, numbers, and special characters.";
            return false;
        }

        if (password.Length < 7 || password.Length > 20)
        {
            _errorMessage = "Password must be between 7 and 20 characters.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            _errorMessage = "Password must contain at least one uppercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            _errorMessage = "Password must contain at least one lowercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
        {
            _errorMessage = "Password must contain at least one special character.";
            return false;
        }

        _errorMessage = string.Empty;
        return true;
    }
}
