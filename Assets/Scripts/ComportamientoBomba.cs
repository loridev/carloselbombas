using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoBomba : MonoBehaviour
{
    public bool explotar;
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
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (other.tag == "Player")
        {
            other.transform.position = new Vector3(other.transform.position.x, 1, other.transform.position.z);
        }
    }
}
