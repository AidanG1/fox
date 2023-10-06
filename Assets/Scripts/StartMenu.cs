using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text gameText;
    public Vector3 originalGameTextPosition;
    public float startingGameTextPositionY = 100f;
    public float gameTextMovementTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // move the game text to starting position
        gameText.transform.position = new Vector3(
            gameText.transform.position.x,
            startingGameTextPositionY,
            gameText.transform.position.z
        );
        // StartCoroutine(ShakeText());
    }

    void Awake()
    {
        originalGameTextPosition = gameText.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // move the game text to original position using pingpong
        gameText.transform.position = new Vector3(
            gameText.transform.position.x,
            Mathf.PingPong(Time.time, gameTextMovementTime) + originalGameTextPosition.y,
            gameText.transform.position.z
        );

    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelsScene");
    }

    IEnumerator ShakeText()
    {
        Vector3 originalPosition = gameText.transform.position;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.1f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPosition.y + Random.Range(-1f, 1f) * shakeMagnitude;

            gameText.transform.position = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        gameText.transform.position = originalPosition;
    }
}
