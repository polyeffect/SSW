using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    float wfps = 100.0f;
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine("ResetWFPS");
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        if (fps < wfps) wfps = fps;
        text.text = string.Format("{0:0.0} ms ({1:0.0} fps / {2:0.0} wfps)", msec, fps, wfps);
    }

    IEnumerator ResetWFPS()
    {
        while(true)
        {
            yield return new WaitForSeconds(5.0f);
            wfps = 100.0f;
        }
    }
}
