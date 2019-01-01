using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetSceneName : MonoBehaviour
{
    public Text activeScene;

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        activeScene.text = scene.name;
    }
}
