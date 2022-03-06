using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Individual : MonoBehaviour
{
    public void cargarMundo()
    {
        switch (gameObject.tag)
        {
            case "Mundo1":
                Globals.WorldNum = 1;
                break;
            case "Mundo2":
                Globals.WorldNum = 2;
                break;
            case "Mundo3":
                Globals.WorldNum = 3;
                break;
        }
        SceneManager.LoadScene("MenuMundos");
    }
}
