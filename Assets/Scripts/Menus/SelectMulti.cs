using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMulti : MonoBehaviour
{
    public void cargarMundo()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void cargarDividida()
    {
        Globals.Modo = "Pantalladiv";
        SceneManager.LoadScene("MapaDinamicoFinal");
    }
}
