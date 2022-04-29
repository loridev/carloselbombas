using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAudio : MonoBehaviour
{

    public AudioSource powerUpAudio;
     void Start()
    {
        powerUpAudio = GetComponent<AudioSource>();
        
    }
     void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        { 
            powerUpAudio.enabled = true;
            powerUpAudio.gameObject.SetActive(true);
            powerUpAudio.Play();
        }
    }

}