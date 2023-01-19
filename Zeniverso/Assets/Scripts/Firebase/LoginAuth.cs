using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;

public class LoginAuth : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    public void LoginButton()
    {
        StartCoroutine(StartLogin(emailInputField.text, passwordInputField.text));
    }

    private IEnumerator StartLogin(string email, string password)
    {
        var LoginTask = FirebaseAuthenticator.instance.auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if(LoginTask.Exception != null)
        {
            HandleLoginErrors(LoginTask.Exception);
        }
        else
        {
            LoginUser(LoginTask);
        }
    }

    void HandleLoginErrors(System.AggregateException loginException)
    {
        Debug.LogWarning(message: $"Falha ao fazer login na tarefa com {loginException}");
        FirebaseException firebaseEx = loginException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        warningLoginText.text = DefineLoginErrorMessage(errorCode);
    }

    string DefineLoginErrorMessage(AuthError errorCode)
    {
        switch(errorCode)
        {
            case AuthError.MissingEmail:
                return "Email ausente";
            case AuthError.MissingPassword:
                return "Senha ausente";
            case AuthError.InvalidEmail:
                return "Email invalido";
            case AuthError.UserNotFound:
                return "Conta não existente";
            default:
                return "Email ou senha erradas";
        }
    }

    void LoginUser(System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> loginTask)
    {
        FirebaseAuthenticator.instance.User = loginTask.Result;
        var cena = "Home";
        ChangeScene.instance.LoadScenes(cena);
        Debug.LogFormat("Login feito com sucesso: {0} ({1})", FirebaseAuthenticator.instance.User.DisplayName, FirebaseAuthenticator.instance.User.Email);

        /*
        warningLoginText.text = "";
        var cena = "Home";
        ChangeScene.instance.LoadScenes(cena);
        confirmLoginText.text = "Logado";
        */
    }
}
