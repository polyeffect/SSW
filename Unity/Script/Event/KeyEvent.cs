using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour
{
    private string keyPrint = " key pressed.";

    void Start()
    {
        
    }

    void Update()
    {
        /***************************
         * Debug
         ***************************/
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SendMessage("StatsControl");
        }

        /***************************
         * Chapter2. Sequence Playback
         ***************************/
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            KeyPrintEvent("Comma");
            GameObject.Find("Sequence Screen").SendMessage("SequencePlayback", true);
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            KeyPrintEvent("Period");
            GameObject.Find("Sequence Screen").SendMessage("SequencePlayback", false);
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            KeyPrintEvent("Slash");
            GameObject.Find("Sequence Screen").SendMessage("SequencePlayback", false);
        }

        /***************************
         * Quit Application
         ***************************/
        if (Input.GetKey("escape"))
        {
            KeyPrintEvent("ESC");
            Application.Quit();
        }
    }

    void KeyPrintEvent(string key)
    {
        print(key + keyPrint);
    }
}
