using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public Transform ventana;
    public CanvasGroup velo;

    public void CargarModoIndiv()
    {
        Globals.Modo = "Indiv";
        SceneManager.LoadScene("MenuIndiv");
    }

    public void CargarModoMulti()
    {
        Globals.Modo = "Multi";
        SceneManager.LoadScene("MenuMulti");
    }

    public void CargarClasificaciones()
    {
        SceneManager.LoadScene("MenuClasificacion");
    }

    public void CargarMenuPersonalizaciones()
    {
        SceneManager.LoadScene("MenuPersonalizacion");
    }

    public void CargarMenuControles()
    {
        SceneManager.LoadScene("MenuControles");
    }

    public void CargarTienda()
    {
        SceneManager.LoadScene("MenuTienda");
    }

    public void AbrirPopup()
    {
        velo.gameObject.SetActive(true);
        velo.alpha = 0;
        velo.LeanAlpha(0.5f, 0.2f);

        ventana.localPosition = new Vector2(0, -Screen.height);
        ventana.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void CerrarPopup()
    {
        velo.LeanAlpha(0, 0.5f);
        ventana.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(completado);
    }

    void completado()
    {
        velo.gameObject.SetActive(false);
        ventana.gameObject.SetActive(false);
    }
}
