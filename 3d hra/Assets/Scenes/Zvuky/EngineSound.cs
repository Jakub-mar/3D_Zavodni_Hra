using UnityEngine;

public class EngineSound : MonoBehaviour
{
    public Rigidbody carRb;

    [Header("Pitch Settings")]
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;
    public float maxSpeedKmh = 200f;

    [Header("Volume Settings")]
    public float minVolume = 0.2f;
    public float maxVolume = 1.0f;

    private AudioSource engineAudio;

    private void Awake()
    {
        engineAudio = GetComponent<AudioSource>();
        
        if(carRb == null)
        {
            carRb = GetComponent<Rigidbody>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (carRb == null) return;

        float speedKmh = carRb.linearVelocity.magnitude * 3.6f;
        float t = Mathf.Clamp01(speedKmh / maxSpeedKmh);
        
        engineAudio.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        engineAudio.volume = Mathf.Lerp(minVolume, maxVolume, t);
    }
}
