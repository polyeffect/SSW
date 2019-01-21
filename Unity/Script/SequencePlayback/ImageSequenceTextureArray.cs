using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequenceTextureArray : MonoBehaviour
{
    private TraceText traceText;
    private ChapterControl chapterControl;

    public string sequencePath;
    public float delayTime = 0.01f;

    private bool isPlay = false;
    private Object[] objects;
    private Texture[] textures;
    private Material goMaterial;
    private int frameCounter = 0;
    public Texture alphaTexture;

    private int loopCounter = 0;
    private int maxLoopCounter = 2;
    private bool isLoopCount = false;

    private void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
    }

    void Start()
    {
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();
        chapterControl = GameObject.Find("Init").GetComponent<ChapterControl>();

        this.objects = Resources.LoadAll("Sequence/Chapter3/BG/" + sequencePath, typeof(Texture));
        this.textures = new Texture[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            this.textures[i] = (Texture)this.objects[i];
        }
    }

    void Update()
    {
        if (isPlay) StartCoroutine("PlayLoop", delayTime);
        goMaterial.mainTexture = textures[frameCounter];
    }

    public void ResetFrame()
    {
        //traceText.InputTraceText("Front Scene Reset Frame: " + this.gameObject.name);
        frameCounter = 0;
        isPlay = true; // Start Sequence Playing
        isLoopCount = true;
        loopCounter = 0;
    }

    public void SetAlphaTexture()
    {
        goMaterial.SetTexture("_Mask", alphaTexture);
    }

    public void StopPlayingSequence()
    {
        isPlay = false; // Stop Sequence Playing
    }

    public void StopLoopCounter()
    {
        //traceText.InputTraceText("Stop Loop Counter: " + this.gameObject.name);
        isLoopCount = false;
        loopCounter = 0;
    }

    public void ResetLoopCounter()
    {
        loopCounter = -1;
    }

    public void GotoNextChapter()
    {
        //traceText.InputTraceText("Go Chapter1 from 3");
        isLoopCount = false;
        loopCounter = 0;
        chapterControl.NextChapter();
    }

    /****************************
     * Sequence play method
     ***************************/
    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        frameCounter = (++frameCounter) % textures.Length;
        if (frameCounter == textures.Length - 1 && isLoopCount)
        {
            loopCounter++;
            traceText.InputTraceText("Chapter3 Scene Loop Counter: " + loopCounter + ", " + this.gameObject.name);
            if (loopCounter >= maxLoopCounter) GotoNextChapter();
        }
        StopCoroutine("PlayLoop");
    }

    // A methos to play th sequence just once
    IEnumerator PlayOnce(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(frameCounter < textures.Length - 1)
        {
            ++frameCounter;
        }
        StopCoroutine("PlayOnce");
    }
}
