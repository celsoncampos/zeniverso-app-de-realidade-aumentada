using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ToggleShowAndHidePassword : MonoBehaviour
{
    bool hideLoginPass = true;
    bool hideRegistrationPass = true;
    public TMP_InputField loginpasswordInputField;
    public TMP_InputField registerPasswordField, registerVerifyPasswordField;

    public void ShowOrHideLoginPassword()
    {
        hideLoginPass = !hideLoginPass;
        if(hideLoginPass)
        {
            loginpasswordInputField.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            loginpasswordInputField.contentType = TMP_InputField.ContentType.Standard;
        }
        EventSystem.current.SetSelectedGameObject(loginpasswordInputField.gameObject, null);
    }

    public void ShowOrHideRegistrationPassword()
    {
        hideRegistrationPass = !hideRegistrationPass;
        if(hideRegistrationPass)
        {
            registerPasswordField.contentType = TMP_InputField.ContentType.Password;
            registerVerifyPasswordField.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            registerPasswordField.contentType = TMP_InputField.ContentType.Standard;
            registerVerifyPasswordField.contentType = TMP_InputField.ContentType.Standard;
        }
        EventSystem.current.SetSelectedGameObject(registerPasswordField.gameObject, null);
        EventSystem.current.SetSelectedGameObject(registerVerifyPasswordField.gameObject, null);
    }
}
