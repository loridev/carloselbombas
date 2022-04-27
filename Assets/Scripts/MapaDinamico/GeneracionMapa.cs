using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private Level nivel;

    public Text labelVidas;
    public Text[] labelVelocidad;
    public Text[] labelAlcance;
    public Text[] labelTiempoDet;
    public Text[] labelCargaBat;
    public Text[] labelNumBom;

    public GameObject mainCamera;

    public Text labelTime;
    private bool cargado = false;
    public static int segundos = 0;
    public GameObject[] cajasPlayers;

    async void Start()
    {
        if (Globals.Modo == "Indiv")
        {
            interfazIndividual.SetActive(true);
        } else
        {
            interfazMultijugador.SetActive(true);
            if (Globals.Modo == "Contrarreloj")
            {
                foreach (GameObject caja in cajasPlayers)
                {
                    caja.SetActive(false);
                }
            }
        }
        nivel = await ApiRequests.GetLevel(Globals.WorldNum, Globals.LevelNum);
        switch (nivel.worldNum)
        {
            case 1:
                mainCamera.transform.position = new Vector3(4.5f, 7, -0.5f);
                break;
            case 2:
                mainCamera.transform.position = new Vector3(6, 12, -1f);
                break;
            case 3:
                mainCamera.transform.position = new Vector3(8.5f, 15, -1f);
                break;
            case 4:
                mainCamera.transform.position = new Vector3(7, 12, -1f);
                break;
        }
        GenerarMapa();

        cargado = true;
        if (Globals.Modo == "Contrarreloj")
        {
            StartCoroutine(ContadorSegundos());
        }
    }


    void Update()
    {
        ActualizarAtributos();
        ControlarPausa();
        if (cargado && Globals.Modo == "Contrarreloj")
        {
            labelTime.text = segundos + "";
        }
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
                            if (Globals.Modo != "Indiv")
                            {
                                var carlosPlayer = new List<Transform> { };
                                foreach (Transform carlos in carlosPlayer)
                                {
                                    //carlosPlayer.Count();
                                    carlos.GetComponent<ComportamientoCarlos>().vidas = 1;
                                }
                            }
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
                            robotijo = Instantiate(prefabNpcFinal, new Vector3(posicion.x, 0, posicion.z), Quaternion.identity);
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

        AsignarCarlosRobotijo();
    }

    private void ActualizarAtributos()
    {
        if (carlos != null)
        {
            ComportamientoCarlos carlosScript = carlos.GetComponent<ComportamientoCarlos>();
            int index = interfazIndividual.GetActive() ? 0 : 1;

            labelVidas.text = "" + carlosScript.vidas;
            labelVelocidad[index].text = "" + carlosScript.velocidadInicial;
            labelAlcance[index].text = "" + carlosScript.alcanceBomba;
            labelTiempoDet[index].text = "" + carlosScript.duracionBomba;
            labelCargaBat[index].text = "" + carlosScript.tiempoCargaBate;
            labelNumBom[index].text = carlosScript.limiteBombas - carlosScript.bombasEnMapa + "/" + carlosScript.limiteBombas;
        }
    }

    private void AsignarCarlosRobotijo()
    {
        if (robotijo != null)
        {
            robotijo.GetComponent<AIDestinationSetter>().target = carlos;
            AstarPath.active.Scan();
            AstarPath.active.data.recastGraph.SnapForceBoundsToScene();
        }
    }

    private void ControlarPausa()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            segundos = 0;
            SceneManager.LoadScene("MenuMundos");
        }
    }

    private IEnumerator ContadorSegundos()
    {
        while (true)
        {
            segundos += 1;
            yield return new WaitForSeconds(1);
        }
    }

    public static async void GuardarRankingJugador()
    {
        if (await ApiRequests.AddIndivRanking(Globals.CurrentUser, Globals.WorldNum, Globals.LevelNum, segundos))
        {
            Debug.Log("RANKING BIEN");
        }
        else
        {
            Debug.Log("RANKING MAL");
        }
        
        if (Globals.LevelNum != 4) segundos = 0;
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