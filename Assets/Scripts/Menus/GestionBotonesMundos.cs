using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionBotonesMundos : MonoBehaviour
{
    public Button[] botonesMundos;

    private void Start()
    {
        GestionBotones();
    }

    private void GestionBotones()
    {
        for (int i = 0; i < botonesMundos.Length; i++)
        {
            if (!(botonesMundos[i].enabled = Globals.CurrentUser.GetWorldNum() > i))
            {
                botonesMundos[i].GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
