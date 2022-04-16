using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogin : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await ApiRequests.Login("", "");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
