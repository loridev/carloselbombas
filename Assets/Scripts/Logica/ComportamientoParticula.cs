using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComportamientoParticula : MonoBehaviour
{
    private List<Transform> colliderEnters;

    void Start()
    {
        Destroy(GetComponent<BoxCollider>(), 0.1f);
        Destroy(gameObject, 1);

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "cNpc":
            case "fNpc":
                BGSoundScript.DieNpcPlay();
                Destroy(other.gameObject);
                break;
            case "Bomba":
                ComportamientoBomba cb = other.GetComponent<ComportamientoBomba>();
                if (cb != null && !cb.explotada)
                {
                    cb.ExplosionBomba(cb.owner.alcanceBomba);
                }
                break;

        }
    }
}
