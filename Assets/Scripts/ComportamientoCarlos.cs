using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComportamientoCarlos : Photon.MonoBehaviour
{
    public PhotonView photonView;
    private Celda[,] celdas;
    
    // RELATIVO A SKINS
    public Material[] skins;
    public Material[] coloresPersonajes;
    public GameObject[] partesNoPersonalizables;
    public GameObject[] partesPersonalizables;
    public Material skinBomba;
    public Material bombaDefault;
    private List<string> nombres = new List<string>{"HELMET", "BODY", "BAT", "BOMB"};
    private Material materialPersonaje;

    // INDICADORES
    public Transform[] indicadores;
    private Transform indicadorBateCargando;
    private Transform indicadorBateCargado;
    private Transform indicadorDiagonal;
    private Transform indicadorNoBombas;

    // VELOCIDAD
    public float velocidadInicial;
    private float velocidad;
    private CharacterController controlador;
    private string vista;

    // BATE
    public int fuerzaBateNormal;
    public int fuerzaBateCargado;
    public float tiempoCargaBate;
    private float temporizadorCargado = 0;
    public bool cargando = false;

    // BOMBAS
    public int alcanceBomba;
    public int duracionBomba;
    public int limiteBombas;
    public int bombasEnMapa;
    public bool siguienteDiagonal;
    public bool restarVidas;

    // INDIVIDUAL
    public int vidas = 3;
    // SONIDOS
    


    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(3, 3);
        if (!CompareTag("Untagged") && (Globals.Modo != "Multi" || Globals.Modo == "Multi" && photonView.isMine))
        {
            if (Globals.Modo == "Multi")
            {
                if (photonView.isMine)
                {
                    name = photonView.owner.NickName;
                }
            }
            skinBomba = bombaDefault;
            restarVidas = true;
            celdas = GeneracionMapa.celdas;
            controlador = GetComponent<CharacterController>();
            velocidad = velocidadInicial;
            materialPersonaje = Globals.CurrentUser.character == "CARLOS" ? coloresPersonajes[0] : coloresPersonajes[1];

            CargarSkins();  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CompareTag("Untagged") && (Globals.Modo != "Multi" || Globals.Modo == "Multi" && photonView.isMine))
        {
            ControladorMovimiento();
            ControladorBate();
        }
    }

    private void ControladorMovimiento()
    {
        float horizontal = CompareTag("Player") ? Input.GetAxisRaw("J1_H") : Input.GetAxisRaw("J2_H");
        float vertical = CompareTag("Player") ? Input.GetAxisRaw("J1_V") : Input.GetAxisRaw("J2_V");
        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        if (direccion.magnitude >= 0.1f)
        {
            Vector3 vectorRotacion = transform.rotation.eulerAngles;
            controlador.Move(direccion * velocidad * Time.deltaTime);

            if (horizontal < 0)
            {
                vectorRotacion.y = 180;
                vista = "left";
            }
            if (horizontal > 0)
            {
                vectorRotacion.y = 0;
                vista = "right";
            }
            if (vertical < 0)
            {
                vectorRotacion.y = 90;
                vista = "down";
            }
            if (vertical > 0)
            {
                vectorRotacion.y = 270;
                vista = "up";
            }

            transform.rotation = Quaternion.Euler(vectorRotacion);

        }
    }

    private void ControladorBate()
    {
        if (Input.GetKey(CompareTag("Player") ? KeyCode.B : KeyCode.Equals))
        {
            if (indicadorBateCargando == null && indicadorBateCargado == null)
            {
                indicadorBateCargando = Instantiate(indicadores[1], new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            }
            cargando = true;
            velocidad = velocidadInicial / 2;
            temporizadorCargado += Time.deltaTime;
            if (indicadorBateCargando != null) indicadorBateCargando.position = new Vector3(transform.position.x, 0, transform.position.z);

            if (temporizadorCargado >= tiempoCargaBate)
            {
                if (indicadorBateCargado == null)
                {
                    Destroy(indicadorBateCargando.gameObject);
                    indicadorBateCargando = null;

                    indicadorBateCargado = Instantiate(indicadores[0], new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                }
                indicadorBateCargado.position = new Vector3(transform.position.x, 0, transform.position.z);
                velocidad = velocidadInicial / 3;
            }
        } else
        {
            velocidad = velocidadInicial;
        }

        if (Input.GetKeyUp(CompareTag("Player") ? KeyCode.B : KeyCode.Equals) && temporizadorCargado >= tiempoCargaBate)
        {
            Destroy(indicadorBateCargado.gameObject);
            indicadorBateCargado = null;
            GolpearBomba(fuerzaBateCargado);
            temporizadorCargado = 0;
            cargando = false;
        } else if (Input.GetKeyUp(CompareTag("Player") ? KeyCode.B : KeyCode.Equals) && temporizadorCargado < tiempoCargaBate)
        {
            Destroy(indicadorBateCargando.gameObject);
            indicadorBateCargando = null;
            GolpearBomba(fuerzaBateNormal);
            temporizadorCargado = 0;
            cargando = false;
        }
    }

    private void GolpearBomba(int fuerza)
    {
        Celda celdaBomba = null;
        Celda celdaActual = EncontrarCeldaMasCerca(transform.position);
        if (celdaActual.objTipoCelda != null)
        {
            if (celdaActual.objTipoCelda.tag == "Bomba") celdaBomba = celdaActual;
        } 
        Vector3 supuestaPosBomba = new Vector3(0, 0, 0);


        if (celdaBomba == null)
        {
            switch (vista)
            {
                case "up":
                    supuestaPosBomba = new Vector3(celdaActual.posicionCelda.x, celdaActual.posicionCelda.y, celdaActual.posicionCelda.z + 1);
                    break;
                case "down":
                    supuestaPosBomba = new Vector3(celdaActual.posicionCelda.x, celdaActual.posicionCelda.y, celdaActual.posicionCelda.z - 1);
                    break;
                case "left":
                    supuestaPosBomba = new Vector3(celdaActual.posicionCelda.x - 1, celdaActual.posicionCelda.y, celdaActual.posicionCelda.z);
                    break;
                case "right":
                    supuestaPosBomba = new Vector3(celdaActual.posicionCelda.x + 1, celdaActual.posicionCelda.y, celdaActual.posicionCelda.z);
                    break;
            }
        }

        List<Celda> celdasColindantes = new List<Celda>();
        if (celdaBomba == null) celdaBomba = EncontrarCeldaMasCerca(supuestaPosBomba);

        celdasColindantes.AddRange(EncontrarCeldasCerca(vista, fuerza, celdaBomba));


        for (int i = celdasColindantes.Count - 1; i >= 0; i--)
        {
            Debug.Log("Hola");
            if (celdasColindantes[i] == null) celdasColindantes.Remove(celdasColindantes[i]);
        }
        if (celdasColindantes.Count == 0) return;

        if (celdasColindantes[0].objTipoCelda != null) return;


        if (celdaBomba.objTipoCelda != null)
        {
            if (celdaBomba.objTipoCelda.tag == "Bomba")
            {
                Celda celdaFinal;
                if (celdasColindantes.Count > fuerza)
                {
                    celdaFinal = celdasColindantes[fuerza - 1];
                } else if (celdasColindantes[celdasColindantes.Count - 1].objTipoCelda != null)
                {
                    celdaFinal = celdasColindantes[celdasColindantes.Count - 2];
                } else
                {
                    celdaFinal = celdasColindantes[celdasColindantes.Count - 1];
                }
                celdaBomba.objTipoCelda.position = new Vector3(celdaFinal.posicionCelda.x, 0.25f, celdaFinal.posicionCelda.z);
                celdaBomba.ocupado = false;
                celdaFinal.objTipoCelda = celdaBomba.objTipoCelda;
                celdaBomba.objTipoCelda = null;
                celdaFinal.ocupado = true;
                
            }
        }

    }

    private async void GestionarPowerUps(string tag)
    {
        string[] separador = { "PU" };

        switch (tag.Split(separador, System.StringSplitOptions.None)[1]) {
            case "alcancebomba":
                if (alcanceBomba < 7) ++alcanceBomba;
                break;
            case "bombadiagonal":
                siguienteDiagonal = true;
                break;
            case "cantbombas":
                if (limiteBombas < 7) ++limiteBombas;
                break;
            case "cargabate":
                if (tiempoCargaBate > 2) --tiempoCargaBate;
                break;
            case "tiempodetonacion":
                if (duracionBomba > 2) --duracionBomba;
                break;
            case "velocidad":
                if (velocidad < 7)
                {
                    ++velocidad;
                    ++velocidadInicial;
                }
                break;
            case "finalNivel":
                if (Globals.LevelNum != 4 || Globals.LevelNum == 5)
                {
                    if (Globals.Modo == "Contrarreloj")
                    {
                        GeneracionMapa.GuardarRankingJugador();
                    } else if (Globals.CurrentUser.GetWorldNum() == Globals.WorldNum & (Globals.CurrentUser.GetLevelNum() == Globals.LevelNum
                        || (Globals.CurrentUser.GetLevelNum() == 4 && Globals.LevelNum == 5)))
                    {
                        Globals.CurrentUser.indiv_level = Globals.CurrentUser.GetLevelNum() == 4 ? (Globals.CurrentUser.GetWorldNum() + 1) + "-" + 1
                            : Globals.CurrentUser.GetWorldNum() + "-" + (Globals.CurrentUser.GetLevelNum() + 1);
                        if (await ApiRequests.SaveProgress(Globals.CurrentUser, Globals.CurrentUser.indiv_level))
                        {
                            Debug.Log("PROGRESO BIEN");
                        } else
                        {
                            Debug.Log("PROGRESO MAL :(");
                        }
                    }

                    if (Globals.LevelNum == 5)
                    {
                        SceneManager.LoadScene("MenuIndiv");
                        return;
                    }
                    
                    SceneManager.LoadScene("MenuMundos");
                } else if (Globals.LevelNum == 4)
                {
                    Globals.LevelNum = 5;
                    SceneManager.LoadScene("MapaDinamicoFinal");
                }
                break;
        }
    }

    private void GestionarPowerDowns(string tag)
    {
        string[] separador = new[] { "PD" };

        switch (tag.Split(separador, System.StringSplitOptions.None)[1])
        {
            case "alcancebomba":
                if (alcanceBomba > 2) --alcanceBomba;
                break;
            case "nobombas":
                StartCoroutine(NoBombas());
                break;
            case "cantbombas":
                if (limiteBombas > 2) --limiteBombas;
                break;
            case "cargabate":
                if (tiempoCargaBate < 7) ++tiempoCargaBate;
                break;
            case "tiempodetonacion":
                if (duracionBomba < 7) ++duracionBomba;
                break;
            case "velocidad":
                if (velocidad > 2)
                {
                    --velocidad;
                    --velocidadInicial;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("PU"))
        {
            // other.GetComponent<ComportamientoPowerups>().powerUpAudio.Play();
            GestionarPowerUps(other.tag);
            Destroy(other.gameObject);
        } else if (other.tag.StartsWith("PD"))
        {
            //other.GetComponent<ComportamientoPowerups>().powerUpAudio.Play();
            GestionarPowerDowns(other.tag);
            Destroy(other.gameObject);
        } else if (other.CompareTag("Particula") || other.CompareTag("cNpc") || other.CompareTag("fNpc"))
        {
            if (other.CompareTag("Particula"))
            {
                Celda celdaCarlos = EncontrarCeldaMasCerca(new Vector3(transform.position.x, 0, transform.position.z));
                Collider[] colliders = Physics.OverlapSphere(celdaCarlos.posicionCelda, 0.00001f);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Bomba"))
                    {
                        ComportamientoBomba bomba = collider.gameObject.GetComponent<ComportamientoBomba>();
                        if (!bomba.explotada)
                        {
                            bomba.ExplosionBomba(alcanceBomba);
                        }

                    }
                }
            }

            if (restarVidas)
            {
                --vidas;

                if (vidas == 0)
                {
                    GeneracionMapa.segundos = 0;
                    if (Globals.Modo == "Indiv" || Globals.Modo == "Contrarreloj")
                    {
                        SceneManager.LoadScene("MenuMundos");
                    }
                    else
                    {
                        if (Globals.Modo == "Pantalladiv")
                        {
                            StartCoroutine(GeneracionMapa.EntreRondas(gameObject));
                        }
                    }
                }
                StartCoroutine(EsperarVidas());
            }
        }
    }

    private IEnumerator NoBombas()
    {
        int limiteBombasAux = limiteBombas;
        limiteBombas = 0;
        Transform indicadorNoBombas = Instantiate(indicadores[2], new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        indicadorNoBombas.parent = transform;
        yield return new WaitForSeconds(5);
        Destroy(indicadorNoBombas.gameObject);
        limiteBombas = limiteBombasAux;
    }

    public Celda[] EncontrarCeldasCerca(string direccion, int distancia, Celda celdaInicial)
    {
        Celda[] retorno = new Celda[distancia];
        Vector3 posSiguiente = new Vector3(0, 0, 0);
        Celda celdaSiguiente;

        switch (direccion)
        {
            case "up":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z + i);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    if (celdaSiguiente != null)
                    {
                        if (celdaSiguiente.objTipoCelda != null)
                        {
                            if (celdaSiguiente.objTipoCelda.tag == "Pared") return retorno;
                        }
                        if (celdaSiguiente.ocupado)
                        {
                            retorno[i - 1] = celdaSiguiente;
                            return retorno;
                        }
                        retorno[i - 1] = celdaSiguiente;
                    }
                    else return retorno;
                }
                break;
            case "down":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z - i);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    if (celdaSiguiente != null)
                    {
                        if (celdaSiguiente.objTipoCelda != null)
                        {
                            if (celdaSiguiente.objTipoCelda.tag == "Pared") return retorno;
                        }
                        if (celdaSiguiente.ocupado)
                        {
                            retorno[i - 1] = celdaSiguiente;
                            return retorno;
                        }
                        retorno[i - 1] = celdaSiguiente;
                    }
                    else return retorno;
                }
                break;
            case "left":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x - i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    if (celdaSiguiente != null)
                    {
                        if (celdaSiguiente.objTipoCelda != null)
                        {
                            if (celdaSiguiente.objTipoCelda.tag == "Pared") return retorno;
                        }
                        if (celdaSiguiente.ocupado)
                        {
                            retorno[i - 1] = celdaSiguiente;
                            return retorno;
                        }
                        retorno[i - 1] = celdaSiguiente;
                    }
                    else return retorno;
                }
                break;
            case "right":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x + i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    if (celdaSiguiente != null)
                    {
                        if (celdaSiguiente.objTipoCelda != null)
                        {
                            if (celdaSiguiente.objTipoCelda.tag == "Pared") return retorno;
                        }
                        if (celdaSiguiente.ocupado)
                        {
                            retorno[i - 1] = celdaSiguiente;
                            return retorno;
                        }
                        retorno[i - 1] = celdaSiguiente;
                    }
                    else return retorno;
                }
                break;
        }

        return retorno;
    }

    public Celda EncontrarCelda(Vector3 posicion)
    {
        foreach (Celda celda in celdas)
        {
            if (celda.posicionCelda == posicion) return celda;
        }
        return null;
    }

    public Celda EncontrarCeldaMasCerca(Vector3 posicion)
    {
        float minDistancia = float.MaxValue;
        Celda celdaCercana = null;

        foreach (Celda celda in celdas)
        {
            float distancia = Vector3.Distance(posicion, celda.posicionCelda);
            if (distancia < minDistancia)
            {
                minDistancia = distancia;
                celdaCercana = celda;
            }

            // Debug.Log(distancia);
            // Debug.Log(celda.obj.name);
        }

        return celdaCercana;
    }


    public IEnumerator EsperarVidas()
    {
        restarVidas = false;
        yield return new WaitForSeconds(2);
        restarVidas = true;
    }

    private void CargarSkins()
    {
        List<Item> equipped = Globals.CurrentUser.equippedItems;
        for (int i = 0; i < skins.Length; i++)
        {
            for (int j = 0; j < equipped.Count; j++)
            {
                if (equipped[j].skin_texture == skins[i].name)
                {
                    for (int k = 0; k < nombres.Count; k++)
                    {
                        if (nombres[k] == equipped[j].type)
                        {
                            if (nombres[k] != "BOMB")
                            {
                                partesPersonalizables[k].GetComponent<MeshRenderer>().material = skins[i];
                            }
                            else
                            {
                                skinBomba = skins[i];
                            }
                        }
                    }
                }
            }
        }

        if (Globals.CurrentUser.equippedItems.Find((item) => item.type == "HELMET") == null)
        {
            partesPersonalizables[0].SetActive(false);
        }
        if (Globals.CurrentUser.equippedItems.Find((item) => item.type == "BODY") == null)
        {
            partesPersonalizables[1].GetComponent<MeshRenderer>().material = materialPersonaje;
        }
        if (Globals.CurrentUser.equippedItems.Find((item) => item.type == "BAT") == null)
        {
            partesPersonalizables[2].GetComponent<MeshRenderer>().material = materialPersonaje;
        }


        foreach (GameObject parte in partesNoPersonalizables)
        {
            parte.GetComponent<MeshRenderer>().material = materialPersonaje;
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Particula"))
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }
}
