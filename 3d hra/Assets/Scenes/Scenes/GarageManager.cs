using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GarageManager : MonoBehaviour
{
    [Header("Garáž Nastavení")]
    public GameObject[] cars;         // Seznam všech tvých modelù aut
    public Button startRaceButton;    // Tlaèítko pro spuštìní závodu
    public int startCarsCount = 3;    // KOLIK aut má hráè od zaèátku (napø. první 3)

    private int currentIndex = 0;
    private List<string> ownedCars = new List<string>();

    void Start()
    {
        LoadOwnedCars();
        ShowCar(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetInventory();
        }
    }

    void LoadOwnedCars()
    {
        ownedCars.Clear();

        // 1. AUTOMATICKÉ ODEMÈENÍ STARTOVNÍCH AUT
        // Projde prvních X aut v poli 'cars' a pøidá je do seznamu vlastnìných
        for (int i = 0; i < startCarsCount; i++)
        {
            if (i < cars.Length)
            {
                ownedCars.Add(cars[i].name);
            }
        }

        // 2. NAÈTENÍ VYHRANÝCH AUT Z DISKU
        if (PlayerPrefs.HasKey("OwnedCars"))
        {
            string data = PlayerPrefs.GetString("OwnedCars");
            string[] splitData = data.Split(',');
            foreach (string s in splitData)
            {
                if (!string.IsNullOrEmpty(s) && !ownedCars.Contains(s))
                {
                    ownedCars.Add(s);
                }
            }
        }
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
        // Aktivuje jen vybraný model
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(i == index);
        }

        // KONTROLA VLASTNICTVÍ
        // Tlaèítko Start se zapne jen pokud je jméno v seznamu ownedCars
        bool isOwned = ownedCars.Contains(cars[index].name);

        if (startRaceButton != null)
        {
            startRaceButton.interactable = isOwned;

            // VOLITELNÉ: Zmìna barvy tlaèítka, když je zamèeno
            startRaceButton.GetComponentInChildren<Text>().text = isOwned ? "START" : "ZAMÈENO";
        }

        if (isOwned && GameManager.Instance != null)
        {
            GameManager.Instance.selectedCar = index;
        }
    }

    public void ConfirmSelectionAndStart()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LaunchRace();
    }
    // Funkce pro smazání všech vyhraných aut
    public void ResetInventory()
    {
        // Smaže jen seznam vlastnìných aut z disku
        PlayerPrefs.DeleteKey("OwnedCars");
        PlayerPrefs.Save();

        // Okamžitì aktualizuje seznam v bìžící høe
        LoadOwnedCars();

        // Vrátí zobrazení na první auto, aby se refreshlo tlaèítko Start
        currentIndex = 0;
        ShowCar(currentIndex);

        Debug.Log("<color=red>Inventáø byl smazán!</color> Teï máš zase jen startovní auta.");
    }
    public void GoBackToTrackSelect()
    {
        SceneManager.LoadScene("MainMenu"); // uprav podle názvu tvý scény
    }
}