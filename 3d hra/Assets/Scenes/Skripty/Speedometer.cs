using UnityEngine;
using TMPro;
using System;

public class Speedometer : MonoBehaviour
{
    [Header("Propojení")]
    public Rigidbody carRb; // Rigidbody aktivního auta
    public TextMeshProUGUI speedText; // Textové pole v UI

    void Update()
    {
        // 1. AUTOMATICKÉ PROPOJENÍ: Pokud auto chybí, najdeme ho podle Tagu "Player"
        if (carRb == null || !carRb.gameObject.activeInHierarchy)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                carRb = playerObj.GetComponent<Rigidbody>();
            }
        }

        // Pokud stále nemáme auto, nepokračujeme (aby to neházelo chyby)
        if (carRb == null) return;

        // 2. VÝPOČET RYCHLOSTI
        // linearVelocity.magnitude dává rychlost v m/s, násobíme 3.6 pro km/h
        float speed = carRb.linearVelocity.magnitude * 3.6f;

        // 3. ZOBRAZENÍ TEXTU
        if (speedText != null)
        {
            speedText.text = Mathf.RoundToInt(speed) + " km/h";
        }
    }
}
