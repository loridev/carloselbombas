using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoBomba : MonoBehaviour
{
    public bool diagonal;
    public Transform particulaExplosion;
    private Transform[] powerUps;
    private Transform[] powerDowns;
    private Celda[,] celdas;
    private GeneracionMapa mapaCosas;
    private ComportamientoCarlos carlosAtributos;
    private bool explotada;
    private Coroutine temporizador;

    private void Start()
    {
        explotada = false;
        celdas = GeneracionMapa.celdas;
        mapaCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapa>();
        powerUps = mapaCosas.powerUps;
        powerDowns = mapaCosas.powerDowns;
        carlosAtributos = GameObject.FindGameObjectWithTag("Player").GetComponent<ComportamientoCarlos>();
        diagonal = carlosAtributos.siguienteDiagonal;
        if (diagonal) carlosAtributos.siguienteDiagonal = false;
        StartCoroutine(EsperarExplosion(carlosAtributos.alcanceBomba, carlosAtributos.duracionBomba));
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
        Instantiate(particulaExplosion, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
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

            // Debug.Log(distancia);
            // Debug.Log(celda.obj.name);
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
                    celdaSiguiente = carlosAtributos.EncontrarCelda(posSiguiente);
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
                    celdaSiguiente = carlosAtributos.EncontrarCelda(posSiguiente);
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
                    celdaSiguiente = carlosAtributos.EncontrarCelda(posSiguiente);
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
                    celdaSiguiente = carlosAtributos.EncontrarCelda(posSiguiente);
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
            celdasExplosion.AddRange(carlosAtributos.EncontrarCeldasCerca("up", alcanceBomba, celda));
            celdasExplosion.AddRange(carlosAtributos.EncontrarCeldasCerca("down", alcanceBomba, celda));
            celdasExplosion.AddRange(carlosAtributos.EncontrarCeldasCerca("left", alcanceBomba, celda));
            celdasExplosion.AddRange(carlosAtributos.EncontrarCeldasCerca("right", alcanceBomba, celda));
        }
        if (!explotada)
        {
            --carlosAtributos.bombasEnMapa;
            explotada = true;
        }

        //Destroy(gameObject);

        // Poner particulas de la bomba
        for (int i = 0; i < celdasExplosion.Count; i++)
        {
            //Debug.Log(celdasExplosion[i].posicionCelda);
            if (celdasExplosion[i] != null)
            {
                Debug.Log("Entrando celda" + i);
                Instantiate(particulaExplosion, new Vector3(celdasExplosion[i].posicionCelda.x, 0.25f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                if (celdasExplosion[i].objTipoCelda != null)
                {
                    if (celdasExplosion[i].objTipoCelda.tag == "Caja")
                    {
                        Destroy(celdasExplosion[i].objTipoCelda.gameObject);
                        celdasExplosion[i].objTipoCelda = null;

                        bool aparecer = UnityEngine.Random.Range(0, 10) <= 5;

                        if (aparecer)
                        {
                            bool powerUp = UnityEngine.Random.Range(0, 10) <= 6;
                            int positionPower = UnityEngine.Random.Range(0, 6);

                            if (powerUp)
                            {
                                Instantiate(powerUps[positionPower], new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                            }
                            else
                            {
                                Instantiate(powerDowns[positionPower], new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                            }
                        }
                    }

                    celdasExplosion[i].ocupado = false;
                    celdasExplosion[i].objTipoCelda = null;


                }
            }
        }
        celda.objTipoCelda = null;
        celda.ocupado = false;
        StopAllCoroutines();
        //Destroy(gameObject);
        // Quitar particulas y bomba (hacer otro script)




        // Restar la bomba una vez explotada:
    }
}
