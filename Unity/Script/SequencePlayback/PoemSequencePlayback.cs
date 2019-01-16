using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemSequencePlayback : MonoBehaviour
{
    private TraceText tracetext;

    public float delayTime = 0.01f;
    public string sequencePath;
    public string introMaskPath;
    public string outroMaskPath;
    //public string imageSequenceName;
    public int poemNumber = 1;

    private float introDelayTime = 1f;
    private float poemIdleTime = 15f;

    // Poem
    private Texture texture;
    private Material goMaterial;
    

    // Alpha
    private Object[] introMaskObjects;
    private Object[] outroMaskObjects;
    private Texture[] introMaskTextures;
    private Texture[] outroMaskTextures;
    private int inFrameCounter = 0;
    private int outFrameCounter = 0;

    private string baseName;

    private bool isIntroPlay = false;
    private bool isOutroPlay = false;

    IEnumerator outroMaskAutoplay;

    void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
        this.baseName = "Sequence/Chapter3/Poem/Poem/";
    }

    void Start()
    {
        tracetext = GameObject.Find("Init").GetComponent<TraceText>();

        SetPoem();

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
            StartCoroutine("PlayIntroMask", delayTime);
            goMaterial.SetTexture("_Mask", introMaskTextures[inFrameCounter]);
        }

        if (isOutroPlay)
        {
            StartCoroutine("PlayOutroMask", delayTime);
            goMaterial.SetTexture("_Mask", outroMaskTextures[outFrameCounter]);
        }
    }

    public void SetPoem()
    {
        texture = (Texture)Resources.Load(baseName + poemNumber.ToString("D2") + "_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
    }

    /****************************
     * A method to set Mask play
     ***************************/
    private void ResetMaskPlay()
    {
        inFrameCounter = 0;
        outFrameCounter = 0;
        isIntroPlay = false;
        isOutroPlay = false;
        goMaterial.SetTexture("_Mask", introMaskTextures[inFrameCounter]);
    }

    public void IntroMaskPlay()
    {
        isIntroPlay = true;
        isOutroPlay = false;
        outroMaskAutoplay = OutroMaskAutoplay();
        StartCoroutine(outroMaskAutoplay);
    }

    public void OutroMaskPlay()
    {
        tracetext.InputTraceText("Poem Outro Mask Play!");
        isIntroPlay = false;
        isOutroPlay = true;
    }

    /****************************
     * Change Poem
     ***************************/
    public void ChangePoem(int _num)
    {
        poemNumber = _num;
        ResetMaskPlay();
        texture = (Texture)Resources.Load(baseName + poemNumber.ToString("D2") + "_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
        Invoke("IntroMaskPlay", introDelayTime);
        StopAllCoroutines();
    }

    IEnumerator OutroMaskAutoplay()
    {
        yield return new WaitForSeconds(poemIdleTime);
        OutroMaskPlay();
    }

    /****************************
     * Sequence play method
     ***************************/
    // A method to play the sequence intro alpha mask just once
    IEnumerator PlayIntroMask()
    {
        yield return new WaitForSeconds(delayTime);
        if (inFrameCounter < introMaskTextures.Length - 1)
        {
            ++inFrameCounter;
        }
        StopCoroutine("PlayIntroMask");
    }

    // A method to play the sequence outro alpha mask just once
    IEnumerator PlayOutroMask()
    {
        yield return new WaitForSeconds(delayTime);
        if (outFrameCounter < introMaskTextures.Length - 1)
        {
            ++outFrameCounter;
        }
        StopCoroutine("PlayOutroMask");
    }
}
