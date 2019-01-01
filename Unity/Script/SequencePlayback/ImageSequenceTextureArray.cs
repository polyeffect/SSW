using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequenceTextureArray : MonoBehaviour
{
    public string sequencePath;
    public float delayTime = 0.01f;
    private bool isRevers = false;

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
        if (isRevers) StartCoroutine("PlayReverse", delayTime);
        else StartCoroutine("PlayLoop", delayTime);

        goMaterial.mainTexture = textures[frameCounter];
    }

    public void SequencePlayback(bool revers)
    {
        this.isRevers = revers;
    }

    // A mrthod to play the sequence in a loop
    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);
        frameCounter = (++frameCounter) % textures.Length;
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

    // A method to play the sequence reverse
    IEnumerator PlayReverse(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (frameCounter <= 0) frameCounter = maxFrameCounter;
        frameCounter = (--frameCounter) % textures.Length;
        
        StopCoroutine("PlayReverse");
    }
}
