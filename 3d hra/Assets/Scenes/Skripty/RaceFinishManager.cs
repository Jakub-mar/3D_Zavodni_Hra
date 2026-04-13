using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RaceFinishManager : MonoBehaviour
{
    [Header("UI Nastavení")]
    public GameObject leaderboardPanel;
    public TextMeshProUGUI resultsText;

    private List<RacerStatus> allRacers = new List<RacerStatus>();

    [System.Serializable]
    public class RacerStatus
    {
        public string name;
        public float time = 9999f;
        public bool hasFinished = false;
        public bool isPlayer = false;
        public LapSystem lapSystem; //  reference na konkrétní auto
    }

    void Start()
    {
        leaderboardPanel.SetActive(false);
        InitializeRacerList();
    }

    // Najde jen AKTIVNÍ auta ve scéně
    public void InitializeRacerList()
    {
        allRacers.Clear();

        LapSystem[] laps = FindObjectsOfType<LapSystem>();

        foreach (var l in laps)
        {
            allRacers.Add(new RacerStatus
            {
                name = l.racerName,
                isPlayer = l.isPlayer,
                lapSystem = l
            });

            Debug.Log($"Registruji auto: {l.racerName}");
        }
    }
    public void RefreshBestTimeDisplay()
    {
        if (resultsText == null) return;

        float best = PlayerPrefs.GetFloat("BestTime", 0);

        // jen přegeneruje spodní část textu
        string updated = resultsText.text;

        int index = updated.IndexOf("Tvůj nejlepší čas:");

        if (index != -1)
        {
            updated = updated.Substring(0, index);
        }

        updated += $"<size=90%>Tvůj nejlepší čas: {FormatTime(best)}</size>";

        resultsText.text = updated;
    }
    bool AllFinished()
    {
        return allRacers.All(r => r.hasFinished);
    }

    //  VOLÁ SE Z LapSystemu
    public void FinishRace(float finalTime, LapSystem sender)
    {
        Debug.Log("Dojel: " + sender.racerName + " čas: " + finalTime);

        var racer = allRacers.Find(r => r.lapSystem == sender);

        if (racer != null && !racer.hasFinished)
        {
            racer.time = finalTime;
            racer.hasFinished = true;
        }

        // Uložit best time hráče
        if (sender.isPlayer)
        {
            SaveBestTime(finalTime);
        }

        // Zobrazit až když všichni dojedou
        if (AllFinished())
        {
            leaderboardPanel.SetActive(true);
            UpdateLeaderboardUI();
        }
    }

    void UpdateLeaderboardUI()
    {
        var sorted = allRacers.OrderBy(r => r.time).ToList();

        string content = "<size=140%><b>VÝSLEDKY ZÁVODU</b></size>\n\n";

        for (int i = 0; i < sorted.Count; i++)
        {
            string timeStr = FormatTime(sorted[i].time);

            string nameDisplay = sorted[i].isPlayer
                ? $"<b>{sorted[i].name} (TY)</b>"
                : sorted[i].name;

            //  emoji
            string prefix = "";
            if (i == 0) prefix = "🥇 ";
            else if (i == 1) prefix = "🥈 ";
            else if (i == 2) prefix = "🥉 ";

            string line = $"{prefix}{i + 1}. {nameDisplay}\n   Time: {timeStr}";

            //  TOP 3 highlight
            if (i == 0)
                line = $"<mark=#FFD700AA><color=black>{line}</color></mark>";
            else if (i == 1)
                line = $"<mark=#C0C0C0AA><color=black>{line}</color></mark>";
            else if (i == 2)
                line = $"<mark=#CD7F32AA><color=black>{line}</color></mark>";
            else
                line = $"<color=white>{line}</color>";

            content += line + "\n\n";
        }

        float best = PlayerPrefs.GetFloat("BestTime", 0);
        content += $"<size=90%>Tvůj nejlepší čas: {FormatTime(best)}</size>";

        resultsText.text = content;
    }

    string FormatTime(float t)
    {
        if (t >= 9998) return "--:--:--";

        return string.Format("{0:00}:{1:00}:{2:00}",
            (int)t / 60,
            (int)t % 60,
            (int)((t * 100) % 100)
        );
    }

    void SaveBestTime(float t)
    {
        float oldBest = PlayerPrefs.GetFloat("BestTime", 9999f);

        if (t < oldBest)
        {
            PlayerPrefs.SetFloat("BestTime", t);
            PlayerPrefs.Save();
        }
    }
}