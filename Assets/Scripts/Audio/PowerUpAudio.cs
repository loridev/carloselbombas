using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAudio : MonoBehaviour
{
    public AudioSource powerUpSource;
    // Start is called before the first frame update
    void Start()
    {
        powerUpSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            powerUpSource.Play();
            Debug.Log("entra");
        }
    }
}