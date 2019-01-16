using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private PoemSequencePlayback poemSequencePlayback;
    private AlpahSequenceTextureArray alphaSequenceTextureArray;

    private string[] allSceneName = new string[] { "Summer", "Night", "Fall", "Winter" };
    private string crtSceneName = "Summer";
    private string prvSceneName = "Summer";
    
    private string data;

    private GameObject frontScene; // front scene
    private GameObject rearScene; // rear scene
    private GameObject backScene; // hidden scene

    void Start()
    {
        poemSequencePlayback = GameObject.Find("Poem Screen").GetComponent<PoemSequencePlayback>();
        alphaSequenceTextureArray = GameObject.Find("BG Alpha Mask").GetComponent<AlpahSequenceTextureArray>();

        frontScene = GameObject.Find("Summer");
        rearScene = GameObject.Find("Night");

        InitScene();
    }

    void Update()
    {
        
    }

    void InitScene()
    {
        frontScene.transform.position = new Vector3(0, 0, 1);
        frontScene.SendMessage("ResetFrame");
        rearScene.transform.position = new Vector3(0, 0, 2);
    }

    public void ReceiveData(string _data)
    {
        print("Receive Data From UDP Event Module: " + _data);
        this.data = _data;

        switch (data)
        {
            case "i01":
                ChangeScene(1);
                ChangePoem(1);
                break;
            case "i02":
                ChangeScene(2);
                ChangePoem(2);
                break;
            case "i03":
                ChangeScene(3);
                ChangePoem(3);
                break;
            case "i04":
                ChangeScene(1);
                ChangePoem(4);
                break;
            case "i05":
                ChangeScene(2);
                ChangePoem(5);
                break;
            case "i06":
                ChangeScene(1);
                ChangePoem(6);
                break;
            case "i07":
                ChangeScene(3);
                ChangePoem(7);
                break;
            case "i08":
                ChangeScene(2);
                ChangePoem(8);
                break;
            case "i09":
                ChangeScene(1);
                ChangePoem(9);
                break;
            case "i10":
                ChangeScene(3);
                ChangePoem(10);
                break;
            case "i11":
                ChangeScene(1);
                ChangePoem(11);
                break;
            case "i12":
                ChangeScene(2);
                ChangePoem(12);
                break;
            case "i13":
                ChangeScene(1);
                ChangePoem(13);
                break;
            case "i14":
                ChangeScene(3);
                ChangePoem(14);
                break;
            case "i15":
                ChangeScene(2);
                ChangePoem(15);
                break;
            case "i16":
                ChangeScene(4);
                ChangePoem(16);
                break;
        }
    }

    // Background Scene Change
    // Position Change
    public void ChangeScene(int _num)
    {
        switch (_num)
        {
            case 1: // Summer
                crtSceneName = "Summer";
                break;
            case 2: // Night
                crtSceneName = "Night";
                break;
            case 3: // Fall
                crtSceneName = "Fall";
                break;
            case 4: // Winter
                crtSceneName = "Winter";
                break;
        }

        if (crtSceneName != prvSceneName)
        {
            frontScene = GameObject.Find(crtSceneName);
            rearScene = GameObject.Find(prvSceneName);
            frontScene.SendMessage("ResetFrame");
            rearScene.SendMessage("StopLoopCounter");
            
            frontScene.transform.position = new Vector3(0, 0, 2);
            rearScene.transform.position = new Vector3(0, 0, 3);
            alphaSequenceTextureArray.ChangeScene(crtSceneName);

            for (int i = 0; i < 4; i++)
            {
                if (allSceneName[i] != crtSceneName && allSceneName[i] != prvSceneName)
                {
                    backScene = GameObject.Find(allSceneName[i]);
                    backScene.SendMessage("StopLoopCounter");
                    backScene.transform.position = new Vector3(0, 0, 4);
                }
            }
        }

        prvSceneName = crtSceneName;
    }

    public void ChangePoem(int _num)
    {
        poemSequencePlayback.ChangePoem(_num);
    }
}
