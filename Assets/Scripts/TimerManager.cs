using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    // GUI text for timer display
    public TMP_Text timerText;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;
        // Display the timer in the GUI text
        timerText.text = timer.ToString("F2");
    }
}
