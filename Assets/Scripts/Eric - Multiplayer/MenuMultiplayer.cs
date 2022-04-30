using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMultiplayer : MonoBehaviour
{
    public void cargarMenuSala()
    {
        SceneManager.LoadScene("MenuSalaMulti");
    }

    public void cargarDividida()
    {
        Globals.Modo = "Pantalladiv";
        SceneManager.LoadScene("MapaDinamicoFinal");
    }
}
