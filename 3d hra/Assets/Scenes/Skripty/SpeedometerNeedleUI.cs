using TMPro;
using UnityEngine;

public class SpeedometerNeedleUI : MonoBehaviour
{

    [Header("Car")]
    public Rigidbody carRb;

    [Header("UI")]
    public RectTransform needlePivot;   // sem dej NeedlePivot
    public RectTransform needle;        // sem dej Needle (jen kvùli délce, není nutné)
    public TextMeshProUGUI speedText;   // volitelné

    [Header("Speed")]
    public float maxSpeedKmh = 260f;

    [Header("Needle Angles")]
    public float minAngle = -135f;  // 0 km/h
    public float maxAngle = 135f;   // maxSpeedKmh

    [Header("Smoothing")]
    public float smooth = 12f;

    float currentAngle;

    void Update()
    {
        if (!carRb || !needlePivot) return;

        float speedKmh = carRb.linearVelocity.magnitude * 3.6f;
        speedKmh = Mathf.Clamp(speedKmh, 0f, maxSpeedKmh);

        float t = speedKmh / maxSpeedKmh;
        float targetAngle = Mathf.Lerp(minAngle, maxAngle, t);

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * smooth);
        needlePivot.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

        if (speedText)
            speedText.text = Mathf.RoundToInt(speedKmh) + " km/h";
    }
}
