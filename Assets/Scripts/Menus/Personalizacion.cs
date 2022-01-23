using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personalizacion : MonoBehaviour
{
    public GameObject contenedor;
    public GameObject[] tabs;
    public GameObject[] contenedores;
    private Color32 colorActivo = new Color32(46, 46, 46, 255);
    private Color32 colorNoActivo = new Color32(96, 96, 96, 255);
    private Color32 colorIconoActivo = new Color32(255, 255, 255, 255);
    private Color32 colorIconoNoActivo = new Color32(58, 58, 58, 255);

    public void MostrarContenedor()
    {

        contenedor.SetActive(true);

        foreach (GameObject tab in tabs)
        {
            tab.transform.parent.gameObject.GetComponent<Image>().color = colorNoActivo;
            tab.GetComponent<Image>().color = colorIconoNoActivo;
        }

        foreach (GameObject contenedorI in contenedores)
        {
            contenedorI.SetActive(false);
        }
        gameObject.GetComponent<Image>().color = colorIconoActivo;
        gameObject.transform.parent.gameObject.GetComponent<Image>().color = colorActivo;
        contenedor.SetActive(true);
    }
}
