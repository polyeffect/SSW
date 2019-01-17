using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPEventModule : MonoBehaviour
{
    private TraceText traceText;
    private ChapterControl chapterControl;
    private SceneController sceneController;
    private ImageSequencePlayback imageSequencePlayback;

    private int chapterNum = 1;

    private string data;
    private string prvData = "null";
    private string crtData = "null";

    private string[] allSceneName = new string[] { "imuReverse", "imuIdle", "imuObverse" };

    public int maxCount = 5;
    private uint count = 0;

    void Start()
    {
        // trace text
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();

        // Chpater Controller
        chapterControl = GameObject.Find("Init").GetComponent<ChapterControl>();

        // Chapter 2 Controller
        imageSequencePlayback = GameObject.Find("Sequence Screen").GetComponent<ImageSequencePlayback>();

        // Chapter 3 COntroller
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void SetChapterNum(int _num)
    {
        chapterNum = _num;
    }

    // UDP data from UDPReceive.cs
    public void UDPDataReceiver(string _data)
    {
        if (chapterNum == 2)
        {
            if(_data == "imuReverse" || _data == "imuIdle" || _data == "imuObverse")
            {
                ChapterTwoControl(_data);
            }
        }
        else if (chapterNum == 3)
        {
            if (_data == "imuReverse" || _data == "imuIdle" || _data == "imuObverse")
            {
                
            }
            else
            {
                ChapterThreeControl(_data);
            }
        }
    }

    // Chapter2 Control
    private void ChapterTwoControl(string _data)
    {
        this.crtData = _data;       

        if (crtData != prvData)
        {
            // Send data to chapter2 sequence controller
            //traceText.InputTraceText("Send data to Chapter 2.: " + _data);
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
                //traceText.InputTraceText("Send data to Chpater 3.: " + _data);
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

    public void ResetCounter()
    {
        count = 0;
    }
}
