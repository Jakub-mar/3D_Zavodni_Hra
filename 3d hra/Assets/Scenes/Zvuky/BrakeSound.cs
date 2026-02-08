using UnityEngine;

public class BrakeSound : MonoBehaviour
{
    public Rigidbody carRb;
    public AudioSource brakeSource;

    public KeyCode brakeKey = KeyCode.Space;
    public float minSpeedKmh = 10f;

    void Awake()
    {
        if (carRb == null) carRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (carRb == null || brakeSource == null) return;

        float speedKmh = carRb.linearVelocity.magnitude * 3.6f;
        bool braking = Input.GetKey(brakeKey) && speedKmh > minSpeedKmh;

        if (braking)
        {
            if (!brakeSource.isPlaying) brakeSource.Play();
        }
        else
        {
            if (brakeSource.isPlaying) brakeSource.Stop();
        }
    }
}
