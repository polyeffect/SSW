using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPEventModule : MonoBehaviour
{
    private TraceText traceText;
    private ChapterControl chapterControl;
    private SceneController sceneController;
    private ImageSequencePlayback imageSequencePlayback;

    private int chapterNum = 2;

    private string data;
    private string prvData = "null";
    private string crtData = "null";

    public int maxCount = 3;
    private uint count = 0;

    void Start()
    {
        print("UDPEventModule Init!!!!");

        // trace text
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();

        // Chpater Controller
        chapterControl = GameObject.Find("Init").GetComponent<ChapterControl>();
        chapterNum = chapterControl.GetCrtChapter();

        // Chapter 2 Controller
        //imageSequencePlayback = GameObject.Find("Sequence Screen").GetComponent<ImageSequencePlayback>();

        // Chapter 3 COntroller
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    // UDP data from UDPReceive.cs
    public void UDPDataReceiver(string _data)
    {
        switch (chapterNum)
        {
            case 2:
                ChapterTwoControl(_data);
                break;
            case 3:
                ChapterThreeControl(_data);
                break;
        }
    }

    // Chapter2 Control
    private void ChapterTwoControl(string _data)
    {
        this.crtData = _data;       

        if (crtData != prvData)
        {
            // Send data to chapter2 sequence controller
            traceText.InputTraceText("Send data to Chapter 2.: " + _data);
            imageSequencePlayback.ReceiveData(_data);
        }

        this.prvData = _data;
    }

    // Chpater3 Control
    private void ChapterThreeControl(string _data)
    {
        this.crtData = _data;

        if (crtData == prvData)
        {
            count++;

            if (count == maxCount)
            {
                // Send data to chapter3 scene controller
                traceText.InputTraceText("Send data to Chpater 3.: " + _data);
                sceneController.ReceiveData(_data);
            }
        }
        else if (crtData != prvData)
        {
            count = 0;
        }

        //print("UDP data receiver: " + _data + ", " + "Receive Count: " + count);
        this.prvData = _data;
    }
}
