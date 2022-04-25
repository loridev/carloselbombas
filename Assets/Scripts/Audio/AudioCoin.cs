using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCoin : MonoBehaviour
{
    public AudioSource coinSource;
    // Start is called before the first frame update
    void Start()
    {
        coinSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            coinSource.Play();
        }
    }
}
