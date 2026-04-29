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

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/player.json";

        Load();      //  naète uložené body
        UpdateUI();
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
}