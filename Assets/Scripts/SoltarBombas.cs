using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoltarBombas : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Celda[,] celdas;
    private ComportamientoCarlos carlosAtributos;
    // Start is called before the first frame update
    void Start()
    {
        celdas = GeneracionMapa.celdas;
        carlosAtributos = GetComponent<ComportamientoCarlos>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float minDistancia = float.MaxValue;
            Celda celdaCercana = null;

            foreach (Celda celda in celdas)
            {
                float distancia = Vector3.Distance(transform.position, celda.posicionCelda);
                if (distancia < minDistancia)
                {
                    minDistancia = distancia;
                    celdaCercana = celda;
                }

                // Debug.Log(distancia);
                // Debug.Log(celda.obj.name);
            }

            // Debug.Log(minDistancia);
            // Debug.Log(celdaCercana.obj.name);

            if (!celdaCercana.ocupado && !carlosAtributos.cargando
                && carlosAtributos.bombasEnMapa < carlosAtributos.limiteBombas)
            {
                ++GetComponent<ComportamientoCarlos>().bombasEnMapa;
                Transform bomba = Instantiate(projectilePrefab, new Vector3(celdaCercana.posicionCelda.x, 0.25f, celdaCercana.posicionCelda.z), projectilePrefab.transform.rotation).transform;
                celdaCercana.ocupado = true;
                explosionBomba(bomba,celdaCercana, carlosAtributos.alcanceBomba, carlosAtributos.duracionBomba);
                Debug.Log("hola");
            }
        }
    }

    private void explosionBomba(Transform bomba, Celda celda, int alcanceBomba, int duracionBomba)
    {
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
        Debug.Log("bomba colocada");

        Debug.Log(celda.posicionCelda);

        Debug.Log("bomba colocada");

        for (int i = 1; i < celdasExplosion.Count; i++)
        {
            Debug.Log(celdasExplosion[i].posicionCelda);
            Instantiate(bomba, celdasExplosion[i].posicionCelda, Quaternion.identity);
        }
        

        // Restar la bomba una vez explotada:
        // --GetComponent<ComportamientoCarlos>().bombasEnMapa;
    }

    private Celda EncontrarCelda(Vector3 posicion)
    {
        foreach (Celda celda in celdas)
        {
            if (celda.posicionCelda == posicion) return celda;
        } return null;
    }

    public Celda[] EncontrarCeldasCerca(string direccion, int distancia, Celda celdaIncial)
    {
        Celda[] retorno = new Celda[distancia];
        bool seguir = true;
        Vector3 posSiguiente = new Vector3(0, 0, 0);
        Celda celdaSiguiente;

        switch (direccion) {
            case "up":
                for (int i = 1; i <= distancia; i++)
                {
                    if (seguir) posSiguiente = new Vector3(celdaIncial.posicionCelda.x, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z + i);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    retorno[i - 1] = celdaSiguiente;
                    if (celdaSiguiente.ocupado || celdaSiguiente == null) return retorno;
                }
                break;
            case "down":
                for (int i = 1; i <= distancia; i++)
                {
                    if (seguir) posSiguiente = new Vector3(celdaIncial.posicionCelda.x, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z - i);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    retorno[i - 1] = celdaSiguiente;
                    if (celdaSiguiente.ocupado || celdaSiguiente == null) return retorno;
                }
                break;
            case "left":
                for (int i = 1; i <= distancia; i++)
                {
                    if (seguir) posSiguiente = new Vector3(celdaIncial.posicionCelda.x - i, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    retorno[i - 1] = celdaSiguiente;
                    if (celdaSiguiente.ocupado || celdaSiguiente == null) return retorno;
                }
                break;
            case "right":
                for (int i = 1; i <= distancia; i++)
                {
                    if (seguir) posSiguiente = new Vector3(celdaIncial.posicionCelda.x + i, celdaIncial.posicionCelda.y, celdaIncial.posicionCelda.z);
                    celdaSiguiente = EncontrarCelda(posSiguiente);
                    retorno[i - 1] = celdaSiguiente;
                    if (celdaSiguiente.ocupado || celdaSiguiente == null) return retorno;
                }
                break;
        }

        return retorno;
    }
}

