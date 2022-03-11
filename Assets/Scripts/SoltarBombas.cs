using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : MonoBehaviour
{
    private GeneracionMapa mapaCosas;
    private Transform[] powerUps;
    private Transform[] powerDowns;
    public GameObject projectilePrefab;
    private Celda[,] celdas;
    private ComportamientoCarlos carlosAtributos;
    public Transform particulaExplosion;
    // Start is called before the first frame update
    void Start()
    {
        mapaCosas = GameObject.FindGameObjectWithTag("Mapa").GetComponent<GeneracionMapa>();
        powerUps = mapaCosas.powerUps;
        powerDowns = mapaCosas.powerDowns;
        celdas = GeneracionMapa.celdas;
        carlosAtributos = GetComponent<ComportamientoCarlos>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Celda celdaCerca = null;

            celdaCerca = EncontrarCeldaMasCerca(transform.position);

            // Debug.Log(minDistancia);
            // Debug.Log(celdaCercana.obj.name);

            if (!celdaCerca.ocupado && !carlosAtributos.cargando
                && carlosAtributos.bombasEnMapa < carlosAtributos.limiteBombas)
            {
                ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                Transform bomba = Instantiate(projectilePrefab, new Vector3(celdaCerca.posicionCelda.x, 0.25f, celdaCerca.posicionCelda.z), projectilePrefab.transform.rotation).transform;
                celdaCerca.ocupado = true;
                celdaCerca.objTipoCelda = bomba;
                bomba.GetComponent<ComportamientoBomba>().explotar = true;
                StartCoroutine(EsperarExplosion(bomba, carlosAtributos.alcanceBomba, carlosAtributos.duracionBomba));
                // explosionBomba(bomba, celdaCerca, carlosAtributos.alcanceBomba, carlosAtributos.duracionBomba);
                Debug.Log("hola");
            }
        }
    }

    public void ExplosionBomba(Transform bomba, int alcanceBomba)
    {
        Celda celda = EncontrarCeldaMasCerca(bomba.position);
        bool siguienteDiagonal = carlosAtributos.siguienteDiagonal;
        List<Celda> celdasExplosion = new List<Celda>();

        if (siguienteDiagonal)
        {
            siguienteDiagonal = false;
            // Logica bomba diagonal
        } else
        {
            celdasExplosion.AddRange(EncontrarCeldasCerca("up", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasCerca("down", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasCerca("left", alcanceBomba, celda));
            celdasExplosion.AddRange(EncontrarCeldasCerca("right", alcanceBomba, celda));
        }

        // Poner particulas de la bomba
        for (int i = 0; i < celdasExplosion.Count; i++)
        {
            //Debug.Log(celdasExplosion[i].posicionCelda);
            if (celdasExplosion[i] != null)
            {
                Instantiate(particulaExplosion, new Vector3(celdasExplosion[i].posicionCelda.x, 0.25f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                if (celdasExplosion[i].objTipoCelda != null)
                {
                    if (celdasExplosion[i].objTipoCelda.tag == "Caja")
                    {
                        Destroy(celdasExplosion[i].objTipoCelda.gameObject);
                        celdasExplosion[i].objTipoCelda = null;
                    }

                    bool aparecer = UnityEngine.Random.Range(0, 10) <= 7;

                    if (aparecer)
                    {
                        int typePower = UnityEngine.Random.Range(0, 2);
                        int positionPower = UnityEngine.Random.Range(0, 6);

                        if (typePower == 0)
                        {
                            Instantiate(powerUps[positionPower], new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                        } else
                        {
                            Instantiate(powerDowns[positionPower], new Vector3(celdasExplosion[i].posicionCelda.x, 0.5f, celdasExplosion[i].posicionCelda.z), Quaternion.identity);
                        }
                    }

                    celdasExplosion[i].ocupado = false;
                    celdasExplosion[i].objTipoCelda = null;

                    if (bomba != null)
                    {
                        Destroy(bomba.gameObject);
                        bomba = null;
                    }
                    celda.objTipoCelda = null;
                    celda.ocupado = false;
                }
            }
        }
        // Quitar particulas y bomba (hacer otro script)


        

        // Restar la bomba una vez explotada:
        --GetComponent<ComportamientoCarlos>().bombasEnMapa;
    }

    private IEnumerator EsperarExplosion(Transform bomba, int alcanceBomba, int duracionBomba)
    {
        yield return new WaitForSeconds(duracionBomba);
        if (bomba != null)
        {
            if (bomba.GetComponent<ComportamientoBomba>().explotar) ExplosionBomba(bomba, alcanceBomba);
        }
    }

    public Celda EncontrarCelda(Vector3 posicion)
    {
        foreach (Celda celda in celdas)
        {
            if (celda.posicionCelda == posicion) return celda;
        } return null;
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

    public Celda[] EncontrarCeldasCerca(string direccion, int distancia, Celda celdaIncial)
    {
        Celda[] retorno = new Celda[distancia];
        Vector3 posSiguiente = new Vector3(0, 0, 0);
        Celda celdaSiguiente;

        switch (direccion) {
            case "up":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaIncial.posicionCelda.x, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z + i);
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
                     } else return retorno;
                }
                break;
            case "down":
                for (int i = 1; i <= distancia; i++)
                {
                    posSiguiente = new Vector3(celdaIncial.posicionCelda.x, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z - i);
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
                    posSiguiente = new Vector3(celdaIncial.posicionCelda.x - i, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z);
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
                    posSiguiente = new Vector3(celdaIncial.posicionCelda.x + i, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z);
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
}

