using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject music = GameObject.Find("Audio");

        //music.GetComponent<AudioSource>().Pause();

        BGSoundScript.BackMusicPause();
    }

}
