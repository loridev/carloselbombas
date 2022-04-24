using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : MonoBehaviour
{
    private GeneracionMapa mapaCosas;
    private Transform[] powerUps;
    private Transform[] powerDowns;
    public GameObject projectilePrefab;
    private Celda[,] celdas;
    private ComportamientoCarlos carlosAtributos;
    public Transform particulaExplosion;
    // Start is called before the first frame update
    void Start()
    {
        mapaCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapa>();
        powerUps = mapaCosas.powerUps;
        powerDowns = mapaCosas.powerDowns;
        celdas = GeneracionMapa.celdas;
        carlosAtributos = GetComponent<ComportamientoCarlos>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Celda celdaCerca = null;

            celdaCerca = carlosAtributos.EncontrarCeldaMasCerca(transform.position);

            // Debug.Log(minDistancia);
            // Debug.Log(celdaCercana.obj.name);

            if (!celdaCerca.ocupado && !carlosAtributos.cargando
                && carlosAtributos.bombasEnMapa < carlosAtributos.limiteBombas)
            {
                ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                Transform bomba = Instantiate(projectilePrefab, new Vector3(celdaCerca.posicionCelda.x, 0.25f, celdaCerca.posicionCelda.z), projectilePrefab.transform.rotation).transform;
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

