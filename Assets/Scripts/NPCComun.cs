using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCComun : MonoBehaviour
{
    public float speed = .0f;
    public bool isVertical;

    // Start is called before the first frame update
    void Start()
    {
        if (!isVertical)
        {
            transform.Rotate(0, 90, 0, Space.Self);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "ground" && !other.tag.StartsWith("PU") && !other.tag.StartsWith("PD")) transform.Rotate(0, 180, 0, Space.Self);
        // speed = -speed;
    }
}
