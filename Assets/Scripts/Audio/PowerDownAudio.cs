using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDownAudio : MonoBehaviour
{
    public void OnTriggerEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            Debug.Log("Entra POWERDOWN");
            BGSoundScript.PowerDownPlay();
        }
    }
}
