using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    // public float velocidad;
    // public CharacterController controlador;
    public float speed;
    float velocityY = 0;
    CharacterController controller;

    void Start()
    {
        // El jugador aparecerá en una esquina mirando hacia abajo
        controller = GetComponent<CharacterController>();
        transform.Rotate(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical");
        // Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        // if (direccion.magnitude >= 0.1f)
        // {
        // controlador.Move(direccion * velocidad * Time.deltaTime);
        // }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(0, 0, 0);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

    }
}
