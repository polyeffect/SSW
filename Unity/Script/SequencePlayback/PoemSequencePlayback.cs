using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemSequencePlayback : MonoBehaviour
{
    public float delayTime = 0.005f; // 45fps

    // Poem
    private string crtPoemState = "idle";
    private int crtPoemNumber = 1;
    private int nxtPoemNumber = 1;

    private float introDelayTime = 0.1f;
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
            StartCoroutine("PlayIntroMask", delayTime);
            goMaterial.SetTexture("_Mask", introMaskTextures[inFrameCounter]);
        }

        if (isOutroPlay)
        {
            StartCoroutine("PlayOutroMask", delayTime);
            goMaterial.SetTexture("_Mask", outroMaskTextures[outFrameCounter]);
        }
    }

    public void SetCurrentPoem()
    {
        texture = (Texture)Resources.Load(baseName + crtPoemNumber.ToString("D2") + "_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
    }

    public void SetNextPoem()
    {

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
        Invoke("OutroMaskPlay", 15.0f);
    }

    public void OutroMaskPlay()
    {
        crtPoemState = "out";

        isIntroPlay = false;
        isOutroPlay = true;
    }

    /****************************
     * Change Poem
     ***************************/
    public void SetNextPoem(int _num)
    {
        nxtPoemNumber = _num;
        print("Current poem state: " + crtPoemState);
        if (crtPoemState == "in")
        {
            CancelInvoke("OutroMaskPlay");
            isDirectOutro = true;
        } else if (crtPoemState == "idle")
        {
            isDirectOutro = true;
            CancelInvoke("OutroMaskPlay");
            OutroMaskPlay();
        } else if (crtPoemState == "out")
        {

        } else if (crtPoemState == "end")
        {
            ChangePoem(nxtPoemNumber);
        }
    }

    public void ChangePoem(int _num)
    {
        crtPoemState = "in";
        isDirectOutro = false;
        crtPoemNumber = _num;
        ResetMaskPlay();
        texture = (Texture)Resources.Load(baseName + crtPoemNumber.ToString("D2") + "_00000", typeof(Texture));
        goMaterial.mainTexture = this.texture;
        Invoke("IntroMaskPlay", introDelayTime);
        StopAllCoroutines();
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
        if (inFrameCounter == introMaskTextures.Length - 1  && !isDirectOutro)
        {
            crtPoemState = "idle";
        } else if (inFrameCounter == introMaskTextures.Length - 1 && isDirectOutro)
        {
            OutroMaskPlay();
        }
        StopCoroutine("PlayIntroMask");
    }

    // A method to play the sequence outro alpha mask just once
    IEnumerator PlayOutroMask()
    {
        yield return new WaitForSeconds(delayTime);
        if (outFrameCounter < outroMaskTextures.Length - 1)
        {
            ++outFrameCounter;
        }
        if (outFrameCounter == outroMaskTextures.Length - 1 && !isDirectOutro)
        {
            crtPoemState = "end";
        }
        if (outFrameCounter == outroMaskTextures.Length - 1 && isDirectOutro)
        {
            isDirectOutro = false;
            ChangePoem(nxtPoemNumber);
        }
        StopCoroutine("PlayOutroMask");
    }
}
