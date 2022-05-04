using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMulti : MonoBehaviour
{
    private void Start()
    {
        Globals.WorldNum = 4;
        Globals.LevelNum = 5;
    }

    public void cargarMundo()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void cargarDividida()
    {
        Globals.Modo = "Pantalladiv";
        SceneManager.LoadScene("MenuLoginDividida");
    }
}
