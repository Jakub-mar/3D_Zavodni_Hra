using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int points;
    public int races;
    public int wins;
}

public class PlayerProfile : MonoBehaviour
{

    public int races;
    public int wins;

    public TextMeshProUGUI racesText;
    public TextMeshProUGUI winsText;
    public int points;
    public TextMeshProUGUI pointsText;
    public static PlayerProfile instance;

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/player.json";

        Load();      //  naète uložené body
        UpdateUI();
    }
    void Update()
    {
        // DEV CHEAT
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddPoints(100);
            Debug.Log("DEV: Pøidáno 100 bodù");
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;

        Save();      //  uloží body
        UpdateUI();
    }

    void UpdateUI()
    {
        if (pointsText != null)
            pointsText.text = "Body: " + points;

        if (racesText != null)
            racesText.text = "Races: " + races;

        if (winsText != null)
            winsText.text = "Wins: " + wins;
    }

    void Save()
    {
        PlayerData data = new PlayerData();
        data.points = points;
        data.races = races;
        data.wins = wins;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Uloženo do: " + savePath);
    }

    void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            points = data.points;
            races = data.races;
            wins = data.wins;

            Debug.Log("Naèteno");
        }
        else
        {
            points = 0;
            races = 0;
            wins = 0;
        }
    }
    public bool SpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;

            Save();
            UpdateUI();

            return true;
        }
        else
        {
            Debug.Log("Nemáš dost bodù!");
            return false;
        }
    }
    public void AddRace(bool win)
    {
        races++;

        if (win)
            wins++;

        Save();
        UpdateUI();
    }
}