using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Auta na trati (stejné poøadí jako v menu!)")]
    public GameObject[] raceCars;

    [Header("Kamera")]
    public NewMonoBehaviourScript cameraScript; // Tvùj skript na kameøe

    void Start()
    {
        Debug.Log("CarSpawner hlásí: GameManager nám posílá index auta: " + GameManager.Instance.selectedCar);
        // 1. Zjistíme z GameManageru, co jsme vybrali
        int selectedIndex = GameManager.Instance.selectedCar;

        // 2. Aktivujeme správné auto
        for (int i = 0; i < raceCars.Length; i++)
        {
            if (i == selectedIndex)
            {
                raceCars[i].SetActive(true);

                // Propojíme kameru s aktivním autem
                if (cameraScript != null)
                {
                    cameraScript.target = raceCars[i].transform;
                }
            }
            else
            {
                raceCars[i].SetActive(false);
            }
        }
    }
}
