using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI counterText;
    private float waitTime = .1f;
    private Coroutine fpsCr;

    private int fps;

    private void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();

        StartFPSCounter();
    }

    private void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
    }

    private void StartFPSCounter()
    {
        fpsCr = StartCoroutine(FPSCr());
    }

    IEnumerator FPSCr()
    {
        yield return new WaitForSeconds(waitTime);
        counterText.text = "FPS: " + fps;

        fpsCr = StartCoroutine(FPSCr());
    }

    private void OnDestroy()
    {
        StopCoroutine(fpsCr);
    }
}
