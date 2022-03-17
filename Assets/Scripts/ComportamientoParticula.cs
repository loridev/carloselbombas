using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComportamientoParticula : MonoBehaviour
{
    private List<Transform> colliderEnters;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GetComponent<BoxCollider>(), 0.1f);
        //StartCoroutine(EsperarDestruccion());
        Destroy(gameObject, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "cNpc":
            case "fNpc":
                Destroy(other.gameObject);
                break;
            case "Bomba":
                GameObject carlitos = GameObject.FindGameObjectWithTag("Player");
                // other.gameObject.GetComponent<ComportamientoBomba>().explotar = false;
                other.gameObject.GetComponent<ComportamientoBomba>().ExplosionBomba(carlitos.GetComponent<ComportamientoCarlos>().alcanceBomba);
                Destroy(other.gameObject);
                break;
            case "Particula":
                Destroy(gameObject);
                break;

        }
    }


}
