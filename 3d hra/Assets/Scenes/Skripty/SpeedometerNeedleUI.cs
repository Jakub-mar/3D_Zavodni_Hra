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
        if (carRb == null || !carRb.gameObject.activeInHierarchy)
        {
            GameObject activeCar = GameObject.FindGameObjectWithTag("Player");
            if (activeCar != null) carRb = activeCar.GetComponent<Rigidbody>();
        }

        if (carRb == null || needlePivot == null) return;

        float speedKmh = carRb.linearVelocity.magnitude * 3.6f;
        float t = Mathf.InverseLerp(0f, maxSpeedKmh, speedKmh);
        float rot = Mathf.Lerp(minRotation, maxRotation, t);

        // Nastavení rotace
        needlePivot.localRotation = Quaternion.Euler(0f, 0f, rot + angleOffset);

        // DEBUG: Tohle smaž, až to bude fungovat
        // Debug.Log("Rychlost: " + speedKmh + " | Rotace Z: " + (rot + angleOffset));

        if (speedText != null)
        {
            speedText.text = ((int)speedKmh).ToString() + " km/h";
        }
    }
}
