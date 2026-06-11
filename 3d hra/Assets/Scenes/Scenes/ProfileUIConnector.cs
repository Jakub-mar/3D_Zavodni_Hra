using TMPro;
using UnityEngine;

public class ProfileUIConnector : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI racesText;
    public TextMeshProUGUI winsText;

    void Start()
    {
        if (PlayerProfile.instance != null)
        {
            PlayerProfile.instance.pointsText = pointsText;
            PlayerProfile.instance.racesText = racesText;
            PlayerProfile.instance.winsText = winsText;

            PlayerProfile.instance.UpdateUI();
        }
    }
}