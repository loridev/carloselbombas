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

    public static void BackMusicPlay()
    {
        allAudios[0].Play();
    }

    public static void BackMusicPause()
    {
        allAudios[0].Stop();
    }
    public static void LoseLifePlay()
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
    }

    public static void DeathCarlosPlay()
    {
        allAudios[5].Play();
    }

    public static void DestroyedBoxPlay()
    {
        allAudios[6].Play();
    }

    public static void DieNpcPlay()
    {
        allAudios[7].Play();
    }

    public static void NachetePlay()
    {
        allAudios[8].Play();
    }

    public static void NacheteStop()
    {
        allAudios[8].Stop();
    }

    //TODO: Sonido CARLOS EL BOMBAS en el countdown de multi
}
