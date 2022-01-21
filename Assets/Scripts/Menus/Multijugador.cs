using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Multijugador : MonoBehaviour
{
    public Button botonAtras;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        botonAtras = root.Q<Button>("botonatras");

        botonAtras.clicked += VolverAtras;
    }

    void VolverAtras()
    {
        SceneManager.LoadScene("Menu Principal");
    }
}
