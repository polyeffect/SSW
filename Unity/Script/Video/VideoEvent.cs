using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoEvent : MonoBehaviour
{
    private ChapterControl chapterControl;

    public VideoPlayer vp;
    public GameObject videoScreen;
    public RenderTexture renderTexture;

    private TraceText traceText;

    // fade and destroy
    bool isPlay = true;
    float alpha = 1.0f;
    Renderer renderer;
    Color currColor;

    void Start()
    {
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();
        chapterControl = GameObject.Find("Init").GetComponent<ChapterControl>();

        // Detect end of video clip
        vp.loopPointReached += DetectVideoEnd;

        renderer = videoScreen.GetComponent<Renderer>();
        currColor = renderer.material.color;
    }

    void Update()
    {
        
    }

    public void VideoPlayControl()
    {
        if (isPlay)
        {
            isPlay = !isPlay;
            traceText.InputTraceText("Video fade out and Destroy");
            StartCoroutine(FadeOutAndDeactivate());
            chapterControl.NextChapter();
        }
        else
        {
            isPlay = !isPlay;
            traceText.InputTraceText("Video fade in and Actiate");
            VideoReplay();
        }
    }


    /****************************
     * Fade in & Fade out
     ***************************/
    // Fade out and Deactivate
    IEnumerator FadeOutAndDeactivate()
    {
        while (alpha > 0.0f)
        {
            alpha = alpha - Time.deltaTime;
            currColor.a = alpha;
            renderer.material.color = currColor;

            yield return null;
        }

        vp.Stop();
        videoScreen.SetActive(false);
    }

    // Fade in and Activate
    IEnumerator FadeInAndActivate()
    {
        yield return new WaitForSeconds(0.5f);

        while (alpha < 1.0f)
        {
            alpha = alpha + Time.deltaTime;
            currColor.a = alpha;
            renderer.material.color = currColor;

            yield return null;
        }
    }

    // Play Video clip again
    private void VideoReplay()
    {
        videoScreen.SetActive(true);
        vp.frame = 0;
        vp.Play();
        StartCoroutine(FadeInAndActivate());
    }

    // Dectect end of video clip
    void DetectVideoEnd(UnityEngine.Video.VideoPlayer vp)
    {
        isPlay = false;
        traceText.InputTraceText("Video is Over.");
        StartCoroutine(FadeOutAndDeactivate());
        chapterControl.NextChapter();
    }
}
