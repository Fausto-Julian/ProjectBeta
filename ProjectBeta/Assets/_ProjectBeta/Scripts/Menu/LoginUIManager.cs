using System;
using System.Collections;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _ProjectBeta.Scripts.Menu
{
    public enum UserError
    {
        EmailNotEnter,
        PasswordNotEnter
    }

    public class LoginUIManager : MonoBehaviour
    {
        [Header("Canvas Group")] 
        [SerializeField] private CanvasGroup loginCanvas;
        [SerializeField] private CanvasGroup menuCanvas;
        [SerializeField] private float fadeTime;

        [Header("Configuration")] [SerializeField]
        private float errorShowTime;

        [Header("Login")] [SerializeField] private GameObject loginPanel;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button goToRegisterPanelButton;
        [SerializeField] private TMP_InputField loginEmail;
        [SerializeField] private TMP_InputField loginPassword;

        [Header("Register")] [SerializeField] private GameObject registerPanel;
        [SerializeField] private Button registerButton;
        [SerializeField] private Button backToLoginPanelButton;
        [SerializeField] private TMP_InputField registerUserName;
        [SerializeField] private TMP_InputField registerEmail;
        [SerializeField] private TMP_InputField registerPassword;
        [SerializeField] private TMP_InputField registerConfirmPassword;

        [Header("Verification")] [SerializeField]
        private GameObject emailVerificationPanel;

        [SerializeField] private Button backToLoginPanelButton2;

        [Space(2f)] [SerializeField] private TMP_Text errorOutput;

        private Coroutine _errorCoroutine;
        private WaitForSeconds _errorWaitTime;
        private LoginManager _manager;

        private void Awake()
        {
            _errorWaitTime = new WaitForSeconds(errorShowTime);
        }
        private void Start()
        {
            menuCanvas.interactable = false;
            menuCanvas.gameObject.SetActive(false);
           
        }

        public void Initialize(LoginManager manager)
        {
            _manager = manager;
            
            loginButton.onClick.AddListener(OnClickedLoginButtonHandler);
            goToRegisterPanelButton.onClick.AddListener(OnClickedActiveRegisterButtonHandler);

            registerButton.onClick.AddListener(OnClickedRegisterButtonHandler);
            backToLoginPanelButton.onClick.AddListener(OnClickedActiveLoginButtonHandler);

            backToLoginPanelButton2.onClick.AddListener(OnClickedActiveLoginButtonHandler);

            OnClickedActiveLoginButtonHandler();
        }

        public void SwitchCanvas(bool menu)
        {
            if (menu)
            {
                StartCoroutine(SwitchCanvas(loginCanvas, menuCanvas));
                return;
            }

            StartCoroutine(SwitchCanvas(menuCanvas, loginCanvas));
        }

        private IEnumerator SwitchCanvas(CanvasGroup hide, CanvasGroup show)
        {
            var counter = 0f;

            while (counter < fadeTime)
            {
                counter += Time.deltaTime;
                hide.alpha = Mathf.Lerp(1, 0, counter / fadeTime);

                yield return null;
            }

            if (hide.alpha <= 0.1)
            {
                hide.gameObject.SetActive(false);
                hide.interactable = false;
                hide.alpha = 0;
            }
            
            show.alpha = 0;
            show.gameObject.SetActive(true);
            show.interactable = true;
            
            counter = 0f;
            while (counter < fadeTime)
            {
                counter += Time.deltaTime;
                show.alpha = Mathf.Lerp(0, 1, counter / fadeTime);

                yield return null;
            }
            show.alpha = 1;
        }

        private void Clear()
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            emailVerificationPanel.SetActive(false);

            SetActiveLoginButtons(false);
            SetActiveRegisterButtons(false);
            backToLoginPanelButton2.interactable = false;

            ClearOutput();
        }

        private void OnClickedActiveLoginButtonHandler()
        {
            Clear();
            loginPanel.SetActive(true);

            SetActiveLoginButtons(true);
        }

        private void OnClickedActiveRegisterButtonHandler()
        {
            Clear();
            registerPanel.SetActive(true);
            SetActiveRegisterButtons(true);
        }

        private static bool CheckIsValidEmail(string proposedEmail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(proposedEmail);
                return addr.Address == proposedEmail;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void OnClickedLoginButtonHandler()
        {
            var proposedEmail = loginEmail.text;

            if (!CheckIsValidEmail(proposedEmail))
            {
                OnError("Email address is not valid");
                return;
            }

            _manager.Login(proposedEmail, loginPassword.text);
        }

        private void OnClickedRegisterButtonHandler()
        {
            _manager.Register(registerUserName.text, registerEmail.text, registerPassword.text,
                registerConfirmPassword.text);
        }

        public void ActiveVerificationEmail()
        {
            Clear();
            emailVerificationPanel.SetActive(true);
            backToLoginPanelButton2.interactable = true;
        }

        public void SetActiveLoginButtons(bool value)
        {
            loginButton.interactable = value;
            goToRegisterPanelButton.interactable = value;
        }

        public void SetActiveRegisterButtons(bool value)
        {
            registerButton.interactable = value;
            backToLoginPanelButton.interactable = value;
        }

        public void SendUserError(UserError error)
        {
            switch (error)
            {
                case UserError.EmailNotEnter:
                    OnError("Please Enter a Email");
                    return;
                case UserError.PasswordNotEnter:
                    OnError("Passwords Do Not Match");
                    return;
                default:
                    OnError("Unknown Error, Please  Try Again");
                    return;
            }
        }

        public void OnError(string errorMessage)
        {
            if (_errorCoroutine != default)
            {
                FinishPendingCoroutine();
            }

            _errorCoroutine = StartCoroutine(OnErrorDisplay(errorMessage));
        }

        private void FinishPendingCoroutine()
        {
            StopCoroutine(_errorCoroutine);
            ClearOutput();
        }

        private IEnumerator OnErrorDisplay(string errorMessage)
        {
            errorOutput.text = errorMessage;
            yield return _errorWaitTime;
            ClearOutput();
        }

        public void SendAuthError(AuthError error)
        {
            switch (error)
            {
                case AuthError.MissingEmail:
                    OnError("Please Enter Your Email");
                    return;
                case AuthError.MissingPassword:
                    OnError("Please Enter your Password");
                    return;
                case AuthError.InvalidEmail:
                    OnError("Invalid Email");
                    return;
                case AuthError.WrongPassword:
                    OnError("Incorrect Password");
                    return;
                case AuthError.UserNotFound:
                    OnError("User Not Found");
                    return;
                case AuthError.EmailAlreadyInUse:
                    OnError("Email Already In Use");
                    return;
                case AuthError.WeakPassword:
                    OnError("Weak Password");
                    return;
                case AuthError.Cancelled:
                    OnError("Task Was Cancelled");
                    return;
                case AuthError.SessionExpired:
                    OnError("Session Expired");
                    return;
                case AuthError.InvalidRecipientEmail:
                    OnError("Invalid Email");
                    return;
                case AuthError.TooManyRequests:
                    OnError("Too Many Request");
                    return;
                case AuthError.None:
                case AuthError.Unimplemented:
                case AuthError.Failure:
                case AuthError.InvalidCustomToken:
                case AuthError.CustomTokenMismatch:
                case AuthError.InvalidCredential:
                case AuthError.UserDisabled:
                case AuthError.AccountExistsWithDifferentCredentials:
                case AuthError.OperationNotAllowed:
                case AuthError.RequiresRecentLogin:
                case AuthError.CredentialAlreadyInUse:
                case AuthError.ProviderAlreadyLinked:
                case AuthError.NoSuchProvider:
                case AuthError.InvalidUserToken:
                case AuthError.UserTokenExpired:
                case AuthError.NetworkRequestFailed:
                case AuthError.InvalidApiKey:
                case AuthError.AppNotAuthorized:
                case AuthError.UserMismatch:
                case AuthError.NoSignedInUser:
                case AuthError.ApiNotAvailable:
                case AuthError.ExpiredActionCode:
                case AuthError.InvalidActionCode:
                case AuthError.InvalidMessagePayload:
                case AuthError.InvalidPhoneNumber:
                case AuthError.MissingPhoneNumber:
                case AuthError.InvalidSender:
                case AuthError.InvalidVerificationCode:
                case AuthError.InvalidVerificationId:
                case AuthError.MissingVerificationCode:
                case AuthError.MissingVerificationId:
                case AuthError.QuotaExceeded:
                case AuthError.RetryPhoneAuth:
                case AuthError.AppNotVerified:
                case AuthError.AppVerificationFailed:
                case AuthError.CaptchaCheckFailed:
                case AuthError.InvalidAppCredential:
                case AuthError.MissingAppCredential:
                case AuthError.InvalidClientId:
                case AuthError.InvalidContinueUri:
                case AuthError.MissingContinueUri:
                case AuthError.KeychainError:
                case AuthError.MissingAppToken:
                case AuthError.MissingIosBundleId:
                case AuthError.NotificationNotForwarded:
                case AuthError.UnauthorizedDomain:
                case AuthError.WebContextAlreadyPresented:
                case AuthError.WebContextCancelled:
                case AuthError.DynamicLinkNotActivated:
                case AuthError.InvalidProviderId:
                case AuthError.WebInternalError:
                case AuthError.WebStorateUnsupported:
                case AuthError.TenantIdMismatch:
                case AuthError.UnsupportedTenantOperation:
                case AuthError.InvalidLinkDomain:
                case AuthError.RejectedCredential:
                case AuthError.PhoneNumberNotFound:
                case AuthError.InvalidTenantId:
                case AuthError.MissingClientIdentifier:
                case AuthError.MissingMultiFactorSession:
                case AuthError.MissingMultiFactorInfo:
                case AuthError.InvalidMultiFactorSession:
                case AuthError.MultiFactorInfoNotFound:
                case AuthError.AdminRestrictedOperation:
                case AuthError.UnverifiedEmail:
                case AuthError.SecondFactorAlreadyEnrolled:
                case AuthError.MaximumSecondFactorCountExceeded:
                case AuthError.UnsupportedFirstFactor:
                case AuthError.EmailChangeNeedsVerification:
                default:
                    OnError("Unknown Error, Please  Try Again");
                    return;
            }
        }

        public void ClearOutput()
        {
            errorOutput.text = string.Empty;
        }
    }
}