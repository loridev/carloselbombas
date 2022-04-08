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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collider colision)
    {
        if (colision.gameObject.tag == "PUfinalNivel")
        {
            coinSource.Play();
        }
    }
}
