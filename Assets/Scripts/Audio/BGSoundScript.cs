using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundScript : MonoBehaviour
{
    private static BGSoundScript instance = null;

    public static BGSoundScript Instance
    {
        get { return instance; }
    }

    public  static AudioSource[] allAudios;

    private void Start()
    {
        allAudios = GetComponentsInChildren<AudioSource>();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }


    public static void NpcPlay()
    {

        Debug.Log(allAudios[1].clip.ToString());
        if (allAudios[1] != null)
        {
            Debug.Log("PLAYY");
            allAudios[1].Play();
           
        }
        else
        {
            Debug.Log("NUUULL");
        }
    }

    public static void PowerUpPlay()
    {
        allAudios[2].Play();
    }

    void Update()
    {
        
    }
}
