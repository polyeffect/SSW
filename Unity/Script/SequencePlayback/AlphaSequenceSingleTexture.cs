using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaSequenceSingleTexture : MonoBehaviour
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
        this.baseName = "Sequence/" + this.sequencePath + "/" + this.imageSequenceName;
    }

    // Start is called before the first frame update
    void Start()
    {
        texture = (Texture)Resources.Load(baseName + "00000", typeof(Texture));
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("PlayOnce", 0.01f);
        goMaterial.SetTexture("_Mask", this.texture);
    }

    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //advance one frame  
        frameCounter = (++frameCounter) % numberOfFrames;

        //load the current frame  
        this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));

        //Stop this coroutine  
        StopCoroutine("PlayLoop");
    }

    //A method to play the sequence just once  
    IEnumerator PlayOnce(float delay)
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
