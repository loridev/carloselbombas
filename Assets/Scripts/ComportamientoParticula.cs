using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                if (other.gameObject.GetComponent<ComportamientoCarlos>().restarVidas)
                {
                    StartCoroutine(EsperarVidas(other));
                    --other.gameObject.GetComponent<ComportamientoCarlos>().vidas;
                }
                if (other.gameObject.GetComponent<ComportamientoCarlos>().vidas <= 0)
                {
                    SceneManager.LoadScene("MenuMundos");
                }
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

    public IEnumerator EsperarVidas(Collider other)
    {
        //TODO: LLAMAR A FUNCION DE CARLOS QUE RESTE VIDA
        other.gameObject.GetComponent<ComportamientoCarlos>().restarVidas = false;
        yield return new WaitForSeconds(2);
        other.gameObject.GetComponent<ComportamientoCarlos>().restarVidas = true;
        Debug.Log("Deberia ser true");
    }


}
