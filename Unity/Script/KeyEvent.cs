using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour
{
    private string keyPrint = " key pressed.";

    public GameObject videoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KeyPrintEvent("space");
        }

        /***************************
         * Video Playback
         ***************************/
        if (Input.GetKeyDown(KeyCode.P))
        {
            videoPlayer.SetActive(true);
            GameObject.Find("VideoPlayback").SendMessage("SetAlpha", 1.0f);
        }


        /***************************
         * Scene Control
         ***************************/
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            KeyPrintEvent("[");
            SendMessage("PrevScene");
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            KeyPrintEvent("]");
            SendMessage("NextScene");
        }

        /***************************
         * Sequence Playback
         ***************************/
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            KeyPrintEvent("Comma");
            GameObject.Find("Quad").SendMessage("SequencePlayback", true);
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            KeyPrintEvent("Period");
            GameObject.Find("Quad").SendMessage("SequencePlayback", false);
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            KeyPrintEvent("Slash");
            SendMessage("SequencePlayback", false);
        }

        /***************************
         * UDP Socket Comm
         ***************************/
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendMessage("SendSocketData");
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
