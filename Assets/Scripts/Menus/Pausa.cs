using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausa : MonoBehaviour
{
    public GameObject labelPausa;
    
    public void AlternarPausa()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        labelPausa.SetActive(Time.timeScale == 0);
    }
}
