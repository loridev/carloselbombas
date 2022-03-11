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
    public Transform prefabFinal;
    public Transform[] prefabNpcComunes;

    public Material[] texturaSuelos;
    public Material[] texturaParedes;

    public GameObject interfazIndividual;
    public GameObject interfazMultijugador;

    public static Celda[,] celdas;

    private Transform carlos;
    private Transform robotijo;

    //public static AstarPath scriptAi;
    private Level nivel;

    public GameObject labelVidas;
    public GameObject labelVelocidad;
    public GameObject labelAlcance;
    public GameObject labelTiempoDet;
    public GameObject labelCargaBat;
    public GameObject labelNumBom;

    async void Start()
    {
        if (Globals.Modo == "Indiv")
        {
            interfazIndividual.SetActive(true);
        } else
        {
            interfazMultijugador.SetActive(true);
        }
        nivel = await ApiRequests.GetLevel(Globals.WorldNum, Globals.LevelNum);
        GenerarMapa();
    }


    void Update()
    {
        ActualizarAtributos();
    }

    private void GenerarMapa()
    {
        celdas = new Celda[nivel.content.Count + 2, nivel.content.Count + 2];
        bool limiteMapa;

        for (int i = 0; i < nivel.content.Count + 2; i++)
        {
            for (int j = 0; j < nivel.content.Count + 2; j++)
            {
                limiteMapa = false;
                Vector3 posicion = new Vector3(i, 0, j);
                Transform obj = Instantiate(prefabCelda, posicion, Quaternion.identity);
                Transform objTipoCelda = null;
                bool ocupado = false;

                if (i == 0 || i == nivel.content.Count + 1 || j == 0 || j == nivel.content.Count + 1)
                {
                    Transform pared = Instantiate(prebabPared, posicion, Quaternion.identity);
                    pared.GetComponent<Renderer>().material = texturaParedes[nivel.worldNum - 1];
                    objTipoCelda = pared;
                    limiteMapa = true;
                    ocupado = true;
                } else
                {
                    switch (nivel.content[i - 1][j - 1])
                    {
                        case "Player":
                            carlos = Instantiate(prefabCarlos, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                            break;
                        case "Wall":
                            Transform pared = Instantiate(prebabPared, posicion, Quaternion.identity);
                            pared.GetComponent<Renderer>().material = texturaParedes[nivel.worldNum - 1];
                            objTipoCelda = pared;
                            ocupado = true;
                            break;
                        case "Box":
                            Transform caja = Instantiate(prefabCaja, new Vector3(posicion.x, 0.25f, posicion.z), Quaternion.identity);
                            objTipoCelda = caja;
                            ocupado = true;
                            break;
                        case "cNpc":
                            Transform npcSpawn;
                            if (nivel.worldNum != 3)
                            {
                                npcSpawn = Instantiate(prefabNpcComunes[nivel.worldNum - 1], posicion, Quaternion.identity);
                            } else
                            {
                                npcSpawn = Instantiate(prefabNpcComunes[nivel.worldNum - 1], new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                            }
                            npcSpawn.GetComponent<NPCComun>().isVertical = i % 2 != 0;
                            break;
                        case "fNpc":
                            robotijo = Instantiate(prefabNpcFinal, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                            break;
                        case "Exit":
                            Instantiate(prefabFinal, new Vector3(posicion.x, 0.35f, posicion.z), Quaternion.identity);
                            break;
                    }
                }
                obj.GetComponent<Renderer>().material = texturaSuelos[nivel.worldNum - 1];
                obj.name = "Celda " + i + "-" + j;
                celdas[i, j] = new Celda(ocupado, posicion, obj, objTipoCelda, limiteMapa);
            }
        }

        /*
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
        */

        //AsignarCarlosRobotijo();
    }

    private void ActualizarAtributos()
    {
        if (carlos != null)
        {
            ComportamientoCarlos carlosScript = carlos.GetComponent<ComportamientoCarlos>();

            labelVidas.GetComponent<UnityEngine.UI.Text>().text = "" + carlosScript.vidas;
            labelVelocidad.GetComponent<UnityEngine.UI.Text>().text = "" + carlosScript.velocidadInicial;
            labelAlcance.GetComponent<UnityEngine.UI.Text>().text = "" + carlosScript.alcanceBomba;
            labelTiempoDet.GetComponent<UnityEngine.UI.Text>().text = "" + carlosScript.duracionBomba;
            labelCargaBat.GetComponent<UnityEngine.UI.Text>().text = "" + carlosScript.tiempoCargaBate;
            labelNumBom.GetComponent<UnityEngine.UI.Text>().text = carlosScript.limiteBombas - carlosScript.bombasEnMapa + "/" + carlosScript.limiteBombas;
        }
    }

    private void AsignarCarlosRobotijo()
    {
        //AstarPath.active.Scan();
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