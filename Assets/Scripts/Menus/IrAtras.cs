using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IrAtras : MonoBehaviour
{
    public void VolverAtras()
    {
        Scene escenaActual = SceneManager.GetActiveScene();

        if (escenaActual.name == "MenuMundos")
        {
            SceneManager.LoadScene("MenuIndiv");
        } else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

    }
}
