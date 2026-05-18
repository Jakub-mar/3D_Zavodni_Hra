using UnityEngine;
using System.Collections.Generic;

public class CarSelector : MonoBehaviour
{
    public GameObject[] auta;

    private List<int> unlockedIndexes = new List<int>();
    private int currentIndex = 0;

    void Start()
    {
        LoadUnlockedCars();

        ShowCurrentCar();
    }

    void Update()
    {
        // RESET INVENTÁØE
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteKey("OwnedCars");
            PlayerPrefs.Save();

            Debug.Log("Inventáø smazán!");

            LoadUnlockedCars();
            currentIndex = 0;

            ShowCurrentCar();
        }
    }

    void LoadUnlockedCars()
    {
        unlockedIndexes.Clear();

        string ownedCars = PlayerPrefs.GetString("OwnedCars", "");

        for (int i = 0; i < auta.Length; i++)
        {
            // první 3 auta jsou vždy odemèené
            if (i < 3)
            {
                unlockedIndexes.Add(i);
            }
            // ostatní jen pokud byly vytoèené
            else if (ownedCars.Contains(auta[i].name))
            {
                unlockedIndexes.Add(i);
            }
        }
    }

    void ShowCurrentCar()
    {
        // vypni všechny auta
        for (int i = 0; i < auta.Length; i++)
        {
            auta[i].SetActive(false);
        }

        // pokud hráè nemá žádné auto
        if (unlockedIndexes.Count == 0)
        {
            Debug.Log("Nemáš žádné auto!");
            return;
        }

        auta[unlockedIndexes[currentIndex]].SetActive(true);
    }

    public void DalsiAuto()
    {
        if (unlockedIndexes.Count == 0) return;

        currentIndex++;

        if (currentIndex >= unlockedIndexes.Count)
            currentIndex = 0;

        ShowCurrentCar();
    }

    public void PredchoziAuto()
    {
        if (unlockedIndexes.Count == 0) return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = unlockedIndexes.Count - 1;

        ShowCurrentCar();
    }
}