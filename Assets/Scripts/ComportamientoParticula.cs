using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoParticula : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EsperarCollider());
        StartCoroutine(EsperarDestruccion());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EsperarCollider()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(GetComponent<BoxCollider>());
        
    }

    private IEnumerator EsperarDestruccion()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
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
                other.gameObject.GetComponent<ComportamientoBomba>().explotar = false;
                carlitos.GetComponent<SoltarBombas>().ExplosionBomba(other.gameObject.transform, carlitos.GetComponent<ComportamientoCarlos>().alcanceBomba);
                break;

        }
    }

}
