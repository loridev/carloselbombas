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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "cNpc":
            case "fNpc":
                Destroy(other.gameObject);
                break;
            case "Bomba":
                ComportamientoBomba cb = other.GetComponent<ComportamientoBomba>();
                if (!cb.explotada)
                {
                    cb.ExplosionBomba(cb.owner.alcanceBomba);
                }
                //Destroy(other.gameObject);
                break;

        }
    }


}
