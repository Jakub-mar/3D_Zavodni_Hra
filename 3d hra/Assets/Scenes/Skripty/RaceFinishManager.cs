using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RaceFinishManager : MonoBehaviour
{
    [Header("UI Nastavení")]
    public GameObject leaderboardPanel;
    public TextMeshProUGUI resultsText;

    // Seznam všech závodníkù
    private List<RacerStatus> allRacers = new List<RacerStatus>();

    [System.Serializable]
    public class RacerStatus
    {
        public string name;
        public float time = 9999f; // Výchozí èas pro ty, co ještì nedojeli
        public bool hasFinished = false;
        public bool isPlayer = false;
    }

    void Start()
    {
        leaderboardPanel.SetActive(false);
        InitializeRacerList();
    }

    // Tato metoda najde všechna auta, i když jsou na zaèátku vypnutá (v Main Menu)
    public void InitializeRacerList()
    {
        allRacers.Clear();

        // KLÍÈOVÁ OPRAVA: FindObjectsInactive.Include najde i tvé "vypnuté" auto
        LapSystem[] laps = Object.FindObjectsByType<LapSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var l in laps)
        {
            allRacers.Add(new RacerStatus
            {
                name = l.racerName,
                isPlayer = l.isPlayer
            });
            Debug.Log($"MANAGER: Registruji auto '{l.racerName}' (Hráè: {l.isPlayer})");
        }
    }

    // Metoda volaná z LapSystemu pøi prùjezdu cílem
    public void FinishRace(float finalTime, string name)
    {
        Debug.Log("MANAGER: Dojel " + name + " s èasem " + finalTime);

        // Najdeme správný slot v seznamu (podle jména a toho, že ještì nemá zapsaný èas)
        var racer = allRacers.Find(r => r.name == name && !r.hasFinished);

        if (racer != null)
        {
            racer.time = finalTime;
            racer.hasFinished = true;
        }
        else
        {
            Debug.LogWarning("MANAGER: Nemùžu najít volný slot pro jméno: " + name);
        }

        // Zapneme tabulku a aktualizujeme text
        leaderboardPanel.SetActive(true);
        UpdateLeaderboardUI();

        // Pokud to byl hráè, uložíme jeho rekord
        var currentPlayer = allRacers.Find(r => r.name == name && r.isPlayer);
        if (currentPlayer != null)
        {
            SaveBestTime(finalTime);
        }
    }

    void UpdateLeaderboardUI()
    {
        // Seøadíme všechny podle èasu (nejrychlejší nahoøe)
        // Ti, co mají 9999f (nedojeli), budou automaticky na konci
        var sorted = allRacers.OrderBy(r => r.time).ToList();

        string content = "<size=140%>VÝSLEDKY ZÁVODU</size>\n\n";

        for (int i = 0; i < sorted.Count; i++)
        {
            // Barvy pro stupnì vítìzù
            string color = "white";
            if (i == 0) color = "#FFD700"; // Zlatá
            else if (i == 1) color = "#C0C0C0"; // Støíbrná
            else if (i == 2) color = "#CD7F32"; // Bronzová

            string timeStr = FormatTime(sorted[i].time);

            // Pokud je to hráè, zvýrazníme ho tuènì a pøidáme (TY)
            string nameDisplay = sorted[i].isPlayer ? $"<b>{sorted[i].name} (TY)</b>" : sorted[i].name;

            content += $"<color={color}>{i + 1}. {nameDisplay} - {timeStr}</color>\n";
        }

        // Zobrazení nejlepšího èasu z PlayerPrefs
        float best = PlayerPrefs.GetFloat("BestTime", 0);
        content += $"\n<size=80%>Tvùj nejlepší èas: {FormatTime(best)}</size>";

        resultsText.text = content;
    }

    string FormatTime(float t)
    {
        if (t >= 9998) return "--:--:--";
        return string.Format("{0:00}:{1:00}:{2:00}", (int)t / 60, (int)t % 60, (int)((t * 100) % 100));
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