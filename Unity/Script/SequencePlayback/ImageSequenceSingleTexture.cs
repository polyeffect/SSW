﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequenceSingleTexture : MonoBehaviour
{
    private Texture texture;
    private Material goMaterial;
    private int frameCounter = 0;
    private int fc = 0;

    public string sequencePath;
    public string imageSequenceName;
    public int numberOfFrames;
    public string startNumber;
    private int sn;

    private string baseName;

    private void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
        this.baseName = "Sequence/" + this.sequencePath + "/" + this.imageSequenceName;
    }
    // Start is called before the first frame update
    void Start()
    {
        texture = (Texture)Resources.Load(baseName + startNumber, typeof(Texture));
        sn = int.Parse(startNumber);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("PlayLoop", 0.01f);
        goMaterial.mainTexture = this.texture;
    }

    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //advance one frame  
        frameCounter = (++frameCounter) % numberOfFrames;
        fc = frameCounter + sn;

        //load the current frame  
        this.texture = (Texture)Resources.Load(baseName + fc.ToString("D5"), typeof(Texture));

        //Stop this coroutine  
        StopCoroutine("PlayLoop");
    }

    //A method to play the sequence just once  
    IEnumerator Play(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //if it isn't the last frame  
        if (frameCounter < numberOfFrames - 1)
        {
            //Advance one frame  
            ++frameCounter;

            //load the current frame  
            this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        }

        //Stop this coroutine  
        StopCoroutine("Play");
    }
}
