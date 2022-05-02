using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudio : MonoBehaviour
{
    private void OnTriggerEnter(Collider colision)
    {
        if (colision.gameObject.tag == "Player")
        {
            BGSoundScript.NpcPlay();
        }
    }
}
