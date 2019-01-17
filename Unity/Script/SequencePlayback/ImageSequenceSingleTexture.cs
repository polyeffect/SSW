using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequenceSingleTexture : MonoBehaviour
{
    private Texture texture;
    private Material goMaterial;
    private int frameCounter = 0;

    public string sequencePath;
    public string imageSequenceName;
    public int numberOfFrames;

    private string baseName;

    private void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
        this.baseName = "Sequence/Chapter3/BG/" + this.sequencePath + "/" + this.imageSequenceName;
    }

    void Start()
    {
        texture = (Texture)Resources.Load(baseName + "00000", typeof(Texture));
    }

    void Update()
    {
        StartCoroutine("PlayLoop", 0.01f);
        goMaterial.mainTexture = this.texture;
    }

    /****************************
    * Sequence play method
    ***************************/
    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        frameCounter = (++frameCounter) % numberOfFrames;
        this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        StopCoroutine("PlayLoop");
    }

    //A method to play the sequence just once  
    IEnumerator Play(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (frameCounter < numberOfFrames - 1)
        {
            ++frameCounter;
            this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        }
        StopCoroutine("Play");
    }
}
