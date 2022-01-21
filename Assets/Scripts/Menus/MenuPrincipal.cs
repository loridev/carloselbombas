using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public Button botonIndiv;
    public Button botonMult;
    public Button botonPerfil;
    public Button botonClasif;
    public Button botonSkin;
    public Button botonControles;
    public Button botonAjustes;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        botonIndiv = root.Q<Button>("Individual");
        botonMult = root.Q<Button>("Multijugador");
        botonPerfil = root.Q<Button>("UserButton");
        botonClasif = root.Q<Button>("clasif");
        botonSkin = root.Q<Button>("skin");
        botonControles = root.Q<Button>("controles");
        botonAjustes = root.Q<Button>("ajustes");

        botonIndiv.clicked += RedirModoIndiv;
        botonMult.clicked += RedirModoMult;

    }

    void RedirModoIndiv()
    {
        SceneManager.LoadScene("Menu Individual");
    }

    void RedirModoMult()
    {
        SceneManager.LoadScene("Multijugador");
    }
}
