using System;
using TMPro;
using UnityEngine;

public class MainMenuBestTime : MonoBehaviour
{
    public TMP_Text bestTimeText;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        float best = PlayerPrefs.GetFloat("BestTime", -1);

        if (best < 0)
        {
            bestTimeText.text = "Best: --:--:---";
            return;
        }

        TimeSpan t = TimeSpan.FromSeconds(best);
        bestTimeText.text = "Best: " + t.ToString(@"mm\:ss\:fff");
    }
}
