using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDownAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //powerUpAudio = GetComponent<AudioSource>();

    }
    void Update()
    {

    }
    public void OnTriggerEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            Debug.Log("Entra");
            BGSoundScript.PowerDownPlay();
        }
    }
}
