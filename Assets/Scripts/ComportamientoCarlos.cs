using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: CARLOS EN BATE GIRA Y LE PEGA A TODO LO DE ALREDEDOR
public class ComportamientoCarlos : MonoBehaviour
{
    private SoltarBombas soltarBombas;
    
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

    // INDIVIDUAL
    public int vidas;



    // Start is called before the first frame update
    void Start()
    {
        controlador = GetComponent<CharacterController>();
        velocidad = velocidadInicial;
        soltarBombas = GetComponent<SoltarBombas>();
    }

    // Update is called once per frame
    void Update()
    {
        ControladorMovimiento();
        ControladorBate();
    }

    private void ControladorMovimiento()
    {
        float horizontal = Input.GetAxisRaw("J1_H");
        float vertical = Input.GetAxisRaw("J1_V");
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
                vista = "up";
            }
            if (vertical > 0)
            {
                vectorRotacion.y = 270;
                vista = "down";
            }

            transform.rotation = Quaternion.Euler(vectorRotacion);

        }
    }

    private void ControladorBate()
    {
        if (Input.GetKey("b"))
        {
            cargando = true;
            velocidad = velocidadInicial / 2;
            temporizadorCargado += Time.deltaTime;

            if (temporizadorCargado >= tiempoCargaBate)
            {
                velocidad = velocidadInicial / 3;
            }
        } else
        {
            velocidad = velocidadInicial;
        }

        if (Input.GetKeyUp("b") && temporizadorCargado >= tiempoCargaBate)
        {
            GolpearBomba(fuerzaBateCargado);
            temporizadorCargado = 0;
            cargando = false;
        }

        if (Input.GetKeyUp("b") && temporizadorCargado < tiempoCargaBate)
        {
            GolpearBomba(fuerzaBateNormal);
            temporizadorCargado = 0;
            cargando = false;
        }
    }

    private void GolpearBomba(int fuerza)
    {
        Celda celdaActual = soltarBombas.EncontrarCeldaMasCerca(transform.position);
        Celda[] celdasDireccion = soltarBombas.EncontrarCeldasCerca(vista, fuerza, celdaActual);

        if (celdasDireccion[0].objTipoCelda)
        {
            if (celdasDireccion[0].objTipoCelda.tag == "Bomba")
            {
                celdasDireccion[0].objTipoCelda.position = celdasDireccion[celdasDireccion.Length - 1].posicionCelda;
            }
        }

    }

    private void GestionarPowerUps(string tag)
    {
        string[] separador = new[] { "PU" };

        switch (tag.Split(separador, System.StringSplitOptions.None)[1]) {
            case "alcancebomba":
                if (alcanceBomba < 7) ++alcanceBomba;
                break;
            case "bombadiagonal":
                siguienteDiagonal = true;
                break;
            case "cantbombas":
                if (limiteBombas < 6) ++limiteBombas;
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
                if (limiteBombas > 1) --limiteBombas;
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
            GestionarPowerUps(other.tag);
            Destroy(other.gameObject);
        } else if (other.tag.StartsWith("PD"))
        {
            GestionarPowerDowns(other.tag);
            Destroy(other.gameObject);
        }
    }

    private IEnumerator NoBombas()
    {
        int limiteBombasAux = limiteBombas;
        limiteBombas = 0;
        yield return new WaitForSeconds(5);
        limiteBombas = limiteBombasAux;
    }


}
