using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour
{
    [Header("Car Models in Garage")]
    public GameObject[] cars;
    private int currentIndex = 0;

    void OnEnable()
    {
        ShowCar(0); // Pøi zapnutí garáže ukáže první auto
    }

    public void NextCar()
    {
        currentIndex = (currentIndex + 1) % cars.Length;
        ShowCar(currentIndex);
    }

    public void PreviousCar()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = cars.Length - 1;
        ShowCar(currentIndex);
    }

    private void ShowCar(int index)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(i == index);
        }
        // Uložíme volbu do GameManageru
        GameManager.Instance.selectedCar = index;
    }

    // Tuto funkci nastav v OnClick u tlaèítka "START HRY" v garáži
    public void ConfirmSelectionAndStart()
    {
        GameManager.Instance.LaunchRace();
    }
    public void ReturnToMenu()
    {
        // Naète zpìt hlavní menu
        SceneManager.LoadScene("MainMenu");
    }
}
