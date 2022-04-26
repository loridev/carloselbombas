using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionBotonesNiveles : MonoBehaviour
{
    public Button[] botonesNiveles;
    public Button[] botonesContrarreloj;

    private void Start()
    {
        Globals.Modo = "Indiv";
        GestionBotones();
    }

    private void GestionBotones()
    {
        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            if (!(botonesNiveles[i].enabled = Globals.CurrentUser.GetLevelNum() > i || Globals.CurrentUser.GetWorldNum() > Globals.WorldNum))
            {
                botonesNiveles[i].GetComponent<Image>().color = Color.grey;
            }
            if (!(botonesContrarreloj[i].enabled = Globals.CurrentUser.GetLevelNum() > i + 1 || Globals.CurrentUser.GetWorldNum() > Globals.WorldNum))
            {
                botonesContrarreloj[i].GetComponent<Image>().color = Color.grey;
            }
        }
    }
}
