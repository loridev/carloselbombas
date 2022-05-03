using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoBomba : MonoBehaviour
{
    public bool diagonal;
    public Transform particulaExplosion;
    public Transform[] indicadores;
    private Transform[] powerUps;
    private Transform[] powerDowns;
    private Celda[,] celdas;
    private GeneracionMapa mapaCosas;
    private GeneracionMapaMulti mapaMultiCosas;
    public bool explotada;
    private Coroutine temporizador;
    public ComportamientoCarlos owner;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        
        foreach (Collider collider in colliders)
        {
            ComportamientoCarlos carlosScript = collider.GetComponent<ComportamientoCarlos>();
            if (carlosScript != null)
            {
                owner = carlosScript;
                break;
            }
        }
        explotada = false;
        celdas = Globals.Modo != "Multi" ? GeneracionMapa.celdas : GeneracionMapaMulti.celdas;

        if (Globals.Modo != "Multi")
        {
            mapaCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapa>();
        }
        else
        {
            mapaMultiCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapaMulti>();
        }
        powerUps = Globals.Modo != "Multi" ? mapaCosas.powerUps : mapaMultiCosas.powerUps;
        powerDowns = Globals.Modo != "Multi" ? mapaCosas.powerDowns : mapaMultiCosas.powerDowns;
        diagonal = owner.siguienteDiagonal;
        if (diagonal)
        {
            owner.siguienteDiagonal = false;
            Transform indicador;
            if (Globals.Modo != "Multi")
            {
                indicador = Instantiate(indicadores[0], transform.position, Quaternion.identity);
            }
            else
            {
                indicador = PhotonNetwork.Instantiate(indicadores[0].name, transform.position, Quaternion.identity, 0)
                    .transform;
            }

            indicador.parent = transform;
        }
        StartCoroutine(EsperarExplosion(owner.alcanceBomba, owner.duracionBomba));
    }
    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.useGravity = false;
            rb.isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (other.tag == "Player")
        {
            other.transform.position = new Vector3(other.transform.position.x, 1, other.transform.position.z);
        }
    }

    public IEnumerator EsperarExplosion(int alcanceBomba, int duracionBomba)
    {
        yield return new WaitForSeconds(duracionBomba);
        // ExplosionBomba(alcanceBomba);
        if (Globals.Modo != "Multi")
        {
            Instantiate(particulaExplosion, new Vector3(transform.position.x, 0.25f, transform.position.z),
                Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(particulaExplosion.name,
                new Vector3(transform.position.x, 0.25f, transform.position.z),
                Quaternion.identity, 0);
        }
    }

    private Celda EncontrarCeldaMasCerca(Vector3 posicion)
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
        }

        return celdaCercana;
    }

    private Celda[] EncontrarCeldasDiagonal(string direccion, int distancia, Celda celdaInicial)
    {
        Celda[] retorno = new Celda[distancia];
        Vector3 posSiguiente = new Vector3(0, 0, 0);
        Celda celdaSiguiente;

        switch (direccion)
        {
            case "upRight":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x + i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z + i);
                    celdaSiguiente = owner.EncontrarCelda(posSiguiente);
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
            case "upLeft":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x - i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z + i);
                    celdaSiguiente = owner.EncontrarCelda(posSiguiente);
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
            case "downRight":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x - i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z - i);
                    celdaSiguiente = owner.EncontrarCelda(posSiguiente);
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
            case "downLeft":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaInicial.posicionCelda.x + i, celdaInicial.posicionCelda.y, celdaInicial.posicionCelda.z - i);
                    celdaSiguiente = owner.EncontrarCelda(posSiguiente);
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

    public void ExplosionBomba(int alcanceBomba)
    {
        Celda celda = EncontrarCeldaMasCerca(transform.position);
        List<Celda> celdasExplosion = new List<Celda>();

        if (diagonal)
        {
            celdasExplosion.AddRange(EncontrarCeldasDiagonal("upRight", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasDiagonal("upLeft", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasDiagonal("downRight", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasDiagonal("downLeft", alcanceBomba, celda));
        }
        else
        {
            celdasExplosion.AddRange(owner.EncontrarCeldasCerca("up", alcanceBomba, celda));
            celdasExplosion.AddRange(owner.EncontrarCeldasCerca("down", alcanceBomba, celda));
            celdasExplosion.AddRange(owner.EncontrarCeldasCerca("left", alcanceBomba, celda));
            celdasExplosion.AddRange(owner.EncontrarCeldasCerca("right", alcanceBomba, celda));
        }
        if (!explotada)
        {
            if (!owner.CompareTag("Untagged"))
            {
                --owner.bombasEnMapa;
            }
            explotada = true;
        }

        //Destroy(gameObject);

        // Poner particulas de la bomba
        for (int i = 0; i < celdasExplosion.Count; i++)
        {
            if (celdasExplosion[i] != null)
            {
                if (Globals.Modo != "Multi")
                {
                    Instantiate(particulaExplosion, new Vector3(celdasExplosion[i].posicionCelda.x, 0.25f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                }
                else
                {
                    PhotonNetwork.Instantiate(particulaExplosion.name,
                        new Vector3(celdasExplosion[i].posicionCelda.x, 0.25f, celdasExplosion[i].posicionCelda.z),
                        Quaternion.identity, 0);
                }
                if (celdasExplosion[i].objTipoCelda != null)
                {
                    if (celdasExplosion[i].objTipoCelda.tag == "Caja")
                    {
                        //Sonido explosion caja
                        BGSoundScript.DestroyedBoxPlay();
                        Debug.Log("Box Destroyeddddddd");
                     
                        Destroy(celdasExplosion[i].objTipoCelda.gameObject, 0.1f);
                        celdasExplosion[i].objTipoCelda = null;

                        bool aparecer = UnityEngine.Random.Range(0, 10) <= 5;

                        if (aparecer)
                        {
                            bool powerUp = Random.Range(0, 10) <= 6;
                            int positionPower = Random.Range(0, 6);

                            if (powerUp)
                            {
                                if (Globals.Modo != "Multi")
                                {
                                    Instantiate(powerUps[positionPower], 
                                        new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), 
                                        Quaternion.identity);
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate(powerUps[positionPower].name, 
                                        new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), 
                                        Quaternion.identity, 0);
                                }
                            }
                            else
                            {
                                if (Globals.Modo != "Multi")
                                {
                                    Instantiate(powerDowns[positionPower], 
                                        new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), 
                                        Quaternion.identity);
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate(powerDowns[positionPower].name, 
                                        new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), 
                                        Quaternion.identity, 0);
                                }
                            }
                        }
                    }

                    celdasExplosion[i].ocupado = false;
                    celdasExplosion[i].objTipoCelda = null;


                }
            }
        }

        if (Globals.Modo != "Multi")
        {
            Destroy(gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
        celda.objTipoCelda = null;
        celda.ocupado = false;
        StopAllCoroutines();
    }
}