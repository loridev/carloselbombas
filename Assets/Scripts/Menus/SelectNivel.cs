using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectNivel : MonoBehaviour
{
    public void cargarNivel()
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
                Globals.LevelNum = 3;
                break;
        }

        SceneManager.LoadScene("MapaDinamicoFinal");
    }
}
