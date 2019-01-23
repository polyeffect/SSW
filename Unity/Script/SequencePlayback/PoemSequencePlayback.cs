using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemSequencePlayback : MonoBehaviour
{
    private float introDelayTime = 0.01f; // 30fps
    private float outroDelayTime = 0.01f; // 30fps

    // Poem
    private string crtPoemState = "idle";
    private int crtPoemNumber = 2;
    private int nxtPoemNumber = 2;

    private float introFreezeTime = 0.05f;
    private float poemIdleTime = 15f;

    private string baseName;
    private Texture texture;
    private Material goMaterial;

    // Alpha
    public string introMaskPath;
    public string outroMaskPath;
    private Object[] introMaskObjects;
    private Object[] outroMaskObjects;
    private Texture[] introMaskTextures;
    private Texture[] outroMaskTextures;
    private int inFrameCounter = 0;
    private int outFrameCounter = 0;

    private bool isIntroPlay = false;
    private bool isOutroPlay = false;
    private bool isDirectOutro = false;

    IEnumerator outroMaskAutoplay;

    void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
        this.baseName = "Sequence/Chapter3/Poem/Poem/";
    }

    void Start()
    {
        SetCurrentPoem();

        // Intro Mask
        this.introMaskObjects = Resources.LoadAll("Sequence/Chapter3/Poem/" + introMaskPath, typeof(Texture));
        this.introMaskTextures = new Texture[introMaskObjects.Length];
        for (int i = 0; i < introMaskObjects.Length; i++)
        {
            this.introMaskTextures[i] = (Texture)this.introMaskObjects[i];
        }

        // Outro Mask
        this.outroMaskObjects = Resources.LoadAll("Sequence/Chapter3/Poem/" + outroMaskPath, typeof(Texture));
        this.outroMaskTextures = new Texture[outroMaskObjects.Length];
        for (int i = 0; i < outroMaskObjects.Length; i++)
        {
            this.outroMaskTextures[i] = (Texture)this.outroMaskObjects[i];
        }
    }

    void Update()
    {
        if (isIntroPlay)
        {
            StartCoroutine("PlayIntroMask", introDelayTime);
            goMaterial.SetTexture("_Mask", introMaskTextures[inFrameCounter]);
        }

        if (isOutroPlay)
        {
            StartCoroutine("PlayOutroMask", outroDelayTime);
            goMaterial.SetTexture("_Mask", outroMaskTextures[outFrameCounter]);
        }
    }

    public void SetCurrentPoem()
    {
        texture = (Texture)Resources.Load(baseName + crtPoemNumber.ToString("D2") + "_text_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
    }

    /****************************
     * A method to set Mask play
     ***************************/
    private void ResetMaskPlay()
    {
        inFrameCounter = 20;
        outFrameCounter = 20;
        isIntroPlay = false;
        isOutroPlay = false;
        goMaterial.SetTexture("_Mask", introMaskTextures[inFrameCounter]);
    }

    public void IntroMaskPlay()
    {
        print("IntroMaskPlay");
        isIntroPlay = true;
        isOutroPlay = false;
        Invoke("OutroMaskPlayNormal", 17.0f);
    }

    public void OutroMaskPlayNormal()
    {
        print("OutroMaskPlayNormal");
        SetCurrentPoemState("out");
        outroDelayTime = 0.01f;
        isIntroPlay = false;
        isOutroPlay = true;
    }

    private void OutroMaskPlayFater()
    {
        print("OutroMaskPlayFater");
        SetCurrentPoemState("out");
        outroDelayTime = 0.005f;
        isIntroPlay = false;
        isOutroPlay = true;
    }

    /****************************
     * Change Poem
     ***************************/
    public void SetNextPoemNum(int _num)
    {
        nxtPoemNumber = _num;
        print("Current poem state: " + crtPoemState);

        introFreezeTime = 0.05f;

        switch (crtPoemState)
        {
            case "in":
                isDirectOutro = true;
                CancelInvoke("OutroMaskPlayNormal");
                break;
            case "idle":
                isDirectOutro = true;
                CancelInvoke("OutroMaskPlayNormal");
                OutroMaskPlayFater();
                break;
            case "out":
                break;
            case "end":
                introFreezeTime = 3f;
                ChangePoem(nxtPoemNumber);
                break;
        }
    }

    private void SetCurrentPoemState(string _state)
    {
        crtPoemState = _state;

        switch (crtPoemState)
        {
            case "in":
                isDirectOutro = false;
                break;
            case "idle":
                
                break;
            case "out":
                break;
            case "end":
                break;
        }
    }

    private void ChangePoem(int _num)
    {
        SetCurrentPoemState("in");
        
        crtPoemNumber = _num;
        ResetMaskPlay();
        texture = (Texture)Resources.Load(baseName + crtPoemNumber.ToString("D2") + "_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
        Invoke("IntroMaskPlay", introFreezeTime);
        StopAllCoroutines();
    }

    
    /****************************
     * Sequence play method
     ***************************/
    // A method to play the sequence intro alpha mask just once
    IEnumerator PlayIntroMask(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (inFrameCounter < introMaskTextures.Length - 1)
        {
            ++inFrameCounter;
        }
        if (inFrameCounter == introMaskTextures.Length - 1  && !isDirectOutro)
        {
            SetCurrentPoemState("idle");
        } else if (inFrameCounter == introMaskTextures.Length - 1 && isDirectOutro)
        {
            OutroMaskPlayFater();
        }
        StopCoroutine("PlayIntroMask");
    }

    // A method to play the sequence outro alpha mask just once
    IEnumerator PlayOutroMask(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (outFrameCounter < outroMaskTextures.Length - 1)
        {
            ++outFrameCounter;
        }
        if (outFrameCounter == outroMaskTextures.Length - 1 && !isDirectOutro)
        {
            SetCurrentPoemState("end");
        }
        if (outFrameCounter == outroMaskTextures.Length - 1 && isDirectOutro)
        {
            //isDirectOutro = false;
            ChangePoem(nxtPoemNumber);
        }
        StopCoroutine("PlayOutroMask");
    }
}
