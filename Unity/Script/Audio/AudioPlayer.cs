using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private TraceText traceText;
    private ChapterControl chapterControl;

    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;

    private float fadeTimer = 4f;

    // Chapter 2.
    public int maxLoopCount = 2;
    private int crtLoopCount = 0;
    private bool isPlay = false;

    // Chpater 3.

    void Start()
    {
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();
        chapterControl = GameObject.Find("Init").GetComponent<ChapterControl>();
    }

    void Update()
    {
        if(isPlay)
        {
            if (!audioSourceOne.isPlaying)
            {
                crtLoopCount++;
                traceText.InputTraceText("current BGM loop count: " + crtLoopCount);

                if (crtLoopCount < maxLoopCount)
                {
                    traceText.InputTraceText("Chapter2 BGM Play");
                    ChapterTwoBGMPlay();
                }
                else if (crtLoopCount >= maxLoopCount)
                {
                    traceText.InputTraceText("Chapter3 Play");
                    isPlay = false;
                    crtLoopCount = 0;
                    chapterControl.NextChapter();
                }
            }
        }
    }

    public void ChapterTwoBGMPlay()
    {
        if (!audioSourceOne.isPlaying)
        {
            StartCoroutine(AudioFadeController.FadeIn(audioSourceOne, fadeTimer));
            isPlay = true;
        }
        else
        {
            StartCoroutine(AudioFadeController.FadeOut(audioSourceOne, fadeTimer));
            isPlay = false;
        }
    }

    public void ChapterThreeBGMPlay()
    {
        if (!audioSourceTwo.isPlaying)
        {
            StartCoroutine(AudioFadeController.FadeIn(audioSourceTwo, fadeTimer));
        }
        else
        {
            StartCoroutine(AudioFadeController.FadeOut(audioSourceTwo, fadeTimer));
        }
    }



    public void SoundControl(int index)
    {
        if(index == 1)
        {
            if (!audioSourceOne.isPlaying)
            {
                audioSourceOne.Play();
            }
            else
            {
                audioSourceOne.Stop();
            }
        }

        if(index == 2)
        {
            if (!audioSourceTwo.isPlaying)
            {
                audioSourceTwo.Play();
            }
            else
            {
                audioSourceTwo.Stop();
            }
        }
        
    }
}
