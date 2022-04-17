using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text status;
    public Text nameInput;
    public Text passwordInput;

    public void LogUser()
    {
        Debug.Log(nameInput.text);
        Debug.Log(passwordInput.text);


    }
}
