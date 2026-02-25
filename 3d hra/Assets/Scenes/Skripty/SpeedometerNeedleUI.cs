using TMPro;
using UnityEngine;

public class SpeedometerNeedleUI : MonoBehaviour
{

    [Header("Car")]
    public Rigidbody carRb;

    [Header("Needle")]
    public RectTransform needlePivot;   // prázdný objekt ve středu
    public float maxSpeedKmh = 260f;

    [Header("Rotation Settings")]
    public float minRotation = 135f;    // pozice 0 km/h
    public float maxRotation = -135f;   // pozice max rychlosti
    public float angleOffset = 0f;      // doladění podle sprite

    [Header("Text (optional)")]
    public TMP_Text speedText;

    void Update()
    {
        if (carRb == null || needlePivot == null) return;

        // rychlost v km/h
        float speedKmh = carRb.linearVelocity.magnitude * 3.6f;

        // normalizace 0–1
        float t = Mathf.InverseLerp(0f, maxSpeedKmh, speedKmh);

        // výpočet rotace
        float rot = Mathf.Lerp(minRotation, maxRotation, t);

        // OTOČÍ SE POUZE PIVOT
        needlePivot.localRotation = Quaternion.Euler(0f, 0f, rot + angleOffset);

        // volitelný text
        if (speedText != null)
        {
            speedText.text = ((int)speedKmh).ToString() + " km/h";
        }
    }
}
