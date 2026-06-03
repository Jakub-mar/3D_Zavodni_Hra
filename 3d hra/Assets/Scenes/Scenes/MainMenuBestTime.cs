using System;
using TMPro;
using UnityEngine;

public class MainMenuBestTime : MonoBehaviour
{
    public TMP_Text bestTimeText;

    public int selectedTrack = 1;


    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        string text = "BEST TIMES - TRAç " + selectedTrack + "\n\n";

        for (int i = 1; i <= 3; i++)
        {
            float best = PlayerPrefs.GetFloat(
                "Track" + selectedTrack + "_BestTime" + i,
                -1
            );

            string line;

            if (best < 0)
            {
                line = i + ". --:--:---";
            }
            else
            {
                TimeSpan t = TimeSpan.FromSeconds(best);
                line = i + ". " + t.ToString(@"mm\:ss\:fff");
            }

            if (i == 1)
            {
                text += "<color=#FFD700><size=120%><b>" + line + "</b></size></color>\n";
            }
            else if (i == 2)
            {
                text += "<color=#C0C0C0>" + line + "</color>\n";
            }
            else
            {
                text += "<color=#CD7F32>" + line + "</color>\n";
            }
        }

        bestTimeText.text = text;
    }

    public void ShowTrack1()
    {
        selectedTrack = 1;
        Refresh();
    }

    public void ShowTrack2()
    {
        selectedTrack = 2;
        Refresh();
    }

    public void ShowTrack3()
    {
        selectedTrack = 3;
        Refresh();
    }
}
