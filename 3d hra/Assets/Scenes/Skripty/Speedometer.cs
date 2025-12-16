using UnityEngine;
using TMPro;
using System;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRb; // rigidbody auta
    public TextMeshProUGUI speedText;
    

    // Update is called once per frame
    void Update()
    {
        float speed = carRb.linearVelocity.magnitude * 3.6f; // p�evod z m/s na km/h
        speedText.text = Mathf.RoundToInt(speed) + " km/h";

    }
}
