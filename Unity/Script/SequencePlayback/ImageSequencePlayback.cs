using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequencePlayback : MonoBehaviour
{
    public string sequencePath;
    //public float delayTime = 0.01f;
    public float easing = 0.02f;
    private float targetSpeed = 0.01f;
    private float currentSpeed = 0.01f;

    private bool isRevers = false;
    private bool speedRevers = false;

    private Object[] objects;
    private Texture[] textures;
    private Material goMaterial;
    private int frameCounter = 0;
    private int maxFrameCounter = 0;

    private void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.objects = Resources.LoadAll("Sequence/" + sequencePath, typeof(Texture));
        this.textures = new Texture[objects.Length];

        maxFrameCounter = objects.Length;

        for (int i = 0; i < objects.Length; i++)
        {
            this.textures[i] = (Texture)this.objects[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        float delaySpeed = targetSpeed - currentSpeed;
        currentSpeed += delaySpeed * easing;
        //print(currentSpeed + ", " + targetSpeed);

        if (currentSpeed >= targetSpeed - 0.001f)
        {
            targetSpeed = 0.01f;
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

    public void SequencePlayback(bool revers)
    {
        if(isRevers != revers)
        {
            this.isRevers = revers;
            targetSpeed = 0.03f;
        }
    }

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
