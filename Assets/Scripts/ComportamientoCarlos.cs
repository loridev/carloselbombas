using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void GestionarTrigger(string tag)
    {
        Debug.Log(tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        GestionarTrigger(other.tag);
    }
}
