using UnityEngine;

public class BestTimeReset : MonoBehaviour
{
    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey("BestTime");
        PlayerPrefs.Save();

        Debug.Log("Best time smazán!");

        // najdi manager (pokud existuje)
        RaceFinishManager manager = FindFirstObjectByType<RaceFinishManager>();

        if (manager != null)
        {
            manager.RefreshBestTimeDisplay();
        }
    }
}