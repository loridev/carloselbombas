using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAudio : MonoBehaviour
{
    public void OnTriggerEnter(Collider colision)
    {
        if(colision.gameObject.tag == "Player")
        {
            BGSoundScript.PowerUpPlay();
        }
    }
}