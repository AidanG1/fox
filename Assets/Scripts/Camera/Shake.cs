using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float duration = 0.5f;
    public AnimationCurve curve;
    public void ShakeScreen()
    {
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float strength = curve.Evaluate(timeElapsed / duration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0f, 0f, transform.position.z);
    }
}
