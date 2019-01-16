using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequencePlayback : MonoBehaviour
{
    private TraceText traceText;

    Renderer renderer;
    Color currColor;
    float alpha = 1.0f;

    public string sequencePath;

    //public float delayTime = 0.01f;
    public float easing = 0.02f;
    private float targetSpeed = 0.03f;
    private float currentSpeed = 0.03f;

    private bool isPlaying = false;
    private bool isRevers = false;
    private bool speedRevers = false;

    private Object[] objects;
    private Texture[] textures;
    private Material goMaterial;
    private int frameCounter = 0;
    private int maxFrameCounter = 0;

    private string data;

    private void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
    }

    void Start()
    {
        traceText = GameObject.Find("TraceText").GetComponent<TraceText>();

        this.objects = Resources.LoadAll("Sequence/" + sequencePath, typeof(Texture));
        this.textures = new Texture[objects.Length];

        this.renderer = this.GetComponent<Renderer>();
        currColor = renderer.material.color;

        maxFrameCounter = objects.Length;

        for (int i = 0; i < objects.Length; i++)
        {
            this.textures[i] = (Texture)this.objects[i];
        }
    }

    void Update()
    {
        if (isPlaying) {
            float delaySpeed = targetSpeed - currentSpeed;
            currentSpeed += delaySpeed * easing;
            //print(currentSpeed + ", " + targetSpeed);

            if (currentSpeed >= targetSpeed - 0.001f)
            {
                targetSpeed = 0.03f;
                if (isRevers) speedRevers = true;
                else speedRevers = false;
            }

            if (speedRevers)
            {
                StartCoroutine("PlayReverse", currentSpeed);
            }
            else if (!speedRevers)
            {
                StartCoroutine("PlayLoop", currentSpeed);
            }

            goMaterial.mainTexture = textures[frameCounter];
        }
    }

    public void ReceiveData(string _data)
    {
        this.data = _data;

        switch (data)
        {
            case "imuReverse" :
                SequencePlayback(true);
                break;
            case "imuIdle" :
                SequencePlayback(false);
                break;
            case "imuObverse":
                SequencePlayback(false);
                break;
        }
    }

    public void SequencePlayback(bool revers)
    {
        if(isRevers != revers)
        {
            this.isRevers = revers;
            targetSpeed = 0.06f;
        }
    }

    public void ResetFrame()
    {
        frameCounter = 0;
        isPlaying = true;
    }

    public void StopPlaying()
    {
        isPlaying = false;
    }

    public void SequencePlayControl()
    {
        if (isPlaying)
        {
            traceText.InputTraceText("Sequence fade out and Deactivate");
            StartCoroutine(FadeOutAndDeactivate());
        }
        else
        {

        }

        //isPlaying = !isPlaying;
    }

    IEnumerator FadeOutAndDeactivate()
    {
        while (alpha > 0.0f)
        {
            alpha = alpha - Time.deltaTime;
            currColor.a = alpha;
            renderer.material.color = currColor;

            yield return null;
        }

        // stop play sequence
        StopPlaying();

        // Deactivate Sequence Screen
        this.gameObject.SetActive(false);
    }


    /****************************
     * Sequence play method
     ***************************/
    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        frameCounter = (++frameCounter) % textures.Length;
        StopCoroutine("PlayLoop");
    }

    // A method to play the sequence reverse
    IEnumerator PlayReverse(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (frameCounter <= 0) frameCounter = maxFrameCounter;
        frameCounter = (--frameCounter) % textures.Length;
        StopCoroutine("PlayReverse");
    }

    /* Probably not used */
    // A methos to play th sequence just once
    IEnumerator PlayOnce(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (frameCounter < textures.Length - 1)
        {
            ++frameCounter;
        }
        StopCoroutine("PlayOnce");
    }
}
