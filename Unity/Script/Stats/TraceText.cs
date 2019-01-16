using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraceText : MonoBehaviour
{
    Text debugText;

    void Start()
    {
        debugText = this.GetComponent<Text>();
    }

    public void InputTraceText(string _text)
    {
        print(_text);
        debugText.text += _text + "\n";
    }
}
