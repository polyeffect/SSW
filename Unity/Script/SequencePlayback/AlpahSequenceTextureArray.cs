using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlpahSequenceTextureArray : MonoBehaviour
{
    public string sequencePath;
    public float delayTime = 0.01f;

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
        StartCoroutine("PlayOnce", delayTime);

        goMaterial.SetTexture("_Mask", textures[frameCounter]);
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
        if (frameCounter < textures.Length - 1)
        {
            ++frameCounter;
        }
        StopCoroutine("PlayOnce");
    }
}
