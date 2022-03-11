using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoParticula : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EsperarCollider());
        //StartCoroutine(EsperarDestruccion());
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EsperarCollider()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(GetComponent<BoxCollider>());
        
    }

    private IEnumerator EsperarDestruccion()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(GetComponent<BoxCollider>());
        switch (other.tag)
        {
            case "Player":
                --other.gameObject.GetComponent<ComportamientoCarlos>().vidas;
                break;
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
