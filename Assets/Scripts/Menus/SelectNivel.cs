using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectNivel : MonoBehaviour
{
    public void CargarNivel()
    {
        switch (gameObject.tag)
        {
            case "Nivel1":
                Globals.LevelNum = 1;
                break;
            case "Nivel2":
                Globals.LevelNum = 2;
                break;
            case "Nivel3":
                Globals.LevelNum = 3;
                break;
            case "Nivel4":
                Globals.LevelNum = 4;
                break;
        }

        SceneManager.LoadScene(Globals.CurrentUser.indiv_level == "1-1" ? "MenuTutorial" : "MapaDinamicoFinal");
    }

    public void CargarNivelContrarreloj()
    {
        Globals.Modo = "Contrarreloj";
        CargarNivel();
    }
}