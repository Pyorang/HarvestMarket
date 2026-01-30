using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    private enum PopupMode
    {
        Login,
        Register
    }

    [Header("Popup")]
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    [Header("Mode Toggle")]
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private PopupMode _currentMode = PopupMode.Login;

    private void Start()
    {
        InitializeButtons();
        _popupPanel.SetActive(false);
    }

    private void InitializeButtons()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
        _loginButton.onClick.AddListener(Login);
        _registerButton.onClick.AddListener(Register);
        _gotoRegisterButton.onClick.AddListener(() => SetMode(PopupMode.Register));
        _gotoLoginButton.onClick.AddListener(() => SetMode(PopupMode.Login));
    }

    public void Open()
    {
        _popupPanel.SetActive(true);
        SetMode(PopupMode.Login);
        ClearInputs();
    }

    public void Close()
    {
        _popupPanel.SetActive(false);
        ClearInputs();
    }

    private void SetMode(PopupMode mode)
    {
        _currentMode = mode;
        bool isLogin = mode == PopupMode.Login;

        _passwordConfirmObject.SetActive(!isLogin);
        _gotoRegisterButton.gameObject.SetActive(isLogin);
        _loginButton.gameObject.SetActive(isLogin);
        _gotoLoginButton.gameObject.SetActive(!isLogin);
        _registerButton.gameObject.SetActive(!isLogin);

        _messageText.text = string.Empty;
    }

    private void ClearInputs()
    {
        _idInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
        _passwordConfirmInputField.text = string.Empty;
        _messageText.text = string.Empty;
    }

    private void Login()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "Please enter your ID and password.";
            return;
        }

        if (!PlayerPrefs.HasKey(id))
        {
            _messageText.text = "Please check your ID and password.";
            return;
        }

        string storedHash = PlayerPrefs.GetString(id);
        if (!PasswordHasher.Verify(password, storedHash))
        {
            _messageText.text = "Please check your ID and password.";
            return;
        }

        Close();
        SceneManager.LoadScene("InGame");
    }

    private void Register()
    {
        string id = _idInputField.text;
        string password = _passwordInputField.text;
        string passwordConfirm = _passwordConfirmInputField.text;

        // Empty value check
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "Please enter your ID and password.";
            return;
        }

        // Email format
        if (!Regex.IsMatch(id, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            _messageText.text = "ID must be in email format.";
            return;
        }

        // Password validation
        if (!ValidatePassword(password, out string errorMessage))
        {
            _messageText.text = errorMessage;
            return;
        }

        // Password confirm
        if (string.IsNullOrEmpty(passwordConfirm) || password != passwordConfirm)
        {
            _messageText.text = "Password confirmation does not match.";
            return;
        }

        // Duplicate check
        if (PlayerPrefs.HasKey(id))
        {
            _messageText.text = "This ID is already in use.";
            return;
        }

        // Registration complete
        string hashedPassword = PasswordHasher.Hash(password);
        PlayerPrefs.SetString(id, hashedPassword);
        PlayerPrefs.Save();

        _messageText.text = "Registration completed successfully.";
        SetMode(PopupMode.Login);
        _idInputField.text = id;
    }

    private bool ValidatePassword(string password, out string errorMessage)
    {
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$"))
        {
            errorMessage = "Password can only contain letters, numbers, and special characters.";
            return false;
        }

        if (password.Length < 7 || password.Length > 20)
        {
            errorMessage = "Password must be between 7 and 20 characters.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            errorMessage = "Password must contain at least one uppercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            errorMessage = "Password must contain at least one lowercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
        {
            errorMessage = "Password must contain at least one special character.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
