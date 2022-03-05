using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionMapa : MonoBehaviour
{
    public Transform[] powerUps;
    public Transform[] powerDowns;

    public Transform prefabCelda;
    public Transform prebabPared;
    public Transform prefabCaja;
    public Transform prefabCarlos;
    public Transform prefabNpcFinal;
    public Transform prefabNpcComun;

    public Material texturaSuelo;
    public Material texturaPared;


    public int ancho;
    public int alto;

    public static Celda[,] celdas;

    private Transform carlos;
    private Transform robotijo;

    public static AstarPath scriptAi;

    async void Start()
    {
        Level nivel = await ApiRequests.GetLevel(1, 2);
        GenerarMapa();
    }


    void Update()
    {
        
    }

    private void GenerarMapa()
    {
        celdas = new Celda[ancho, alto];
        bool limiteMapa;

        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < alto; j++)
            {
                limiteMapa = false;
                Transform carlos;
                Transform robotijo;
                Vector3 posicion = new Vector3(i, 0, j);
                Transform obj = Instantiate(prefabCelda, posicion, Quaternion.identity);
                Transform objTipoCelda = null;
                if (i == 0 || i == ancho - 1 || j == 0 || j == alto - 1)
                {
                    Transform pared = Instantiate(prebabPared, posicion, Quaternion.identity);
                    pared.GetComponent<Renderer>().material = texturaPared;
                    objTipoCelda = pared;
                    limiteMapa = true;
                }

                if (i == 2 && j == 2)
                {
                    Instantiate(prefabCaja, new Vector3(posicion.x, 0.25f, posicion.z), Quaternion.identity);
                }

                if (i == 1 && j == alto - 2)
                {
                    carlos = Instantiate(prefabCarlos, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                }

                if (i == ancho - 2 && j == 1)
                {
                    //robotijo = Instantiate(prefabNpcFinal, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                }

                if (i == ancho / 2 && j == alto / 2)
                {
                    if (prefabNpcComun.name == "topo 1")
                    {
                        Instantiate(prefabNpcComun, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                    } else
                    {
                        Instantiate(prefabNpcComun, new Vector3(posicion.x, 0, posicion.z), Quaternion.identity);
                    }
                }

                if (i == ancho / 2 && j == alto / 2)
                {
                    Instantiate(powerDowns[2], new Vector3(posicion.x - 0.75f, 1, posicion.z - 0.25f), Quaternion.identity);
                }


                obj.GetComponent<Renderer>().material = texturaSuelo;
                obj.name = "Celda " + i + "-" + j;
                celdas[i, j] = new Celda(false, posicion, obj, objTipoCelda, limiteMapa);
            }
        }

        AsignarCarlosRobotijo();
    }

    private void AsignarCarlosRobotijo()
    {
        AstarPath.active.Scan();
    }
}

public class Celda
{
    public bool ocupado;
    public Vector3 posicionCelda;
    public Transform obj;
    public Transform objTipoCelda;
    public bool limiteMapa;
    public Celda(bool ocupado, Vector3 posicionCelda, Transform obj, Transform objTipoCelda, bool limiteMapa)
    {
        this.ocupado = ocupado;
        this.posicionCelda = posicionCelda;
        this.obj = obj;
        this.objTipoCelda = objTipoCelda;
        this.limiteMapa = limiteMapa;
    }
}