using TMPro;
using UnityEngine;

public class MainMenuBestTime : MonoBehaviour
{
    public GameObject bestTimesPanel;
    public TextMeshProUGUI bestTimeText;

    private const string BEST_TIME_KEY = "BestLapTime";

    public void OpenBestTimes()
    {
        bestTimesPanel.SetActive(true);

        if (PlayerPrefs.HasKey(BEST_TIME_KEY))
        {
            float time = PlayerPrefs.GetFloat(BEST_TIME_KEY);
            bestTimeText.text = "BEST TIME:\n" + FormatTime(time);
        }
        else
        {
            bestTimeText.text = "BEST TIME:\n--:--.--";
        }
    }

    public void CloseBestTimes()
    {
        bestTimesPanel.SetActive(false);
    }

    string FormatTime(float time)
    {
        int min = (int)(time / 60);
        float sec = time % 60;
        return min + ":" + sec.ToString("00.00");
    }
}
