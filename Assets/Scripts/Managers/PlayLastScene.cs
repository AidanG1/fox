using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
public class PlayLastScene : MonoBehaviour
{
    [Tooltip("Reference to the Canvas component")]
    public Canvas canvas;
    [Tooltip("Reference to the text component for the game timer")]
    public TMP_Text gameTimerText;
    [Tooltip("Reference to the video player component")]
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
        
        // Access the timer from HoldTimeData
        gameTimerText.text = " " + HoldTimeData.instance.sharedValue.ToString("F2") + " sec";
    }
}
