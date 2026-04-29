using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startMenu;
    public GameObject trackSelect;
    public GameObject bestTime;
    public GameObject optionMenu;
    public GameObject playerProfilePanel;
    public GameObject casePanel;


    void Start()
    {
        ShowMainMenu();
    }
    public void ShowCarSelect()
    {
        // Místo carSelect.SetActive(true) naèteme novou scénu
        // Ujisti se, že se scéna v Build Settings jmenuje pøesnì "Garage"
        SceneManager.LoadScene("CarSelect");
    }
    public void ShowOptions()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        bestTime.SetActive(false);
        optionMenu.SetActive(true);
        playerProfilePanel.SetActive(false);
    }
    public void ShowBestTime()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        bestTime.SetActive(true);
        optionMenu.SetActive(false);
        playerProfilePanel.SetActive(false);
    }
    public void ShowMainMenu()
    {

        mainMenu.SetActive(true);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        bestTime.SetActive(false);
            optionMenu.SetActive(false);
        playerProfilePanel.SetActive(false);
    }

    public void ShowStartMenu()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        trackSelect.SetActive(false);
        bestTime.SetActive(false);
        optionMenu.SetActive(false);
        playerProfilePanel.SetActive(false);
    }

    public void ShowTrackSelect()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false); 
        trackSelect.SetActive(true);
        bestTime.SetActive(false);
            optionMenu.SetActive(false);
        playerProfilePanel.SetActive(false);
    }
    public void ShowPlayerProfile()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        bestTime.SetActive(false);
        optionMenu.SetActive(false);

        playerProfilePanel.SetActive(true);
        casePanel.SetActive(false);
    }
    public void OpenCasePanel()
    {
        casePanel.SetActive(true);
    }

    public void CloseCasePanel()
    {
        casePanel.SetActive(false);
    }

}
