using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoPowerups : MonoBehaviour
{

    public Transform[] indicadores;

    void Start()
    {
        Transform indicador;
        if (gameObject.tag.StartsWith("PU"))
        {
            indicador = Instantiate(indicadores[0], transform.position, Quaternion.identity);
        }
        else
        {
            indicador = Instantiate(indicadores[1], transform.position, Quaternion.identity);
        }
        indicador.parent = transform;
        Destroy(gameObject, 10);
    }
}
