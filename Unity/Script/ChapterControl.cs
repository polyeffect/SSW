using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterControl : MonoBehaviour
{
    private TraceText traceText;
    private VideoEvent videoEvent;
    private ImageSequencePlayback imgSqcPlayback;

    public GameObject chapter1;
    public GameObject chapter2;
    public GameObject chapter3;

    private int prvChapterNum = 1;
    private int crtChapterNum = 3;

    void Start()
    {
        //StartCoroutine(ChapterInit());
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();
        //videoEvent = GameObject.Find("Video Player").GetComponent<VideoEvent>();
        //imgSqcPlayback = GameObject.Find("Sequence Screen").GetComponent<ImageSequencePlayback>();

        SetCrtChapter();
    }

    void Update()
    {
        
    }

    public void PrevChapter()
    {
        crtChapterNum--;
        if (crtChapterNum < 1) crtChapterNum = 3;
    }

    public void NextChapter()
    {
        crtChapterNum++;
        if (crtChapterNum > 3) crtChapterNum = 1;

        GameObject.Find("TraceText").SendMessage("InputTraceText", "Current chapter: " + crtChapterNum);

        if (crtChapterNum == 1)
        {
            ChapterOneStart();
        }
        else if (crtChapterNum == 2)
        {
            ChapterTwoStart();
        }
        else if (crtChapterNum == 3)
        {
            ChapterThreeStart();
        }
    }

    public int GetCrtChapter()
    {
        return crtChapterNum;
    }

    public void SetCrtChapter()
    {
        switch (crtChapterNum)
        {
            case 1 :
                break;
            case 2 :
                break;
            case 3 :
                ChapterThreeStart();
                break;
        }
    }

    private void SceneControl()
    {

    }

    /***************************
    * Each Chapter Starter Method
    ***************************/
    // Chapter1. Start
    private void ChapterOneStart()
    {
        // Chapter3 Fade Out & Deactivate
        GameObject.Find("Audio System").SendMessage("ChapterThreeBGMPlay");

        // Chapter1 Video activate & Video play
        //videoEvent.VideoPlayControl();

        // Chpater2 Activate (Ready for Chapter2 play)

    }

    // Chapter2. Start
    private void ChapterTwoStart()
    {
        // Chpater2 Image Sequence Play & BGM Play
        imgSqcPlayback.ResetFrame();
        GameObject.Find("Audio System").SendMessage("ChapterTwoBGMPlay");
    }

    // Chapter3. Start
    private void ChapterThreeStart()
    {
        // Chapter2 Fade out & Deactivate
        //imgSqcPlayback.SequencePlayControl();

        // Chapter3 BGM Play
        GameObject.Find("Audio System").SendMessage("ChapterThreeBGMPlay");
    }

    // Initializ All Chapter when Application start
    IEnumerator ChapterInit()
    {
        yield return new WaitForSeconds(5f);

        chapter1.SetActive(true);
        chapter2.SetActive(true);
        chapter3.SetActive(false);

        StopCoroutine(ChapterInit());
    }
}
