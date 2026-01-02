using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string[] sceneNames;
    public int selectedTrack = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else Destroy(gameObject);
    }

    public void SelectTrack(int index)
    {
        selectedTrack = index;
    }

    public void StartGame()
    {
        if (selectedTrack >= 0 && selectedTrack < sceneNames.Length)
        {
            Time.timeScale = 1f; // Vždy resetuj èas pøed naèítáním
            SceneManager.LoadScene(sceneNames[selectedTrack]);
        }
    }

    public void ReturnToMainMenu()
    {
        selectedTrack = -1;
        SceneManager.LoadScene("MainMenu");
    }

}
