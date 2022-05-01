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
        allAudios[1].Play();
    }

    public static void PowerUpPlay()
    {
        allAudios[2].Play();
    }

    public static void CoinPlay()
    {
        allAudios[3].Play();
    }

    public static void PowerDownPlay()
    {
        allAudios[4].Play();
        Debug.Log(allAudios[4].clip.ToString());
    }

    void Update()
    {
        
    }
}
