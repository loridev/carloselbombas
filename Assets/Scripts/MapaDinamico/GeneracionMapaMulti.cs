using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionMapaMulti
{

    public static void SpawnJugador(GameObject prefabCarlos, Vector3 pos)
    {
        GameObject carlitos = PhotonNetwork.Instantiate(prefabCarlos.name, pos, Quaternion.identity, 0);
        carlitos.GetComponent<ComportamientoCarlos>().vidas = 1;
    }
}
