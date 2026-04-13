using UnityEngine;

public class BestTimeReset : MonoBehaviour
{
    [Header("Odkaz na leaderboard manager")]
    public RaceFinishManager raceFinishManager;

    // Tohle pøipojíš na Button OnClick
    public void ResetBestTime()
    {
        // smaže uložený èas
        PlayerPrefs.DeleteKey("BestTime");
        PlayerPrefs.Save();

        Debug.Log("Výsledky byly smazány!");

        // aktualizace UI (pokud je otevøené)
        if (raceFinishManager != null)
        {
            raceFinishManager.RefreshBestTimeDisplay();
        }
    }
}