using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fox")) // Check if the FOX collided with the trigger
        {
            Debug.Log("yes");
            // Move the main camera to the position of this GameObject
            Camera.main.transform.position = transform.position;
        }
    }
}
