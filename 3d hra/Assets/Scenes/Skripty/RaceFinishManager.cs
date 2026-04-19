using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class RaceFinishManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject leaderboardPanel;
    public Transform contentParent;
    public GameObject rowPrefab;
    public TextMeshProUGUI bestTimeText;
    [Header("HUD")]
    public GameObject hudCanvas; // sem dáš tachometr, speed, timer

    private List<RacerStatus> allRacers = new List<RacerStatus>();

    [System.Serializable]
    public class RacerStatus
    {
        public string name;
        public float time;
        public bool hasFinished;
        public bool isPlayer;
        public LapSystem lapSystem;
    }

    void Start()
    {
        leaderboardPanel.SetActive(false);
        InitializeRacerList();
        
    }

    public void InitializeRacerList()
    {
        allRacers.Clear();

        LapSystem[] laps = FindObjectsByType<LapSystem>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        foreach (var l in laps)
        {
            allRacers.Add(new RacerStatus
            {
                name = l.racerName,
                isPlayer = l.isPlayer,
                lapSystem = l,
                time = 9999f,
                hasFinished = false
            });

            Debug.Log("Registruji: " + l.racerName + " player: " + l.isPlayer);
        }
    }

    public void FinishRace(float finalTime, LapSystem sender)
    {
        Debug.Log("FINISH: " + sender.racerName + " time: " + finalTime);

        var racer = allRacers.FirstOrDefault(r => r.lapSystem == sender);

        // když se nenajde, přidáme ho
        if (racer == null)
        {
            racer = new RacerStatus
            {
                name = sender.racerName,
                isPlayer = sender.isPlayer,
                lapSystem = sender,
                time = finalTime,
                hasFinished = true
            };

            allRacers.Add(racer);
        }
        else
        {
            racer.time = finalTime;
            racer.hasFinished = true;
        }

        // best time jen pro hráče
        if (sender.isPlayer)
        {
            SaveBestTime(finalTime);
        }

        if (AllFinished())
        {
            leaderboardPanel.SetActive(true);
            UpdateLeaderboardUI();

            if (hudCanvas != null)
                hudCanvas.SetActive(false);
        }
    }

    bool AllFinished()
    {
        return allRacers.Count > 0 && allRacers.All(r => r.hasFinished);
    }

    void UpdateLeaderboardUI()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var sorted = allRacers.OrderBy(r => r.time).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);

            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = (i + 1).ToString();
            texts[1].text = sorted[i].isPlayer ? "TY" : sorted[i].name;
            texts[2].text = FormatTime(sorted[i].time);

            Image img = row.GetComponent<Image>();

            if (i == 0)
                img.color = new Color(1f, 0.84f, 0f, 0.3f);
            else if (i == 1)
                img.color = new Color(0.75f, 0.75f, 0.75f, 0.3f);
            else if (i == 2)
                img.color = new Color(0.8f, 0.5f, 0.2f, 0.3f);
            else
                img.color = new Color(1f, 1f, 1f, 0.05f);

            if (sorted[i].isPlayer)
            {
                texts[1].fontStyle = FontStyles.Bold;
                row.transform.localScale = Vector3.one * 1.1f;
            }
        }

        float best = PlayerPrefs.GetFloat("BestTime", 9999f);
        bestTimeText.text = "Tvůj nejlepší čas: " + FormatTime(best);
    }

    string FormatTime(float t)
    {
        if (t >= 9998f) return "--:--:--";

        return string.Format("{0:00}:{1:00}:{2:00}",
            (int)t / 60,
            (int)t % 60,
            (int)((t * 100) % 100)
        );
    }

    public void RefreshBestTimeDisplay()
    {
        if (bestTimeText == null) return;

        if (!PlayerPrefs.HasKey("BestTime"))
        {
            bestTimeText.text = "Tvůj nejlepší čas: Žádný";
            return;
        }

        float best = PlayerPrefs.GetFloat("BestTime");
        bestTimeText.text = "Tvůj nejlepší čas: " + FormatTime(best);
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