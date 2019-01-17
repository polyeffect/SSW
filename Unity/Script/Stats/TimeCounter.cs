using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    Text text;
    float timer = 0.0f;
    int seconds;
    int maxCounter = 5;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        while (seconds < maxCounter)
        {
            timer += Time.deltaTime;
            seconds = (int)timer % 60;
            text.text = (maxCounter - seconds).ToString() + " sec.";
        }
    }
}
