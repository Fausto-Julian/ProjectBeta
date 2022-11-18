using System;
using Firebase;
using Firebase.Auth;
using Photon.Pun;
using UnityEngine;

namespace _ProjectBeta.Scripts.Menu
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private LoginUIManager loginUIManager;

        private FirebaseAuth _auth;
        private FirebaseUser _user;

        private void Awake()
        {
            loginUIManager.Initialize(this);
            
            if (PhotonNetwork.IsConnected)
                loginUIManager.SwitchCanvas(true);
        }

        private void Start()
        {
            CheckAndFixDependenciesCoroutine();
        }

        private async void CheckAndFixDependenciesCoroutine()
        {
            var task = FirebaseApp.CheckAndFixDependenciesAsync();
            await task;

            if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError($"Could not resolve all firebase dependency: {task.Result}");
                return;
            }

            InitializeFireBase();
        }

        private void InitializeFireBase()
        {
            _auth = FirebaseAuth.DefaultInstance;

            _auth.StateChanged += AuthOnStateChanged;
            AuthOnStateChanged(this, null);
        }

        private void AuthOnStateChanged(object sender, EventArgs e)
        {
            if (_auth.CurrentUser == null || _auth.CurrentUser == _user)
                return;

            _user = _auth.CurrentUser;
            Debug.Log($"Signed In: {_user.DisplayName}");
        }

        public async void Login(string email, string password)
        {
            loginUIManager.SetActiveLoginButtons(false);
            loginUIManager.ClearOutput();

            var credential = EmailAuthProvider.GetCredential(email, password);

            var task = _auth.SignInWithCredentialAsync(credential);
            try
            {
                await task;
            }

            catch (Exception e)
            {
                loginUIManager.OnError(e.Message);
                loginUIManager.SetActiveLoginButtons(true);
                return;
            }


            if (CheckError(task.Exception))
            {
                loginUIManager.SetActiveLoginButtons(true);
                return;
            }

            if (_user.IsEmailVerified)
            {
                PhotonNetwork.NickName = _user.DisplayName;

                var props = PhotonNetwork.LocalPlayer.CustomProperties;
                props[GameSettings.ReadyId] = false;
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                PhotonNetwork.ConnectUsingSettings();
                loginUIManager.SwitchCanvas(true);
                return;
            }

            SendEmailVerification();
        }

        public async void Register(string userName, string email, string password, string confirmPassword)
        {
            loginUIManager.SetActiveRegisterButtons(false);
            loginUIManager.ClearOutput();

            if (email == string.Empty)
            {
                loginUIManager.SendUserError(UserError.EmailNotEnter);
                loginUIManager.SetActiveRegisterButtons(true);
                return;
            }

            if (password != confirmPassword)
            {
                loginUIManager.SendUserError(UserError.PasswordNotEnter);
                loginUIManager.SetActiveRegisterButtons(true);
                return;
            }

            var task = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await task;

            if (CheckError(task.Exception))
            {
                loginUIManager.SetActiveRegisterButtons(true);
                return;
            }

            var profile = new UserProfile()
            {
                DisplayName = userName
            };

            var userTask = _user.UpdateUserProfileAsync(profile);
            await userTask;

            if (userTask.Exception != null)
            {
                await _user.DeleteAsync();
                CheckError(userTask.Exception);
                loginUIManager.SetActiveRegisterButtons(true);
                return;
            }

            SendEmailVerification();
        }

        private async void SendEmailVerification()
        {
            if (_user == null)
                return;

            var task = _user.SendEmailVerificationAsync();

            await task;

            if (CheckError(task.Exception))
            {
                loginUIManager.SetActiveLoginButtons(true);
                return;
            }

            loginUIManager.ActiveVerificationEmail();
        }

        private bool CheckError(Exception exception)
        {
            if (exception == null)
                return false;
            var firebaseException = (FirebaseException)exception.GetBaseException();
            var error = (AuthError)firebaseException.ErrorCode;

            loginUIManager.SendAuthError(error);

            return true;
        }
    }
}