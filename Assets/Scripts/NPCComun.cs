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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isVertical)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        } else
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.Rotate(0, 180, 0, Space.Self);
        // speed = -speed;
    }
}
