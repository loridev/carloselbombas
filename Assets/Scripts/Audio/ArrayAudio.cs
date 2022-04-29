using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayAudio : MonoBehaviour
{
    public static AudioSource[] audios;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponentsInChildren<AudioSource>();
        Debug.Log(audios[2].clip.ToString());
        NpcPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void NpcPlay()
    {
        Debug.Log(audios[2].clip.ToString());
        if (audios[2] != null)
        {
            Debug.Log("PLAYY");
            audios[2].Play();
        } else
        {
            Debug.Log("NUUULL");
        }
    }
}
