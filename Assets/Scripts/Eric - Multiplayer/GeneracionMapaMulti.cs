using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneracionMapaMulti : MonoBehaviour
{
    public static Celda[,] celdas;
    public GameObject interfazMultijugador;
    public GameObject[] cajasPlayers;
    public GameObject prefabCarlos;
    
    public Transform[] powerUps;
    public Transform[] powerDowns;
    
    private Vector3[] posicionesMulti =
    {
        new Vector3(1, 1, 1),
        new Vector3(13, 1, 13),
        new Vector3(13, 1, 1),
        new Vector3(1, 1, 13)
    };

    private GameObject[] carloses = {null, null, null, null};

    public GameObject panelMulti;
    public Text status;
    private void Start()
    {
        ComportamientoCarlos[] carlosActivos = FindObjectsOfType<ComportamientoCarlos>();
        for (int i = 0; i < carlosActivos.Length; i++)
        {
            carloses[i] = carlosActivos[i].gameObject;
        }
        celdas = new Celda[15, 15];
        AsignarCeldas(); // TODO: POSICIONES CARLOS Y INTERFAZ CON BOTON
    }

    public void SpawnJugador()
    {
        int index = 0;
        for (int i = 0; i < carloses.Length; i++)
        {
            if (carloses[i] == null)
            {
                index = i;
                break;
            }
        }
        
        GameObject carlitos = PhotonNetwork.Instantiate(prefabCarlos.name, posicionesMulti[index], Quaternion.identity, 0);
        carloses[index] = carlitos;
        cajasPlayers[index].SetActive(true);
        carlitos.GetComponent<ComportamientoCarlos>().vidas = 1;
        carlitos.GetComponent<ComportamientoCarlos>().celdas = celdas;
        
        panelMulti.SetActive(false);
    }

    private void AsignarCeldas()
    {
        foreach (GameObject celda in GameObject.FindGameObjectsWithTag("ground"))
        {
            Collider[] colliders = Physics.OverlapSphere(celda.transform.position, 0.00001f);
            Vector3 posicionCelda = celda.transform.position;

            Transform objTipoCelda = null;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Pared") || collider.CompareTag("Caja"))
                {
                    objTipoCelda = collider.transform;
                    break;
                }
                

            }
            celdas[(int) posicionCelda.x, (int) posicionCelda.z] = new Celda(
                    objTipoCelda != null, posicionCelda, objTipoCelda
            );
        }
    }
}
