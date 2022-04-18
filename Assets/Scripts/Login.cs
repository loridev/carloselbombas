using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public Text status;
    public GameObject nameInput;
    public GameObject passwordInput;

    private void Start()
    {
        status.text = "Introduce tu nombre de usuario y tu contraseña";
    }

    public async void LogUser()
    {
        status.text = "Cargando...";
        Globals.CurrentUser = await ApiRequests.Login(nameInput.GetComponent<InputField>().text, passwordInput.GetComponent<InputField>().text);

        if (Globals.CurrentUser != null)
        {
            SceneManager.LoadScene("MenuPrincipal");
        } else
        {
            status.text = "Nombre o contraseña incorrectos";
        }


    }
}
