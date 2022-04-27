using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarDatosUser : MonoBehaviour
{
    public Text labelUsername;
    public Text labelEmail;
    public Text labelSaludo;

    // Start is called before the first frame update
    void Start()
    {
        labelUsername.text += Globals.CurrentUser.name;
        labelEmail.text += Globals.CurrentUser.email;
        if (Globals.City != null)
        {
            labelSaludo.text = "Hola " + Globals.CurrentUser.name + " desde " + Globals.City + "!";
        }
        else
        {
            labelSaludo.text = "Hola " + Globals.CurrentUser.name + "!";
        }
    }

    public async void Logout()
    {
        if (await ApiRequests.Logout(Globals.Token)) {
            SceneManager.LoadScene("MenuLogin");
        }
    }
}
