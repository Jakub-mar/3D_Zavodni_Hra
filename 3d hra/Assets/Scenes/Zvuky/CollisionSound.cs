using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource crashSource;
    public AudioClip crashClip;

    public float minImpact = 2f;     // minimální síla nárazu
    public float maxImpact = 15f;    // od této síly už je zvuk max

    private void OnCollisionEnter(Collision collision)
    {
        if (crashSource == null || crashClip == null) return;

        float impact = collision.relativeVelocity.magnitude;

        if (impact < minImpact) return;

        float volume = Mathf.InverseLerp(minImpact, maxImpact, impact);
        crashSource.PlayOneShot(crashClip, Mathf.Clamp01(volume));
    }
}
