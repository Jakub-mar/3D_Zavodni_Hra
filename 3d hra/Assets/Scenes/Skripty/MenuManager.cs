using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startMenu;
    public GameObject trackSelect;
    public GameObject carSelect;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        carSelect.SetActive(false);
    }

    public void ShowStartMenu()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        trackSelect.SetActive(false);
        carSelect.SetActive(false);
    }

    public void ShowTrackSelect()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        trackSelect.SetActive(true);
        carSelect.SetActive(false);
    }

    public void ShowCarSelect()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        trackSelect.SetActive(false);
        carSelect.SetActive(true);
    }
    
}
