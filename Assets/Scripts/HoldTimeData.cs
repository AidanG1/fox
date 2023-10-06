using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTimeData : MonoBehaviour
{
    public static HoldTimeData instance;

    // Add any data you want to share between scenes here
    public float sharedValue;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
