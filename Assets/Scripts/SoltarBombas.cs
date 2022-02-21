using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Celda[,] celdas;
    // Start is called before the first frame update
    void Start()
    {
        celdas = GeneracionMapa.celdas;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float minDistancia = float.MaxValue;
            Celda celdaCercana = null;

            foreach (Celda celda in celdas)
            {
                float distancia = Vector3.Distance(transform.position, celda.posicionCelda);
                if (distancia < minDistancia)
                {
                    minDistancia = distancia;
                    celdaCercana = celda;
                }

                // Debug.Log(distancia);
                // Debug.Log(celda.obj.name);
            }

            // Debug.Log(minDistancia);
            // Debug.Log(celdaCercana.obj.name);

            if (!celdaCercana.ocupado && !GetComponent<ComportamientoCarlos>().cargando
                && GetComponent<ComportamientoCarlos>().bombasEnMapa < GetComponent<ComportamientoCarlos>().limiteBombas)
            {
                ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                Transform bomba = Instantiate(projectilePrefab, new Vector3(celdaCercana.posicionCelda.x, 0.25f, celdaCercana.posicionCelda.z), projectilePrefab.transform.rotation).transform;
                celdaCercana.ocupado = true;
                explosionBomba(bomba,celdaCercana, GetComponent<ComportamientoCarlos>().alcanceBomba, GetComponent<ComportamientoCarlos>().duracionBomba);
                Debug.Log("hola");
            }
        }
    }

    private void explosionBomba(Transform bomba, Celda celda, int alcanceBomba, int duracionBomba)
    {
        Debug.Log("bomba colocada");

        Debug.Log(celda.posicionCelda);

        


        // --GetComponent<ComportamientoCarlos>().bombasEnMapa;
    }
}

