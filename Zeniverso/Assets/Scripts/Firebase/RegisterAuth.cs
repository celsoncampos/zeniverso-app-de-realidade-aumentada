using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class RegisterAuth : MonoBehaviour
{
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField verifypasswordRegisterField;
    public TMP_Text warningRegisterText;

    public void RegisterButton()
    {
        StartCoroutine(StartRegister(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator StartRegister(string email, string password, string userName)
    {
        if (!CheckRegistrationFieldsAndReturnForErrors())
        {
            var RegisterTask = FirebaseAuthenticator.instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if(RegisterTask.Exception != null)
            {
                HandleRegisterErrors(RegisterTask.Exception);
            }
            else
            {
                StartCoroutine(RegisterUser(RegisterTask, userName, email, password));
            }
        }
    }

    bool CheckRegistrationFieldsAndReturnForErrors()
    {
        if(usernameRegisterField.text == "")
        {
            warningRegisterText.text = "Preencha todos os campos";
            return true;
        }
        else if(passwordRegisterField.text != verifypasswordRegisterField.text)
        {
            warningRegisterText.text = "Senha e verificar senha diferentes";
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleRegisterErrors(System.AggregateException registerException)
    {
        Debug.LogWarning(message: $"Falha na tarefa de registro com {registerException}");
        FirebaseException firebaseEx = registerException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        warningRegisterText.text = DefineRegisterErrorMessage(errorCode);
    }

    string DefineRegisterErrorMessage(AuthError errorCode)
    {
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "Email ausente";
            case AuthError.MissingPassword:
                return "Senha ausente";
            case AuthError.WeakPassword:
                return "Senha fraca";
            case AuthError.InvalidEmail:
                return "Email invalido";
            case AuthError.EmailAlreadyInUse:
                return "Este email já tem uma conta";
            default:
                return "Lamentamos, não é possível fazer o registro de usuário";
        }
    }

    private IEnumerator RegisterUser(System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> registerTask, string displayName, string email, string password)
    {
        FirebaseAuthenticator.instance.User = registerTask.Result;

        if(FirebaseAuthenticator.instance.User != null)
        {
            UserProfile profile = new UserProfile { DisplayName = displayName };
            var ProfileTask = FirebaseAuthenticator.instance.User.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

            if(ProfileTask.Exception != null)
            {
                HandleProfileCreationError(ProfileTask.Exception);
            }
            else
            {
                ChangeUI.instance.ChangeBetweenLoginAndRegister();
                GetComponent<LoginAuth>().emailInputField.text = email;
                GetComponent<LoginAuth>().passwordInputField.text = password;
                GetComponent<LoginAuth>().LoginButton();
            }
        }
    }

    void HandleProfileCreationError(System.AggregateException profileException)
    {
        Debug.LogWarning(message: $"Falhou o registro da tarefa com {profileException}");
        FirebaseException firebaseEx = profileException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
        warningRegisterText.text = "Falha ao definir nome de usuário";
    }

}
