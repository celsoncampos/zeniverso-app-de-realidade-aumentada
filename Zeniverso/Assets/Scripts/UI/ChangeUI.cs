using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUI : MonoBehaviour
{
    #region Singleton
    public static ChangeUI instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    public GameObject loginScreenPanel, registerScreenPanel;
    bool loginActive = true;

    public void ChangeBetweenLoginAndRegister()
    {
        loginActive = !loginActive;
        loginScreenPanel.SetActive(loginActive);
        registerScreenPanel.SetActive(!loginActive);
    }
}
