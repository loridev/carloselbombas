using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;

    private void Awake()
    {
        GameCanvas.SetActive(true);
    }

    public void SpawnPlayer()
    {
        GameCanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
