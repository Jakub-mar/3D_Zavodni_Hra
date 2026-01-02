using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour
{
    [Header("Track Buttons")]
    public Button[] trackButtons;

    [Header("Start Button")]
    public GameObject startButton;

    void Start()
    {
        startButton.SetActive(false);

        for (int i = 0; i < trackButtons.Length; i++)
        {
            int index = i;
            Outline outline = trackButtons[i].GetComponent<Outline>();
            if (outline != null) outline.enabled = false;

            // DŸLEéIT…: Nejd¯Ìv odstranÌme starÈ listenery, aby tam nebyly dvakr·t
            trackButtons[i].onClick.RemoveAllListeners();
            trackButtons[i].onClick.AddListener(() => SelectTrack(index));
        }
    }

    void SelectTrack(int index)
    {
        //nastavÌ traù v GameManageru
        GameManager.Instance.selectedTrack = index;

        startButton.SetActive(true);

        for (int i = 0; i < trackButtons.Length; i++)
        {
            Outline outline = trackButtons[i].GetComponent<Outline>();
            outline.enabled = (i == index);
        }
    }

    public void StartGame()
    {
        //jen zavol· GameManager
        GameManager.Instance.StartGame();
    }
}