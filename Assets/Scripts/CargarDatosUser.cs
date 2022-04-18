using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargarDatosUser : MonoBehaviour
{
    public Text labelUsername;
    public Text labelEmail;

    // Start is called before the first frame update
    void Start()
    {
        labelUsername.text += Globals.CurrentUser.name;
        labelEmail.text += Globals.CurrentUser.email;
    }

    public async void Logout()
    {
        if (await ApiRequests.Logout(Globals.Token)) {
            SceneManager.LoadScene("MenuLogin");
        }
    }
}
