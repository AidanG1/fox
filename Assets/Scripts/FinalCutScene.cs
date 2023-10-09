using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;


public class FinalCutScene : MonoBehaviour
{
    [Header("Final Cut Scene Properties")]
    [Tooltip("GameTimerManager component")]
    public GameTimerManager gameTimerManager;
    [Tooltip("Public to reference to the text component for the game timer")]
    
    private float timerValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger is entered by an object tagged as "Player"
        if (other.CompareTag("Player"))
        {

            timerValue = gameTimerManager.GetTimerValue();
            HoldTimeData.instance.sharedValue = timerValue;

            gameTimerManager.EndGame();

            SceneManager.LoadScene("FinalCutScene");
        }
    }
}