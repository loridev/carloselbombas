using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad;
    private CharacterController controlador;
    public static string vista;
    //public float speed;
    // float velocityY = 0;
    // CharacterController controller;

    void Start()
    {
        // El jugador aparecerá en una esquina mirando hacia abajo
        // controller = GetComponent<CharacterController>();
        controlador = GetComponent<CharacterController>();
        // transform.Rotate(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
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

        /*
        if (Input.GetKey(KeyCode.W))
        {
            // transform.Rotate(0, 0, 0);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.position += Vector3.forward * speed * Time.deltaTime;
            transform.Rotate(0, -90, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // transform.Rotate(0, -1, 0);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.position += Vector3.left * speed * Time.deltaTime;
            transform.Rotate(180, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // transform.Rotate(0, 1, 0);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.position += Vector3.right * speed * Time.deltaTime;
            transform.Rotate(0, 0, 0);
        }
        */
    }
}