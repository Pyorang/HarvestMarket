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
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private PopupMode _currentMode = PopupMode.Login;

    private readonly AccountEmailSpecification _emailSpec = new AccountEmailSpecification();
    private readonly AccountPasswordSpecification _passwordSpec = new AccountPasswordSpecification();

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
        _emailInputField.text = string.Empty;
        _passwordInputField.text = string.Empty;
        _passwordConfirmInputField.text = string.Empty;
        _messageText.text = string.Empty;
    }

    private async void Login()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;

        AccountResult result = await AccountManager.Instance.TryLogin(email, password);

        if (result.Success)
        {
            Close();
            SceneManager.LoadScene("InGame");
        }
        else
        {
            _messageText.text = result.ErrorMessage;
        }
    }

    private async void Register()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;
        string passwordConfirm = _passwordConfirmInputField.text;

        AccountResult result = await AccountManager.Instance.TryRegister(email, password, passwordConfirm);

        if (result.Success)
        {
            _messageText.text = "Registration completed successfully.";
            SetMode(PopupMode.Login);
            _emailInputField.text = email;
        }
        else
        {
            _messageText.text = result.ErrorMessage;
        }
    }

    public void OnEmailTextChange(string email)
    {
        if (_emailSpec.IsSatisfiedBy(email))
        {
            _messageText.text = "Valid email format!";
        }
        else
        {
            _messageText.text = _emailSpec.ErrorMessage;
        }
        UpdateButtonState();
    }

    public void OnPasswordTextChange(string password)
    {
        if (_passwordSpec.IsSatisfiedBy(password))
        {
            _messageText.text = "Valid password format!";
        }
        else
        {
            _messageText.text = _passwordSpec.ErrorMessage;
        }
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;

        bool isValid = _emailSpec.IsSatisfiedBy(email) && _passwordSpec.IsSatisfiedBy(password);

        _loginButton.interactable = isValid;
        _registerButton.interactable = isValid;
    }
}
