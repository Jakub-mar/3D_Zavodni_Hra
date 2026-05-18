using UnityEngine;
using TMPro;

public class BestTimeReset : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey("BestTime");
        PlayerPrefs.Save();

        bestTimeText.text = "Tvùj nejlepší èas: Žádný";

        Debug.Log("Best time smazán!");
    }
}