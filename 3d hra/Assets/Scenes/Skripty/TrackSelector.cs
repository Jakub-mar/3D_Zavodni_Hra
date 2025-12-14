using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour
{
    [Header("Track Buttons")]
    public Button[] trackButtons;

    [Header("Start Button")]
    public GameObject startButton;

    [Header("Scene Names")]
    public string[] sceneNames;

    private int selectedTrack = -1;

    void Start()
    {
        // Skryj Start na zaèátku
        startButton.SetActive(false);

        // VYPNI VŠECHNY OUTLINE NA ZAÈÁTKU
        for (int i = 0; i < trackButtons.Length; i++)
        {
            Outline outline = trackButtons[i].GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;

            int index = i;
            trackButtons[i].onClick.AddListener(() => SelectTrack(index));
        }
    }

    void SelectTrack(int index)
    {
        selectedTrack = index;

        startButton.SetActive(true);

        for(int i = 0; i < trackButtons.Length; i++)
        {
            Outline outline = trackButtons[i].GetComponent<Outline>();
            outline.enabled = (i == index);
        }
    }

    public void StartGame()
    {
        if (selectedTrack>=0)
        {
           SceneManager.LoadScene(sceneNames[selectedTrack]);
        }
    }
}
