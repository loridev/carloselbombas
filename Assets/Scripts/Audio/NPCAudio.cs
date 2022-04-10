using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudio : MonoBehaviour
{
    public AudioSource npcSource;
    // Start is called before the first frame update
    void Start()
    {
        npcSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            npcSource.Play();
            Debug.Log("warning");
        }
    }
}