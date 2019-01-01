using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            slider.value = progress;
            progressText.text = progress * 100.0f + "%";

            yield return null;
        }
    }
}
