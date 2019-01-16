using UnityEngine;

public class DebugControl : MonoBehaviour
{
    public GameObject stats;
    private bool isShow = true;

    public void StatsControl()
    {
        if (isShow)
        {
            stats.SetActive(false);
            isShow = !isShow;
        }
        else
        {
            stats.SetActive(true);
            isShow = !isShow;
        }
    }
}
