using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCoin : MonoBehaviour
{
    AudioSource coinSource;
    // Start is called before the first frame update
    void Start()
    {
        coinSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            coinSource.Play();
        }
    }
}
//public class SawSoundController : MonoBehaviour
//{
   // public AudioClip saw;    // Add your Audi Clip Here;
                             // This Will Configure the  AudioSource Component; 
                             // MAke Sure You added AudioSouce to death Zone;
   // void Start()
   // {
       // GetComponent<AudioSource>().playOnAwake = false;
        //GetComponent<AudioSource>().clip = saw;
    //}

    //void OnCollisionEnter()  //Plays Sound Whenever collision detected
    //{
        //GetComponent<AudioSource>().Play();
    //}
    // Make sure that deathzone has a collider, box, or mesh.. ect..,
    // Make sure to turn "off" collider trigger for your deathzone Area;
    // Make sure That anything that collides into deathzone, is rigidbody;
//}