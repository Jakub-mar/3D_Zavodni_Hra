using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject trackSelectPanel; // Panel s tratìmi v menu

    [Header("Track Buttons")]
    public Button[] trackButtons;

    [Header("Start Button")]
    public GameObject startButton;

    void Start()
    {
        // Na zaèátku menu schováme tlaèítko Start, dokud se nevybere tra
        startButton.SetActive(false);

        // Nastavení listenerù pro tlaèítka tratí
        for (int i = 0; i < trackButtons.Length; i++)
        {
            int index = i;

            // --- PØIDÁNO: Vypnutí obrysu hned na zaèátku ---
            Outline outline = trackButtons[i].GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            // ----------------------------------------------

            trackButtons[i].onClick.RemoveAllListeners();
            trackButtons[i].onClick.AddListener(() => SelectTrack(index));
        }
    }

    void SelectTrack(int index)
    {
        // Uloíme index tratì do GameManageru
        GameManager.Instance.selectedTrack = index;
        startButton.SetActive(true);

        // Zvıraznìní vybraného tlaèítka
        for (int i = 0; i < trackButtons.Length; i++)
        {
            Outline outline = trackButtons[i].GetComponent<Outline>();
            if (outline != null) outline.enabled = (i == index);
        }
    }

    // Tato metoda se zavolá po kliknutí na "Start" pod vıbìrem tratí
    public void StartGame()
    {
        // Pøepneme do scény Garáe pøes GameManager
        GameManager.Instance.GoToGarage();
    }
}