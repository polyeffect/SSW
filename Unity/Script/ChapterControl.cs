using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterControl : MonoBehaviour
{
    private TraceText traceText;
    private UDPEventModule udpEventModule;

    private VideoEvent videoEvent; // Chapter1 Video Playback
    private GameObject sequenceScreen; // Chapter2 Sequence Screen (just screen)
    private ImageSequencePlayback imgSqcPlayback; // Chapter2 Sequence Playback
    private SceneController sceneController; // Chapter3 Poem Playback (Scene Controller)

    public GameObject initScene;
    public GameObject chapter1;
    public GameObject chapter2;
    public GameObject chapter3;

    private int crtChapterNum = 1;

    private bool isInit = false;

    void Start()
    {
        
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();
        udpEventModule = GameObject.Find("UDPComm").GetComponent<UDPEventModule>();

        // All Chapters Playback initialize
        videoEvent = GameObject.Find("Video Player").GetComponent<VideoEvent>();
        sequenceScreen = GameObject.Find("Sequence Screen");
        imgSqcPlayback = GameObject.Find("Sequence Screen").GetComponent<ImageSequencePlayback>();
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();

        StartCoroutine(ChapterInit());
    }

    void Update()
    {
        
    }

    public void NextChapter()
    {
        crtChapterNum++;
        if (crtChapterNum > 3) crtChapterNum = 1;

        SetCrtChapter(crtChapterNum);
    }

    public int GetCrtChapter()
    {
        return crtChapterNum;
    }

    public void SetCrtChapter(int _num)
    {
        switch (_num)
        {
            case 1 :
                ChapterOneStart();
                UDPEventSetChapterNum(_num);
                break;
            case 2 :
                ChapterTwoStart();
                UDPEventSetChapterNum(_num);
                break;
            case 3 :
                ChapterThreeStart();
                UDPEventSetChapterNum(_num);
                break;
        }
    }

    private void UDPEventSetChapterNum(int _num)
    {
        udpEventModule.SetChapterNum(_num);
    }

    /***************************
    * Each Chapter Starter Method
    ***************************/
    // Chapter1. Start
    private void ChapterOneStart()
    {
        // Chapter3 Fade Out & Deactivate
        if (isInit) GameObject.Find("Audio System").SendMessage("ChapterThreeBGMPlay");

        // Chapter1 Video Activate & Video Play
        videoEvent.VideoPlayControl();

        // Chpater2 Activate (Ready for Chapter2 play)
        sequenceScreen.SetActive(true);
        imgSqcPlayback.FadeInAndActivate();

        isInit = true;
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
        // About Chapter2.
        // Chapter2 Fade out & Deactivate
        imgSqcPlayback.SequenceFadeOutAndDeactivate();

        // Chapter2 BGM Stop
        //GameObject.Find("Audio System").SendMessage("ChapterTwoBGMPlay");


        // About Chapter3.
        // Chapter3 Activate
        chapter3.SetActive(true);

        // Chapter3 Initialize
        sceneController.InitScene();
        udpEventModule.ResetCounter();

        // Chapter3 BGM Play
        GameObject.Find("Audio System").SendMessage("ChapterThreeBGMPlay");
    }

    // Initializ All Chapter when Application start
    IEnumerator ChapterInit()
    {
        yield return new WaitForSeconds(5f);

        initScene.SetActive(false);
        chapter1.SetActive(true);
        chapter2.SetActive(true);
        chapter3.SetActive(false);

        SetCrtChapter(crtChapterNum);

        StopCoroutine(ChapterInit());
    }
}
