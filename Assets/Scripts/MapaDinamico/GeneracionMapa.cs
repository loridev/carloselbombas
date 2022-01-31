using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionMapa : MonoBehaviour
{
    public Transform prefabCelda;
    public Transform prebabPared;
    public Transform prefabCarlos;
    public Transform prefabNPC;

    public Material texturaSuelo;

    public int ancho;
    public int alto;
    private Celda[,] celdas;

    private Transform carlos;
    private Transform robotijo;

    void Start()
    {
        GenerarMapa();
        AsignarCarlosRobotijo();
    }


    void Update()
    {
        
    }

    private void GenerarMapa()
    {
        celdas = new Celda[ancho, alto];

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                Transform carlos;
                Transform robotijo;
                Vector3 posicion = new Vector3(i, 0, j);
                Transform obj = Instantiate(prefabCelda, posicion, Quaternion.identity);
                if (i == 0 || i == ancho - 1 || j == 0 || j == alto - 1)
                {
                    Instantiate(prebabPared, posicion, Quaternion.identity);
                }

                if (i == 1 && j == alto - 2)
                {
                    carlos = Instantiate(prefabCarlos, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                }

                if (i == ancho - 2 && j == 1)
                {
                    robotijo = Instantiate(prefabNPC, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                }


                obj.GetComponent<Renderer>().material = texturaSuelo;
                obj.name = "Celda " + i + "-" + j;
                celdas[i, j] = new Celda(false, posicion, obj);
            }
        }
    }

    private void AsignarCarlosRobotijo()
    {
        robotijo.GetComponent<AIDestinationSetter>().target = carlos.transform;
    }
}

public class Celda
{
    public bool ocupado;
    public Vector3 posicionCelda;
    public Transform obj;

    public Celda(bool ocupado, Vector3 posicionCelda, Transform obj)
    {
        this.ocupado = ocupado;
        this.posicionCelda = posicionCelda;
        this.obj = obj;
    }
}