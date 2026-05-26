using UnityEngine;
using TMPro;

public class BestTimeReset : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;

    public void ResetBestTime()
    {
        PlayerPrefs.DeleteKey("BestTime1");
        PlayerPrefs.DeleteKey("BestTime2");
        PlayerPrefs.DeleteKey("BestTime3");
        PlayerPrefs.Save();

        bestTimeText.text =
    "BEST TIMES\n\n" +
    "1. --:--:---\n" +
    "2. --:--:---\n" +
    "3. --:--:---";

        Debug.Log("Best time smazán!");
    }
}