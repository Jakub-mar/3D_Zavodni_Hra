using TMPro;
using UnityEngine;

public class MainMenuBestTime : MonoBehaviour
{
    [SerializeField] private string bestTimeKey = "BEST_TIME_TRACK_1";
    [SerializeField] private TMP_Text bestTimeText;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!PlayerPrefs.HasKey(bestTimeKey))
        {
            bestTimeText.text = "Best time: --:--.---";
            return;
        }

        float t = PlayerPrefs.GetFloat(bestTimeKey);
        bestTimeText.text = "Best time: " + FormatTime(t);
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60f);
        float s = seconds % 60f;
        return $"{minutes:00}:{s:00.000}";
    }
}
