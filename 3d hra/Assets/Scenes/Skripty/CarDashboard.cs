using UnityEngine;
using TMPro;

public class CarDashboard : MonoBehaviour
{
    public Rigidbody rb;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gearText;

    [Header("Gear Settings")]
    public string[] gears = { "R", "N", "1", "2", "3", "4", "5" };
    public float[] gearSpeedLimits = { 20f, 0f, 40f, 70f, 110f, 160f, 250f };

    private int currentGear = 1; // 0=R, 1=N, 2=1. rychlost...

    void Update()
    {
        float speed = rb.linearVelocity.magnitude * 3.6f; // m/s → km/h
        speedText.text = Mathf.RoundToInt(speed) + " km/h";

        UpdateGear(speed);
        gearText.text = gears[currentGear];
    }

    void UpdateGear(float speed)
    {
        // Automatická převodovka styl GTA
        for (int i = 1; i < gearSpeedLimits.Length; i++)
        {
            if (speed < gearSpeedLimits[i])
            {
                currentGear = i;
                return;
            }
        }

        currentGear = gearSpeedLimits.Length - 1; // nejvyšší rychlost
    }
}
