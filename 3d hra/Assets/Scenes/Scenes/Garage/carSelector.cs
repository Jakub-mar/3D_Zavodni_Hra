using UnityEngine;

public class CarSelector : MonoBehaviour
{
    public GameObject[] auta;
    private int aktualniIndex = 0;

    void Start()
    {
        // Na zaèátku zapne jen první auto a ostatní vypne
        for (int i = 0; i < auta.Length; i++)
        {
            auta[i].SetActive(i == aktualniIndex);
        }
    }

    public void DalsiAuto()
    {
        auta[aktualniIndex].SetActive(false);
        aktualniIndex++;
        if (aktualniIndex >= auta.Length) aktualniIndex = 0;
        auta[aktualniIndex].SetActive(true);
    }

    public void PredchoziAuto()
    {
        auta[aktualniIndex].SetActive(false);
        aktualniIndex--;
        if (aktualniIndex < 0) aktualniIndex = auta.Length - 1;
        auta[aktualniIndex].SetActive(true);
    }
}