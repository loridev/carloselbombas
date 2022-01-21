using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Mundos : MonoBehaviour
{
    public VisualElement root;
    public Button botonAtras;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        botonAtras = root.Q<Button>("botonatras");

        botonAtras.clicked += VolverAtras;
    }

    void VolverAtras()
    {
        SceneManager.LoadScene("Menu Individual");
    }
}
