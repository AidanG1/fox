using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTimeData : MonoBehaviour
{
    public static HoldTimeData instance;
    // Function for sharing data between Game and FinalCutScene
    public float sharedValue;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
