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
    public GameObject hudCanvas;

    private List<Racer> racers = new List<Racer>();
    private PlayerProfile playerProfile;

    [System.Serializable]
    public class Racer
    {
        public LapSystem lapSystem;
        public string name;
        public float time;
        public bool isPlayer;
        public bool finished;
        public bool active;
        public int points;
    }

    void Start()
    {
        leaderboardPanel.SetActive(false);
        playerProfile = FindFirstObjectByType<PlayerProfile>();

       
    }

    // REGISTRACE AUT (jen aktivní)
    public void RegisterRacer(LapSystem lap)
    {
        if (!lap.gameObject.activeInHierarchy) return;

        racers.Add(new Racer
        {
            lapSystem = lap,
            name = lap.racerName,
            isPlayer = lap.isPlayer,
            time = 0,
            finished = false,
            active = true
        });
    }

    public void FinishRace(float time, LapSystem sender)
    {
        var r = racers.FirstOrDefault(x => x.lapSystem == sender);

        if (r != null)
        {
            r.time = time;
            r.finished = true;
        }

        if (sender.isPlayer)
        {
            SaveBestTime(time);
        }

        // ✔ čekáme jen na AKTIVNÍ auta
        if (AllActiveFinished())
        {
            ShowLeaderboard();

            if (hudCanvas != null)
                hudCanvas.SetActive(false);
        }
    }

    bool AllActiveFinished()
    {
        return racers
            .Where(r => r.active)
            .All(r => r.finished);
    }

    void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);

        foreach (Transform c in contentParent)
            Destroy(c.gameObject);

        var activeRacers = racers.Where(r => r.active).ToList();
        var sorted = activeRacers.OrderBy(r => r.time).ToList();

        GivePointsToPlayer(sorted);

        for (int i = 0; i < sorted.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);

            var ui = row.GetComponent<LeaderboardRow>();

            if (ui == null) continue;

            ui.position.text = (i + 1).ToString();
            ui.playerName.text = sorted[i].isPlayer ? "TY" : sorted[i].name;
            ui.time.text = FormatTime(sorted[i].time);
            ui.pointsText.text = "+" + sorted[i].points + " bodů";

            Image img = row.GetComponent<Image>();

            if (i == 0) img.color = new Color(1f, 0.84f, 0f, 0.3f);
            else if (i == 1) img.color = new Color(0.75f, 0.75f, 0.75f, 0.3f);
            else if (i == 2) img.color = new Color(0.8f, 0.5f, 0.2f, 0.3f);
            else img.color = new Color(1f, 1f, 1f, 0.05f);
        }

        RefreshBestTime();
    }

    string FormatTime(float t)
    {
        return string.Format("{0:00}:{1:00}:{2:00}",
            (int)t / 60,
            (int)t % 60,
            (int)((t * 100) % 100)
        );
    }

    void SaveBestTime(float t)
    {
        List<float> times = new List<float>();

        // načtení starých časů
        for (int i = 1; i <= 3; i++)
        {
            float saved = PlayerPrefs.GetFloat("BestTime" + i, 9999f);

            if (saved < 9999f)
                times.Add(saved);
        }

        // přidání nového času
        times.Add(t);

        // seřazení
        times = times.OrderBy(x => x).Take(3).ToList();

        // uložení TOP 3
        for (int i = 0; i < times.Count; i++)
        {
            PlayerPrefs.SetFloat("BestTime" + (i + 1), times[i]);
        }

        PlayerPrefs.Save();
    }

    public void RefreshBestTime()
    {
        float best = PlayerPrefs.GetFloat("BestTime1", -1);

        if (best < 0)
        {
            bestTimeText.text = "BEST TIME\n\n--:--:--";
        }
        else
        {
            bestTimeText.text = "BEST TIME\n\n" + FormatTime(best);
        }
    }

    void GivePointsToPlayer(List<Racer> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            int position = i + 1;

            int points = position == 1 ? 10 :
                         position == 2 ? 6 :
                         position == 3 ? 3 : 1;

            sorted[i].points = points;

            if (sorted[i].isPlayer)
            {
                playerProfile.AddPoints(points);

                bool isWinner = (i == 0); // první místo
                playerProfile.AddRace(isWinner);
            }
        }
    }
}