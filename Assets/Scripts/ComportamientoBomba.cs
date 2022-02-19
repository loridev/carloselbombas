using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoBomba : MonoBehaviour
{

    private void Start()
    {
        // AstarPath.active.Scan();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
