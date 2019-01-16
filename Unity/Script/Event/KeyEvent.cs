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
         * Chpater Control
         ***************************/
        

        /***************************
         * Chapter1. Video Playback
         ***************************/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("Video Player").SendMessage("VideoPlayControl");
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
         * Chapter3. Sequence Interaction
         ***************************/

        // Scene
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameObject.Find("Scene Controller").SendMessage("ChangeScene");
            GameObject.Find("BG Alpha Mask").SendMessage("ChangeScene");
        }
        
        // Poem
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject.Find("Poem Screen").SendMessage("introMaskPlay");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Find("Poem Screen").SendMessage("ChangePoem");
            GameObject.Find("Poem Screen").SendMessage("introMaskPlay");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameObject.Find("Poem Screen").SendMessage("outroMaskPlay");
        }

        // Interaction
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            KeyPrintEvent("1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            KeyPrintEvent("2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            KeyPrintEvent("3");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            KeyPrintEvent("4");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            KeyPrintEvent("5");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            KeyPrintEvent("6");
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            KeyPrintEvent("7");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            KeyPrintEvent("8");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            KeyPrintEvent("9");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            KeyPrintEvent("0");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            KeyPrintEvent("Q");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            KeyPrintEvent("W");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            KeyPrintEvent("E");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            KeyPrintEvent("R");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            KeyPrintEvent("T");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            KeyPrintEvent("Y");
        }

        /***************************
         * Audio Playback
         ***************************/
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("Audio System").SendMessage("SoundControl", 1);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("Audio System").SendMessage("SoundControl", 2);
        }


        /***************************
         * UDP Socket Comm
         ***************************/
        if (Input.GetKeyDown(KeyCode.S))
        {
            KeyPrintEvent("S");
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
