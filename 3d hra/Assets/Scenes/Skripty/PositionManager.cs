using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PositionManager : MonoBehaviour
{
    public TextMeshProUGUI positionText;

    void Update()
    {
        LapSystem[] racers = FindObjectsByType<LapSystem>(
            FindObjectsSortMode.None);

        racers = racers
            .Where(r => r.gameObject.activeInHierarchy)
            .ToArray();

        List<LapSystem> sorted = racers
            .OrderByDescending(r => r.GetCurrentLap())
            .ThenByDescending(r => r.GetCheckpoint())
            .ToList();

        LapSystem player = sorted.Find(r => r.isPlayer);

        if (player != null)
        {
            int position = sorted.IndexOf(player) + 1;
            positionText.text = position + "/" + sorted.Count;
        }
    }
}