using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.asyncUploadTimeSlice = 4;
        QualitySettings.asyncUploadBufferSize = 16;
        QualitySettings.asyncUploadPersistentBuffer = true;

        Screen.SetResolution(2800, 800, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrevScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void NextScene()
    {
        SceneManager.LoadScene("SequenceTest");
    }
}
