using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayLastScene : MonoBehaviour
{
    public Canvas canvas; // Reference to the Canvas component
    public VideoPlayer videoPlayer;


    //GameTimerText

    public TMP_Text gameTimerText;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
        

        // Access the timer from HoldTimeData
        
        gameTimerText.text = " " + HoldTimeData.instance.sharedValue.ToString("F2") + " sec";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
