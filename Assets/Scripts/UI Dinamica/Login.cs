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
    private Scene escenaActual;

    private void Start()
    {
        escenaActual = SceneManager.GetActiveScene();
        status.text = escenaActual.name == "MenuLogin" ? "Introduce tu nombre de usuario y tu contraseña" 
            : "Introduce el nombre y contraseña del jugador 2";
        if (Globals.City == null && escenaActual.name != "MenuLoginDividida")
        {
            StartCoroutine(LocationUtils.GetUserCity());
        }
    }

    public async void LogUser()
    {
        status.text = "Cargando...";
        if (escenaActual.name == "MenuLogin")
        {
            Globals.CurrentUser = await ApiRequests.Login(
                nameInput.GetComponent<InputField>().text, 
                passwordInput.GetComponent<InputField>().text,
                false);
        }
        else
        {
            Globals.Player2 = await ApiRequests.Login(
                nameInput.GetComponent<InputField>().text, 
                passwordInput.GetComponent<InputField>().text,
                true);
        }


        if ((escenaActual.name == "MenuLogin" ? Globals.CurrentUser : Globals.Player2) != null)
        {
            if (await ApiRequests.GetEquipped(escenaActual.name == "MenuLogin" ? Globals.Token : Globals.Token2))
            {
                if (escenaActual.name == "MenuLogin")
                {
                    BGSoundScript.BackMusicPlay();
                    Debug.Log("entra en la escena");
                }

                SceneManager.LoadScene(escenaActual.name == "MenuLogin" ? "MenuPrincipal" : "MapaDinamicoFinal");

            }
            else
            {
                status.text = "Error obteniendo el inventario, vuelve a intentar más tarde";
            }

        } else
        {
            status.text = "Nombre o contraseña incorrectos";
        }
    }
}
