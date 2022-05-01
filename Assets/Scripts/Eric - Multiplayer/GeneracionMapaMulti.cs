using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionMapaMulti : MonoBehaviour
{
    private Celda[,] celdas;
    private void Start()
    {
        celdas = new Celda[15, 15];
        AsignarCeldas();
        Debug.Log(celdas); // TODO: POSICIONES CARLOS Y INTERFAZ CON BOTON
    }

    public static void SpawnJugador(GameObject prefabCarlos, Vector3 pos)
    {
        GameObject carlitos = PhotonNetwork.Instantiate(prefabCarlos.name, pos, Quaternion.identity, 0);
        carlitos.GetComponent<ComportamientoCarlos>().vidas = 1;
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
