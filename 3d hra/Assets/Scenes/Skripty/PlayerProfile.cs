using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int points;
}

public class PlayerProfile : MonoBehaviour
{
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
    }

    void Save()
    {
        PlayerData data = new PlayerData();
        data.points = points;

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

            Debug.Log("Naèteno: " + points);
        }
        else
        {
            points = 0;
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
}