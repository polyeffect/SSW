using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour
{
    private SceneController sceneConroller;

    private string keyPrint = " key pressed.";

    void Start()
    {
        sceneConroller = GameObject.Find("Scene Controller").GetComponent<SceneController>();
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
         * Chapter3. Interaction
         ***************************/
        string input = Input.inputString;
        
        switch (input)
        {
            case "1":
                sceneConroller.ReceiveData("i01");
                break;
            case "2":
                sceneConroller.ReceiveData("i02");
                break;
            case "3":
                sceneConroller.ReceiveData("i03");
                break;
            case "4":
                sceneConroller.ReceiveData("i04");
                break;
            case "5":
                sceneConroller.ReceiveData("i05");
                break;
            case "6":
                sceneConroller.ReceiveData("i06");
                break;
            case "7":
                sceneConroller.ReceiveData("i07");
                break;
            case "8":
                sceneConroller.ReceiveData("i08");
                break;
            case "9":
                sceneConroller.ReceiveData("i09");
                break;
            case "0":
                sceneConroller.ReceiveData("i10");
                break;
            case "-":
                sceneConroller.ReceiveData("i16");
                break;
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
