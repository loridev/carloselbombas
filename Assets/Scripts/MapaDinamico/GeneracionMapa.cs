using Pathfinding;
using System.Collections;
using System.Collections.Generic;using System.Threading.Tasks;
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

    private static int vidasConst = 0;
    private static int velocidadConst = 0;
    private static int alcanceConst = 0;
    private static int tiempoDetConst = 0;
    private static int cargaBatConst = 0;
    private static int limiteConst = 0;

    private static int[] scores = {0, 0, 0, 0};

    private List<Vector3> posicionesMulti;

    public GameObject panelMulti;
    public Text status;

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
            } else if (Globals.Modo == "Pantalladiv")
            {
                cajasPlayers[2].SetActive(false);
                cajasPlayers[3].SetActive(false);
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
        } else if (Globals.Modo == "Pantalladiv")
        {
            segundos = 180;
            StartCoroutine(ContadorDesc());
        }
    }


    void Update()
    {
        ActualizarAtributos();
        if (Globals.Modo == "Pantalladiv")
        {
            ActualizarScore();
        }
        ControlarSalida();
        if (cargado && (Globals.Modo == "Contrarreloj" || Globals.Modo == "Pantalladiv"))
        {
            labelTime.text = segundos + "";
        }
    }

    private void GenerarMapa()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == 3)
            {
                for (int j = 0; j < scores.Length; j++)
                {
                    scores[j] = 0;
                }
            }
        }
        celdas = new Celda[nivel.content.Count + 2, nivel.content.Count + 2];

        for (int i = 0; i < nivel.content.Count + 2; i++)
        {
            int playerCount = 0;
            for (int j = 0; j < nivel.content.Count + 2; j++)
            {
                Vector3 posicion = new Vector3(i, 0, j);
                Transform obj = Instantiate(prefabCelda, posicion, Quaternion.identity);
                Transform objTipoCelda = null;
                bool ocupado = false;

                if (i == 0 || i == nivel.content.Count + 1 || j == 0 || j == nivel.content.Count + 1)
                {
                    Transform pared = Instantiate(prebabPared, posicion, Quaternion.identity);
                    pared.GetComponent<Renderer>().material = texturaParedes[nivel.worldNum - 1];
                    objTipoCelda = pared;
                    ocupado = true;
                } else
                {
                    switch (nivel.content[i - 1][j - 1])
                    {
                        case "Player":
                            playerCount++;
                            if (Globals.Modo == "Indiv" ||
                                (Globals.Modo == "Pantalladiv" && (posicion.x == posicion.z))
                                || Globals.Modo == "Contrarreloj")
                            {
                                carlos = Instantiate(prefabCarlos, new Vector3(posicion.x, 1, posicion.z), Quaternion.identity);
                                carlos.tag = playerCount != 1 && Globals.Modo == "Pantalladiv" ? "Player" + playerCount : "Player";
                                carlos.GetComponent<ComportamientoCarlos>()
                                    .CargarSkins(playerCount > 1 ? Globals.Player2 : Globals.CurrentUser);
                            }
                            if (Globals.Modo != "Indiv")
                            {
                                carlos.GetComponent<ComportamientoCarlos>().vidas = 1;
                            }
                            if (nivel.levelNum == 5 && nivel.worldNum != 4)
                            {
                                ComportamientoCarlos carlosScript = carlos.GetComponent<ComportamientoCarlos>();

                                carlosScript.vidas = vidasConst;
                                carlosScript.velocidadInicial = velocidadConst;
                                carlosScript.alcanceBomba = alcanceConst;
                                carlosScript.duracionBomba = tiempoDetConst;
                                carlosScript.tiempoCargaBate = cargaBatConst;
                                carlosScript.limiteBombas = limiteConst;
                                
                            }

                            if (Globals.Modo == "Pantalladiv" 
                                && GameObject.FindWithTag("Player") != null) carlos = GameObject.FindWithTag("Player").transform;
                            break;
                        case "Wall":
                            Transform pared = Instantiate(prebabPared, posicion, Quaternion.identity);
                            pared.GetComponent<Renderer>().material = texturaParedes[nivel.worldNum - 1];
                            objTipoCelda = pared;
                            ocupado = true;
                            break;
                        case "Box":
                            Transform caja;
                            caja = Instantiate(prefabCaja, new Vector3(posicion.x, 0.25f, posicion.z), Quaternion.identity);
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
                celdas[i, j] = new Celda(ocupado, posicion, objTipoCelda);
            }
        }

        AsignarCarlosRobotijo();
    }

    private void ActualizarAtributos()
    {
        if (carlos != null)
        {
            ComportamientoCarlos carlosScript = carlos.GetComponent<ComportamientoCarlos>();
            int index = interfazIndividual.activeSelf ? 0 : 1;
            vidasConst = carlosScript.vidas;
            velocidadConst = (int) carlosScript.velocidadInicial;
            alcanceConst = carlosScript.alcanceBomba;
            tiempoDetConst = carlosScript.duracionBomba;
            cargaBatConst = (int) carlosScript.tiempoCargaBate;
            limiteConst = carlosScript.limiteBombas;

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

    private void ControlarSalida()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (BGSoundScript.allAudios[8].isPlaying)
            {
                BGSoundScript.NacheteStop();
            }
            segundos = 0;
            SceneManager.LoadScene(Globals.Modo == "Pantalladiv" ? "MenuMulti" : "MenuMundos");
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

    private IEnumerator ContadorDesc()
    {
        while (segundos > 0)
        {
            segundos -= 1;
            if (segundos == 20 )
            {
                BGSoundScript.NachetePlay();
            }
            yield return new WaitForSeconds(1);
        }
        MuerteSubita();
    }

    private void MuerteSubita()
    {
        ComportamientoCarlos[] scriptsCarlos = FindObjectsOfType<ComportamientoCarlos>();
        foreach (ComportamientoCarlos carlos in scriptsCarlos)
        {
            carlos.velocidadInicial = 7;
            carlos.tiempoCargaBate = 2;
            carlos.alcanceBomba = 7;
            carlos.duracionBomba = 2;
            carlos.limiteBombas = 7;
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

    public void ActualizarScore()
    {
        cajasPlayers[0].GetComponentInChildren<Text>().text = Globals.CurrentUser.name + "\n" + "Rondas: " + scores[0] + "/3";
        cajasPlayers[1].GetComponentInChildren<Text>().text = Globals.Player2.name + "\n" + "Rondas: " + scores[1] + "/3";
    }

    public static IEnumerator EntreRondas(GameObject muerto)
    {
        if (BGSoundScript.allAudios[8].isPlaying)
        {
            BGSoundScript.NacheteStop();
        }
        ComportamientoBomba[] bombas = FindObjectsOfType<ComportamientoBomba>();

        foreach (ComportamientoBomba bomba in bombas)
        {
            Destroy(bomba.gameObject);
        }
        
        scores[muerto.CompareTag("Player") ? 1 : 0]++;
        muerto.transform.position = new Vector3(0, 50, 0);
        yield return new WaitForSeconds(3);
        bool acabada;
        Task<bool> taskAcabada = IsAcabada();
        yield return new WaitUntil(() => taskAcabada.IsCompleted);
        acabada = taskAcabada.Result;
        SceneManager.LoadScene(acabada ? "MenuMulti" : "MapaDinamicoFinal");
    }
    
    private static async Task<bool> IsAcabada()
    {
        bool acabada = false;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == 3)
            {
                acabada = true;
                User winner = i == 0 ? Globals.CurrentUser : Globals.Player2;
                User loser = i == 1 ? Globals.Player2 : Globals.CurrentUser;
                if (await ApiRequests.SaveMultiProgress(winner, loser, ++winner.multi_wins,
                        i == 0 ? scores[0] - scores[1] : scores[1] - scores[0]))
                {
                    Debug.Log("MULTI BIEN");
                }
                else
                {
                    Debug.Log("MULTI MAL :(");
                }
                break;
            }
        }

        return acabada;
    }
}


public class Celda
{
    public bool ocupado;
    public Vector3 posicionCelda;
    public Transform objTipoCelda;
    public Celda(bool ocupado, Vector3 posicionCelda, Transform objTipoCelda)
    {
        this.ocupado = ocupado;
        this.posicionCelda = posicionCelda;
        this.objTipoCelda = objTipoCelda;
    }
}