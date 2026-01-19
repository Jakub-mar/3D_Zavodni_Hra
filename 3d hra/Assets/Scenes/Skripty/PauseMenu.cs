using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Transform startPoint;
    public GameObject playerCar;

    

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Resume(); // odpauzuje hru

        Time.timeScale = 1f;

        // 2. Naète aktuálnì aktivní scénu znovu od zaèátku
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void Quit()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);

        SceneManager.LoadScene("MainMenu"); // název scény
    }
}
