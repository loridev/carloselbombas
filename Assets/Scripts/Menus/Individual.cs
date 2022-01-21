using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Individual : MonoBehaviour
{
    public Button botonAtras;
    public Button botonMundo1;
    public Button botonMundo2;
    public Button botonMundo3;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        botonAtras = root.Q<Button>("botonatras");
        botonMundo1 = root.Q<Button>("mundo1");
        botonMundo2 = root.Q<Button>("mundo2");
        botonMundo3 = root.Q<Button>("mundo3");
        Button[] botonesMundos = new Button[] { botonMundo1, botonMundo2, botonMundo3 };

        for (int i = 0; i < botonesMundos.Length; i++)
        {
            botonesMundos[i].clicked += CargarMundo;
        }

        botonAtras.clicked += VolverAtras;
    }

    void VolverAtras()
    {
        SceneManager.LoadScene("Menu Principal");
    }

    void CargarMundo()
    {
        // Mundos mundosScript = GameObject.Find("UIDocumentMundos").GetComponent<Mundos>();

        // Label tituloMundo = mundosScript.root.Q<Label>("titletext");

        // tituloMundo.text = "Mundo 1";

        SceneManager.LoadScene("Mundos");
    }
}
