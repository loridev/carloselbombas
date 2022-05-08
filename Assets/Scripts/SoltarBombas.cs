using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : Photon.MonoBehaviour
{
    public GameObject projectilePrefab;
    private ComportamientoCarlos carlosAtributos;

    // Start is called before the first frame update
    void Start()
    {
        if (!CompareTag("Untagged"))
        {
            carlosAtributos = GetComponent<ComportamientoCarlos>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CompareTag("Untagged"))
        {
            if (Input.GetKeyDown(CompareTag("Player") ? KeyCode.Space : KeyCode.Return))
            {
                Celda celdaCerca = null;

                celdaCerca = carlosAtributos.EncontrarCeldaMasCerca(transform.position);

                if (!celdaCerca.ocupado && !carlosAtributos.cargando
                                        && carlosAtributos.bombasEnMapa < carlosAtributos.limiteBombas)
                {
                    ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                    Transform bomba;
                
                    bomba = Instantiate(projectilePrefab, new Vector3(celdaCerca.posicionCelda.x, 0.25f, celdaCerca.posicionCelda.z),
                        projectilePrefab.transform.rotation).transform;
                    celdaCerca.ocupado = true;
                    celdaCerca.objTipoCelda = bomba;
                    foreach (Transform tr in bomba.GetComponentsInChildren<Transform>())
                    {
                        if (tr.CompareTag("BombaSkin"))
                        {
                            tr.GetComponent<MeshRenderer>().material = carlosAtributos.skinBomba;
                        }
                    }
                }
            }
        }
    }
}

