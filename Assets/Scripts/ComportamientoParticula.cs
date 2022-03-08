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

}
