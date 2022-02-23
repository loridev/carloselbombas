using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: CARLOS EN BATE GIRA Y LE PEGA A TODO LO DE ALREDEDOR
public class ComportamientoCarlos : MonoBehaviour
{
    
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



    // Start is called before the first frame update
    void Start()
    {
        controlador = GetComponent<CharacterController>();
        velocidad = velocidadInicial;
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
                vista = "izq";
            }
            if (horizontal > 0)
            {
                vectorRotacion.y = 0;
                vista = "der";
            }
            if (vertical < 0)
            {
                vectorRotacion.y = 90;
                vista = "abajo";
            }
            if (vertical > 0)
            {
                vectorRotacion.y = 270;
                vista = "arriba";
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

    }

    private void GestionarPowerUps(string tag)
    {
        string[] separador = new[] { "PU" };

        switch (tag.Split(separador, System.StringSplitOptions.None)[1]) {
            case "alcancebomba":
                ++alcanceBomba;
                break;
            case "bombadiagonal":
                // diagonal = true
                break;
            case "cantbombas":
                ++limiteBombas;
                break;
            case "cargabate":
                --tiempoCargaBate;
                break;
            case "tiempodetonacion":
                --duracionBomba;
                break;
            case "velocidad":
                ++velocidad;
                ++velocidadInicial;
                break;
        }
    }

    private void GestionarPowerDowns(string tag)
    {
        string[] separador = new[] { "PD" };

        switch (tag.Split(separador, System.StringSplitOptions.None)[1])
        {
            case "alcancebomba":
                --alcanceBomba;
                break;
            case "nobombas":
                // contador limite bombas 0
                break;
            case "cantbombas":
                --limiteBombas;
                break;
            case "cargabate":
                ++tiempoCargaBate;
                Debug.Log(tag.Split(separador, System.StringSplitOptions.None)[1]);
                break;
            case "tiempodetonacion":
                ++duracionBomba;
                break;
            case "velocidad":
                --velocidad;
                --velocidadInicial;
                Debug.Log(velocidad);
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
}
