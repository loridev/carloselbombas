using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : Photon.MonoBehaviour
{
    private GeneracionMapa mapaCosas;
    public GameObject projectilePrefab;
    private ComportamientoCarlos carlosAtributos;

    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        if (!CompareTag("Untagged"))
        {
            mapaCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapa>();
            carlosAtributos = GetComponent<ComportamientoCarlos>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CompareTag("Untagged") && (Globals.Modo != "Multi" || Globals.Modo == "Multi" && photonView.isMine))
        {
            if (Input.GetKeyDown(CompareTag("Player") ? KeyCode.Space : KeyCode.Return))
            {
                Celda celdaCerca = null;

                celdaCerca = carlosAtributos.EncontrarCeldaMasCerca(transform.position);

                // Debug.Log(minDistancia);
                // Debug.Log(celdaCercana.obj.name);

                if (!celdaCerca.ocupado && !carlosAtributos.cargando
                                        && carlosAtributos.bombasEnMapa < carlosAtributos.limiteBombas)
                {
                    ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                    Transform bomba;

                    if (Globals.Modo == "Multi")
                    {
                        bomba = PhotonNetwork.Instantiate(projectilePrefab.name,
                            new Vector3(celdaCerca.posicionCelda.x, 0.25f, celdaCerca.posicionCelda.z),
                            projectilePrefab.transform.rotation, 0).transform;
                    }
                    else
                    {
                        bomba = Instantiate(projectilePrefab, new Vector3(celdaCerca.posicionCelda.x, 0.25f, celdaCerca.posicionCelda.z),
                            projectilePrefab.transform.rotation).transform;
                    }
                    celdaCerca.ocupado = true;
                    celdaCerca.objTipoCelda = bomba;
                    foreach (Transform tr in bomba.GetComponentsInChildren<Transform>())
                    {
                        if (tr.CompareTag("BombaSkin"))
                        {
                            tr.GetComponent<MeshRenderer>().material = carlosAtributos.skinBomba;
                        }
                    }
                    // explosionBomba(bomba, celdaCerca, carlosAtributos.alcanceBomba, carlosAtributos.duracionBomba);
                    Debug.Log("hola");
                }
            }
        }
    }

}

