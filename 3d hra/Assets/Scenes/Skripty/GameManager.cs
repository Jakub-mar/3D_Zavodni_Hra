using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string[] sceneNames; // Seznam závodních tratí
    public int selectedTrack = -1;
    public int selectedCar = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Pøenáší data mezi scénami
        }
        else { Destroy(gameObject); }
    }

    // Voláno z MainMenu po výbìru tratì
    public void GoToGarage()
    {
        SceneManager.LoadScene("CarSlecet"); // Název tvé nové scény s garáží
    }

    // Voláno z Garáže po výbìru auta
    public void LaunchRace()
    {
        if (selectedTrack >= 0 && selectedTrack < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[selectedTrack]); // Naète vybranou tra
        }
    }
}
