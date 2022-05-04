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
        } else if (escenaActual.name == "MenuLoginDividida")
        {
            SceneManager.LoadScene("MenuMulti");
        } else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }

    }
}
